using DataModels;
using Microsoft.AspNetCore.Mvc;
using Repository.PF;

namespace ESIC_PF_Registration.Controllers
{
    public class PfController : Controller
    {
        private readonly IPfRegistration _ipf;
        private readonly IWebHostEnvironment _env;
        public PfController(IPfRegistration ipf, IWebHostEnvironment env)
        {
            _ipf = ipf;
            _env = env;
        }

        [HttpGet("CreatePfEmployeeReg")]
        public IActionResult CreatePfEmployeeReg()
        {
            var model = new PfRegistrationDTO();
            return View(model);
        }

        [HttpPost("CreatePfEmployeeReg")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePfEmployeeReg(PfRegistrationDTO model)
        {
            await AttachUploadedFilesAsync(model, model);
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .Select(x => $"{x.Key} => {string.Join(", ", x.Value!.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();
                TempData["Message"] = string.Join("\\n", errors);
                return View(model);
            }

            var result = await _ipf.SavePFEmployee(model);

            if (result == "Success")
            {
                TempData["Message"] = "Saved successfully.";
                return RedirectToAction(nameof(ResultPage));
            }

            TempData["Message"] = "Failed to save.";
            return View(model);
        }

        public IActionResult ResultPage()
        {
            return View();
        }

        private async Task AttachUploadedFilesAsync(PfRegistrationDTO dto, PfRegistrationDTO model)
        {
            if (model.AadhaarFile != null && model.AadhaarFile.Length > 0)
            {
                dto.Aadhaarpath = await SaveFileAsync(model.AadhaarFile, "uploads/PF_Reg/employee-aadhaar");
            }

            if (model.PanFile != null && model.PanFile.Length > 0)
            {
                dto.PanPath = await SaveFileAsync(model.PanFile, "uploads/PF_Reg/employee-pan");
            }
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext))
                ext = ".bin";

            var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}{ext}";

            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var targetFolder = Path.Combine(webRoot, folder.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var fullPath = Path.Combine(targetFolder, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{folder.TrimEnd('/')}/{fileName}";
        }
    }
}
