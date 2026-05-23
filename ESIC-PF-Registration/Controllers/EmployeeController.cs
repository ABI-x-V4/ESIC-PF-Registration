using DataModels;
using Insfrastructure.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.District;
using Repository.Employee;
using Repository.State;

namespace Insfrastructure.DbModels.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployee _iemp;
        private readonly IWebHostEnvironment _env;
        private readonly IDistrict _idist;
        private readonly IState _istate;

        public EmployeeController(IEmployee iemp, IWebHostEnvironment env, IDistrict idist, IState istate)
        {
            _iemp = iemp;
            _env = env;
            _idist = idist;
            _istate = istate;
        }


        [HttpGet("CreateEmployeeReg")]
        public async Task<IActionResult> CreateEmployeeReg()
        {
            var model = new EmployeeRegistrationDTO();
            var states = await _istate.GetAllStates();
            model.StateList = states.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),   
                Text = s.StateName
            }).ToList();

            return View(model);
        }

        [HttpPost("CreateEmployeeReg")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployeeReg(EmployeeRegistrationDTO model)
        {
            var states = await _istate.GetAllStates();
            model.StateList = states.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.StateName
            }).ToList();
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

            
            var result = await _iemp.SaveEmployee(model);

            if (result == "Success")
            {
                TempData["Message"] = "Employee created successfully.";
                return RedirectToAction(nameof(ThankYouPage)); 
            }

            TempData["Message"] = "Failed to create employee.";
            return View(model);
        }

        public IActionResult ThankYouPage()
        {
            return View();
        }

        [HttpGet("EditEmployeeReg/{id:int}")]
        public async Task<IActionResult> EditEmployeeReg(int id)
        {
            var dto = await _iemp.GetEmpById(id);
            return View(dto);
        }

        [HttpPut("EditEmployeeReg/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployeeReg(int id, EmployeeRegistrationDTO model)
        {
            var dto = model;
            dto.EmployeeId = id;  

            await AttachUploadedFilesAsync(dto, model);

            var result = await _iemp.SaveEmployee(dto); 

            TempData["Message"] = "Success";
            return RedirectToAction("Edit", new { id });
        }

        [HttpGet]
        public async Task<JsonResult> GetDistrictsByState(int stateId)
        {
            var districts = await _idist.GetDistrictsByStateId(stateId);
            return Json(districts);
        }

        private async Task AttachUploadedFilesAsync(EmployeeRegistrationDTO dto, EmployeeRegistrationDTO model )
        {
            if (model.PhotoFile != null && model.PhotoFile.Length > 0)
            {
                dto.PhotoPath = await SaveFileAsync(model.PhotoFile, "uploads/employee-photos");
            }
            
            if (model.AadhaarFile != null && model.AadhaarFile.Length > 0)
            {
                dto.AadhaarPath = await SaveFileAsync(model.AadhaarFile, "uploads/employee-aadhaar");
            }
            
            if (model.PanFile != null && model.PanFile.Length > 0)
            {
                dto.PanPath = await SaveFileAsync(model.PanFile, "uploads/employee-pan");
            }
            
            if (dto.FamilyParticulars != null && dto.FamilyParticulars.Count > 0)
            {
                if (model.FamilyMemberPhotos != null && model.FamilyMemberPhotos.Count > 0)
                {
                    for (int i = 0; i < dto.FamilyParticulars.Count; i++)
                    {
                        if (i < model.FamilyMemberPhotos.Count)
                        {
                            var file = model.FamilyMemberPhotos[i];
                            if (file != null && file.Length > 0)
                            {
                                dto.FamilyParticulars[i].MemberPhotoPath =
                                    await SaveFileAsync(file, "uploads/family-member-photos");
                            }
                        }
                    }
                }

                if (model.FamilyProofDocs != null && model.FamilyProofDocs.Count > 0)
                {
                    for (int i = 0; i < dto.FamilyParticulars.Count; i++)
                    {
                        if (i < model.FamilyProofDocs.Count)
                        {
                            var file = model.FamilyProofDocs[i];
                            if (file != null && file.Length > 0)
                            {
                                dto.FamilyParticulars[i].ProofDocPath =
                                    await SaveFileAsync(file, "uploads/family-proof-docs");
                            }
                        }
                    }
                }
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
