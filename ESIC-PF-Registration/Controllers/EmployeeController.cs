using DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.District;
using Repository.Employee;
using Repository.State;

namespace ESIC_PF_Registration.Controllers
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

        [Authorize]
        [HttpGet("GetAllEmpReg")]
        public async Task<IActionResult> GetAllEmpReg(int page = 1, int pageSize = 5, string? search = null, string? gender = null,
                                                      string? sortBy = null, string? sortDir = null)
        {

            var result = await _iemp.GetEmployeesPagedAsync(page, pageSize, search, gender, sortBy, sortDir);
            return View(result);

        }

        [Authorize]
        [HttpGet("GetEmpRegById/{id:int}")]
        public async Task<IActionResult> GetEmpRegById(int id)
        {
            var model = await _iemp.GetEmpById(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployeeReg()
        {
            var employeeSaved = Request.Cookies["EmployeeSaved"];

            if (employeeSaved == "true")
            {
                return RedirectToAction(nameof(ThankYouPage)); 
            }

            ViewData["ShowTab"] = true;
            var model = new EmployeeRegistrationDTO();
            var states = await _istate.GetAllStates();
            model.StateList = states.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.StateName
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployeeReg(EmployeeRegistrationDTO model)
        {
            ViewData["ShowTab"] = true; // ADDED

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


            var employeeId = await _iemp.SaveEmployee(model);

            if (employeeId > 0)
            {
                Response.Cookies.Append("EmployeeSaved","true",
                                new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = Request.IsHttps,
                                    SameSite = SameSiteMode.Lax,
                                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                                });

                return RedirectToAction(nameof(ThankYouPage));
               // return RedirectToAction("CreatePfEmployeeReg", "Pf");
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

        [HttpGet]
        public async Task<IActionResult> ExportEmployeeExcel(EmployeeListRowDto search)
        {
            var fileBytes = await _iemp.ExportEmployeeFullReportAsync(search);

            if (fileBytes == null || fileBytes.Length == 0)
            {
                TempData["ErrorMessage"] = "No data found for export.";
                return RedirectToAction("GetAllEmpReg");
            }

            string fileName = $"Employee_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private async Task AttachUploadedFilesAsync(EmployeeRegistrationDTO dto, EmployeeRegistrationDTO model)
        {
            if (model.PhotoFile != null && model.PhotoFile.Length > 0)
            {
                dto.PhotoPath = await SaveFileAsync(model.PhotoFile, "uploads/EmployeeESICReg/employee-photos");
            }

            if (model.AadhaarFile != null && model.AadhaarFile.Length > 0)
            {
                dto.AadhaarPath = await SaveFileAsync(model.AadhaarFile, "uploads/EmployeeESICReg/employee-aadhaar");
            }

            if (model.PanFile != null && model.PanFile.Length > 0)
            {
                dto.PanPath = await SaveFileAsync(model.PanFile, "uploads/EmployeeESICReg/employee-pan");
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
                                    await SaveFileAsync(file, "uploads/EmployeeESICReg/family-member-photos");
                            }
                        }
                    }
                }

                if (model.FamilyProofDocs != null && model.FamilyProofDocs.Count > 0)
                {
                    int proofFileIndex = 0;

                    for (int i = 0; i < dto.FamilyParticulars.Count; i++)
                    {
                        var familyMember = dto.FamilyParticulars[i];

                        if (familyMember.familyParticularsDocumentDTOs == null ||
                            familyMember.familyParticularsDocumentDTOs.Count == 0)
                        {
                            continue;
                        }

                        for (int j = 0; j < familyMember.familyParticularsDocumentDTOs.Count; j++)
                        {
                            if (proofFileIndex >= model.FamilyProofDocs.Count)
                                break;

                            var file = model.FamilyProofDocs[proofFileIndex];

                            if (file != null && file.Length > 0)
                            {
                                familyMember.familyParticularsDocumentDTOs[j].DocPath =
                                    await SaveFileAsync(
                                        file,
                                        "uploads/EmployeeESICReg/family-proof-docs"
                                    );

                                familyMember.familyParticularsDocumentDTOs[j].CreatedDate = DateTime.Now;
                            }

                            proofFileIndex++;
                        }
                    }
                }
                //if (model.FamilyProofDocs != null && model.FamilyProofDocs.Count > 0)
                //{
                //    for (int i = 0; i < dto.FamilyParticulars.Count; i++)
                //    {
                //        if (i < model.FamilyProofDocs.Count)
                //        {
                //            var file = model.FamilyProofDocs[i];
                //            if (file != null && file.Length > 0)
                //            {
                //                dto.FamilyParticulars[i].ProofDocPath =
                //                    await SaveFileAsync(file, "uploads/EmployeeReg/family-proof-docs");
                //            }
                //        }
                //    }
                //}
            }
            if (model.EmpBankDetails.BankDocFile != null && model.EmpBankDetails.BankDocFile.Length > 0)
            {
                dto.EmpBankDetails.BankDoc = await SaveFileAsync(model.EmpBankDetails.BankDocFile!, "uploads/EmployeeReg/employee-bankdoc");
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
