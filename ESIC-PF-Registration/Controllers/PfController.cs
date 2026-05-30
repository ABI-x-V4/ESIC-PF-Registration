using DataModels;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("GetAllPf")]
        public async Task<IActionResult> GetAllPf(int page = 1, int pageSize = 5, string? search = null, string? gender = null,
                                                           string sortBy = "Name", string sortDir = "asc")
        {

            var result = await _ipf.GetPfEmployeesPagedAsync(page, pageSize, search, gender, sortBy, sortDir);
            return View(result);

        }

        [Authorize]
        [HttpGet("GetPfEmpRegById/{id:int}")]
        public async Task<IActionResult> GetPfEmpRegById(int id)
        {
            var model = await _ipf.GetPFEmpById(id);
            return View(model);
        }


        [HttpGet("CreatePfEmployeeReg")]
        public IActionResult CreatePfEmployeeReg(int employeeId)
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
               
                return View(model);
            }

            var result = await _ipf.SavePFEmployee(model);

            if (result == "Success")
            {
               // TempData["Message"] = "Saved successfully.";
                return RedirectToAction(nameof(ResultPage));
            }

            TempData["Message"] = "Failed to save.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExportPfEmployeeExcel(EmployeeListRowDto search)
        {
            var fileBytes = await _ipf.ExportPFEmployeeFullReportAsync(search);

            if (fileBytes == null || fileBytes.Length == 0)
            {
                TempData["ErrorMessage"] = "No data found for export.";
                return RedirectToAction("GetAllPf");
            }

            string fileName = $"Employee_PF_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
