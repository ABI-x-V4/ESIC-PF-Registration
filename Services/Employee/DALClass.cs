using ClosedXML.Excel;
using DataModels;
using Insfrastructure.DbModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Employee;
using System.Data;
using System.Text;

namespace Services.Employee
{
    public class DALClass : IEmployee
    {
        private readonly EsicPfRegistrationDbContext _context;
        private readonly IConfiguration _configuration;
        public DALClass(EsicPfRegistrationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<EmployeeRegistrationDTO>> GetAllEmp()
        {
            var stateDict = await _context.States.ToDictionaryAsync(x => x.Id, x => x.Name);
            var districtDict = await _context.Districts.ToDictionaryAsync(x => x.Id, x => x.Name);

            var data = await _context.EmployeeRegistrations
                .Include(x => x.EmpAddresses)
                .Include(x => x.DispenseryDetails)
                .Include(x => x.EmploymentDetails)
                .Include(x => x.EmpBankDetails)
                .Include(x => x.NomineeDetails)
                .Include(x => x.FamilyParticulars)
                .Select(e => new EmployeeRegistrationDTO
                {
                    EmployeeId = e.EmployeeId,
                    IsEsicAvailable = e.IsEsicavailable,
                    IsEsicDisabled = e.IsEsicdisabled,
                    TypeOfDisability = e.TypeOfDisability,
                    Name = e.Name,
                    FatherOrHusband = e.FatherOrHusband,
                    FatherOrHusbandName = e.FatherOrHusbandName,
                    Dob = e.Dob,
                    MaritalStatus = e.MaritalStatus,
                    Gender = e.Gender,
                    AadhaarNo = e.AadhaarNo,
                    PanNo = e.PanNo,
                    PhotoPath = e.PhotoPath,
                    IpNumber = e.IpNumber,
                    AadhaarPath = e.AadhaarPath,
                    PanPath = e.PanPath,

                    EmpAddresses = e.EmpAddresses
                        .Select(a => new EmpAddressDTO
                        {
                            Id = a.Id,
                            EmployeeId = a.EmployeeId,
                            PstAddress = a.PstAddress,
                            PstStateId = a.PstStateId,
                            PstDistrictId = a.PstDistrictId,
                            PstStateName = stateDict.ContainsKey(a.PstStateId)
                                ? stateDict[a.PstStateId]
                                : null,
                            PstDistrictName = districtDict.ContainsKey(a.PstDistrictId)
                                ? districtDict[a.PstDistrictId]
                                : null,
                            PstPinCode = a.PstPinCode,
                            PstMobile = a.PstMobile,
                            PstEmail = a.PstEmail,

                            PmtAddress = a.PmtAddress,
                            PmtStateId = a.PmtStateId,
                            PmtDistrictId = a.PmtDistrictId,
                            PmtStateName = stateDict.ContainsKey(a.PmtStateId)
                                ? stateDict[a.PmtStateId]
                                : null,
                            PmtDistrictName = districtDict.ContainsKey(a.PmtDistrictId)
                                ? districtDict[a.PmtDistrictId]
                                : null,
                            PmtPinCode = a.PmtPinCode,
                            PmtMobile = a.PmtMobile,
                            PmtEmail = a.PmtEmail,
                            IsSameAddress = a.IsSameAddress
                        })
                        .FirstOrDefault() ?? new EmpAddressDTO(),

                    DispenseryDetails = e.DispenseryDetails
                        .Select(d => new DispenseryDetailDTO
                        {
                            Id = d.Id,
                            EmployeeId = d.EmployeeId,

                            DispensaryOrImpormEudforIp = d.DispensaryOrImpormEudforIp,
                            StateIdForIp = d.StateIdForIp,
                            DistrictIdForIp = d.DistrictIdForIp,
                            StateNameForIp = d.StateIdForIp.HasValue &&
                                             stateDict.ContainsKey(d.StateIdForIp.Value)
                                ? stateDict[d.StateIdForIp.Value]
                                : null,
                            DistrictNameForIp = d.DistrictIdForIp.HasValue &&
                                                districtDict.ContainsKey(d.DistrictIdForIp.Value)
                                ? districtDict[d.DistrictIdForIp.Value]
                                : null,
                            DispensaryNameForIp = d.DispensaryNameForIp,

                            DispensaryOrImpormEudforFamily = d.DispensaryOrImpormEudforFamily,
                            StateIdForFamily = d.StateIdForFamily,
                            DistrictIdForFamily = d.DistrictIdForFamily,
                            StateNameForFamily = d.StateIdForFamily.HasValue &&
                                                 stateDict.ContainsKey(d.StateIdForFamily.Value)
                                ? stateDict[d.StateIdForFamily.Value]
                                : null,
                            DistrictNameForFamily = d.DistrictIdForFamily.HasValue &&
                                                    districtDict.ContainsKey(d.DistrictIdForFamily.Value)
                                ? districtDict[d.DistrictIdForFamily.Value]
                                : null,
                            DispensaryNameForFamily = d.DispensaryNameForFamily
                        })
                        .FirstOrDefault() ?? new DispenseryDetailDTO(),

                    EmploymentDetails = e.EmploymentDetails
                        .Select(em => new EmploymentDetailDTO
                        {
                            Id = em.Id,
                            EmployeeId = em.EmployeeId,
                            DojofCurrentEmployer = em.DojofCurrentEmployer,
                            HasPreviousEmployer = em.HasPreviousEmployer,
                            EmployerCode = em.EmployerCode,
                            PreviousInsuarenceNo = em.PreviousInsuarenceNo,
                            EmployerName = em.EmployerName,
                            EmployerAddress = em.EmployerAddress,
                            StateId = em.StateId,
                            DistrictId = em.DistrictId,

                            StateName = em.StateId.HasValue &&
                                        stateDict.ContainsKey(em.StateId.Value)
                                ? stateDict[em.StateId.Value]
                                : null,

                            DistrictName = em.DistrictId.HasValue &&
                                           districtDict.ContainsKey(em.DistrictId.Value)
                                ? districtDict[em.DistrictId.Value]
                                : null,

                            Pincode = em.Pincode
                        })
                        .FirstOrDefault() ?? new EmploymentDetailDTO(),

                    EmpBankDetails = e.EmpBankDetails
                        .Select(b => new EmpBankDetailDTO
                        {
                            Id = b.Id,
                            EmployeeId = b.EmployeeId,
                            AccountNumber = b.AccountNumber,
                            TypeofAccount = b.TypeofAccount,
                            BankName = b.BankName,
                            BranchName = b.BranchName,
                            Micr = b.Micr,
                            Ifsc = b.Ifsc,
                            BankDoc = b.BankDoc
                        })
                        .FirstOrDefault() ?? new EmpBankDetailDTO(),

                    NomineeDetails = e.NomineeDetails
                        .Select(n => new NomineeDetailDTO
                        {
                            Id = n.Id,
                            EmployeeId = n.EmployeeId,
                            Name = n.Name,
                            Dob = n.Dob,
                            Relationship = n.Relationship,
                            Address = n.Address,
                            StateId = n.StateId,
                            DistrictId = n.DistrictId,

                            StateName = stateDict.ContainsKey(n.StateId)
                                ? stateDict[n.StateId]
                                : null,

                            DistrictName = districtDict.ContainsKey(n.DistrictId)
                                ? districtDict[n.DistrictId]
                                : null,

                            Pincode = n.Pincode,
                            IsFamilyMember = n.IsFamilyMember
                        }).ToList(),

                    FamilyParticulars = e.FamilyParticulars
                        .Select(f => new FamilyParticularDTO
                        {
                            Id = f.Id,
                            EmployeeId = f.EmployeeId,
                            Name = f.Name,
                            Dob = f.Dob,
                            Relationship = f.Relationship,
                            Gender = f.Gender,
                            ResidingWith = f.ResidingWith,
                            StateIdofResiding = f.StateIdofResiding,
                            DistrictIdofResiding = f.DistrictIdofResiding,

                            StateName = f.StateIdofResiding.HasValue &&
                                        stateDict.ContainsKey(f.StateIdofResiding.Value)
                                ? stateDict[f.StateIdofResiding.Value]
                                : null,

                            DistrictName = f.DistrictIdofResiding.HasValue &&
                                           districtDict.ContainsKey(f.DistrictIdofResiding.Value)
                                ? districtDict[f.DistrictIdofResiding.Value]
                                : null,

                            MemberPhotoPath = f.MemberPhotoPath,
                            familyParticularsDocumentDTOs = f.FamilyParticularsDocuments.Select(x => new FamilyParticularsDocumentDTO
                            {
                                Id = x.Id,
                                FamilyParticualrId = x.FamilyParticualrId,
                                DocName = x.DocName,
                                DocPath = x.DocPath
                            }).ToList()

                        }).ToList()
                })
                .ToListAsync();

            return data;
        }
        public async Task<EmployeeRegistrationDTO> GetEmpById(int id)
        {
            var data = await GetAllEmp();
            return data.FirstOrDefault(e => e.EmployeeId == id)!;
        }
        public async Task<PaginatedResult<EmployeeListRowDto>> GetEmployeesPagedAsync(int page, int pageSize, string? search, string? gender, string sortBy,
                                                                                      string sortDir)
        {


            var baseQuery = _context.EmployeeRegistrations
                            .AsNoTracking()
                            .Select(e => new
                            {
                                e.EmployeeId,
                                e.Name,
                                e.Gender,
                                e.Dob,
                                e.PanNo,
                                e.AadhaarNo,
                                Mobile = e.EmpAddresses.Select(a => a.PstMobile).FirstOrDefault(),
                                Email = e.EmpAddresses.Select(a => a.PstEmail).FirstOrDefault()
                            });

            var total = await baseQuery.CountAsync();

            if (!string.IsNullOrWhiteSpace(gender))
                baseQuery = baseQuery.Where(x => x.Gender == gender);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                baseQuery = baseQuery.Where(x =>
                    (x.Name ?? "").ToLower().Contains(search) ||
                    (x.Mobile ?? "").ToLower().Contains(search) ||
                    (x.Email ?? "").ToLower().Contains(search) ||
                    (x.PanNo ?? "").ToLower().Contains(search) ||
                    (x.AadhaarNo ?? "").ToLower().Contains(search) ||
                    (x.Gender ?? "").ToLower().Contains(search)
                );
            }

            var filtered = await baseQuery.CountAsync();


            if (string.IsNullOrWhiteSpace(sortBy))
            {
                baseQuery = baseQuery.OrderByDescending(x => x.EmployeeId);
            }
            else
            {
                bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

                baseQuery = (sortBy.ToLower()) switch
                {
                    "name" => desc
                        ? baseQuery.OrderByDescending(x => x.Name)
                        : baseQuery.OrderBy(x => x.Name),

                    "mobile" => desc
                        ? baseQuery.OrderByDescending(x => x.Mobile)
                        : baseQuery.OrderBy(x => x.Mobile),

                    "email" => desc
                        ? baseQuery.OrderByDescending(x => x.Email)
                        : baseQuery.OrderBy(x => x.Email),

                    "dob" => desc
                        ? baseQuery.OrderByDescending(x => x.Dob)
                        : baseQuery.OrderBy(x => x.Dob),

                    _ => baseQuery.OrderByDescending(x => x.EmployeeId)
                };
            }

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EmployeeListRowDto
                {
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    Mobile = x.Mobile,
                    Email = x.Email,
                    Gender = x.Gender,
                    Dob = x.Dob,
                    PanNo = x.PanNo,
                    AadhaarNo = x.AadhaarNo
                })
                .ToListAsync();

            return new PaginatedResult<EmployeeListRowDto>
            {
                Items = items,
                TotalRecords = total,
                FilteredRecords = filtered,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<int> SaveEmployee(EmployeeRegistrationDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                EmployeeRegistration employee;

                #region Personal Details
                if (dto.EmployeeId > 0)
                {
                    employee = await _context.EmployeeRegistrations
                        .Include(x => x.EmpAddresses)
                        .Include(x => x.DispenseryDetails)
                        .Include(x => x.EmploymentDetails)
                        .Include(x => x.EmpBankDetails)
                        .Include(x => x.NomineeDetails)
                        .Include(x => x.FamilyParticulars)
                        .ThenInclude(x => x.FamilyParticularsDocuments)
                        .FirstOrDefaultAsync(x => x.EmployeeId == dto.EmployeeId);

                    if (employee == null)
                    {
                        return 0;
                    }

                    employee.IsEsicavailable = dto.IsEsicAvailable;
                    employee.IsEsicdisabled = dto.IsEsicDisabled!.Value;
                    employee.TypeOfDisability = dto.TypeOfDisability;
                    employee.Name = dto.Name;
                    employee.FatherOrHusband = dto.FatherOrHusband;
                    employee.FatherOrHusbandName = dto.FatherOrHusbandName;
                    employee.Dob = dto.Dob!.Value;
                    employee.MaritalStatus = dto.MaritalStatus;
                    employee.Gender = dto.Gender;
                    employee.AadhaarNo = dto.AadhaarNo;
                    employee.PanNo = dto.PanNo;
                    employee.PhotoPath = dto.PhotoPath;
                    employee.IpNumber = dto.IpNumber;
                    employee.AadhaarPath = dto.AadhaarPath;
                    employee.PanPath = dto.PanPath;
                    employee.CreatedDate = DateTime.Now;
                }
                else
                {
                    employee = new EmployeeRegistration
                    {
                        IsEsicavailable = dto.IsEsicAvailable,
                        IsEsicdisabled = dto.IsEsicDisabled!.Value,
                        TypeOfDisability = dto.TypeOfDisability,
                        Name = dto.Name,
                        FatherOrHusband = dto.FatherOrHusband,
                        FatherOrHusbandName = dto.FatherOrHusbandName,
                        Dob = dto.Dob!.Value,
                        MaritalStatus = dto.MaritalStatus,
                        Gender = dto.Gender,
                        AadhaarNo = dto.AadhaarNo,
                        PanNo = dto.PanNo,
                        PhotoPath = dto.PhotoPath,
                        IpNumber = dto.IpNumber,
                        AadhaarPath = dto.AadhaarPath,
                        PanPath = dto.PanPath,
                        CreatedDate = DateTime.Now
                    };

                    _context.EmployeeRegistrations.Add(employee);
                    await _context.SaveChangesAsync();
                }

                int empId = employee.EmployeeId;

                #endregion

                #region Address
                var address = employee.EmpAddresses.FirstOrDefault();

                if (address == null)
                {
                    address = new EmpAddress
                    {
                        EmployeeId = empId
                    };

                    employee.EmpAddresses.Add(address);
                }

                address.PstAddress = dto.EmpAddresses.PstAddress;
                address.PstStateId = dto.EmpAddresses.PstStateId;
                address.PstDistrictId = dto.EmpAddresses.PstDistrictId;
                address.PstPinCode = dto.EmpAddresses.PstPinCode;
                address.PstMobile = dto.EmpAddresses.PstMobile;
                address.PstEmail = dto.EmpAddresses.PstEmail;

                address.PmtAddress = dto.EmpAddresses.PmtAddress;
                address.PmtStateId = dto.EmpAddresses.PmtStateId;
                address.PmtDistrictId = dto.EmpAddresses.PmtDistrictId;
                address.PmtPinCode = dto.EmpAddresses.PmtPinCode;
                address.PmtMobile = dto.EmpAddresses.PmtMobile;
                address.PmtEmail = dto.EmpAddresses.PmtEmail;
                address.IsSameAddress = dto.EmpAddresses.IsSameAddress;
                #endregion

                #region Dispensary 
                var dispensary = employee.DispenseryDetails.FirstOrDefault();

                if (dispensary == null)
                {
                    dispensary = new DispenseryDetail
                    {
                        EmployeeId = empId
                    };

                    employee.DispenseryDetails.Add(dispensary);
                }

                dispensary.DispensaryOrImpormEudforIp = dto.DispenseryDetails.DispensaryOrImpormEudforIp;
                dispensary.StateIdForIp = dto.DispenseryDetails.StateIdForIp;
                dispensary.DistrictIdForIp = dto.DispenseryDetails.DistrictIdForIp;
                dispensary.DispensaryNameForIp = dto.DispenseryDetails.DispensaryNameForIp;
                dispensary.DispensaryOrImpormEudforFamily = dto.DispenseryDetails.DispensaryOrImpormEudforFamily;
                dispensary.StateIdForFamily = dto.DispenseryDetails.StateIdForFamily;
                dispensary.DistrictIdForFamily = dto.DispenseryDetails.DistrictIdForFamily;
                dispensary.DispensaryNameForFamily = dto.DispenseryDetails.DispensaryNameForFamily;

                #endregion

                #region Employment Details
                var employment = employee.EmploymentDetails.FirstOrDefault();

                if (employment == null)
                {
                    employment = new EmploymentDetail
                    {
                        EmployeeId = empId
                    };

                    employee.EmploymentDetails.Add(employment);
                }

                employment.DojofCurrentEmployer = dto.EmploymentDetails.DojofCurrentEmployer!.Value;
                employment.HasPreviousEmployer = dto.EmploymentDetails.HasPreviousEmployer;
                employment.EmployerCode = dto.EmploymentDetails.EmployerCode;
                employment.PreviousInsuarenceNo = dto.EmploymentDetails.PreviousInsuarenceNo;
                employment.EmployerName = dto.EmploymentDetails.EmployerName;
                employment.EmployerAddress = dto.EmploymentDetails.EmployerAddress;
                employment.StateId = dto.EmploymentDetails.StateId;
                employment.DistrictId = dto.EmploymentDetails.DistrictId;
                employment.Pincode = dto.EmploymentDetails.Pincode;
                #endregion

                #region Bank Details
                var bank = employee.EmpBankDetails.FirstOrDefault();

                if (bank == null)
                {
                    bank = new EmpBankDetail
                    {
                        EmployeeId = empId
                    };

                    employee.EmpBankDetails.Add(bank);
                }

                bank.AccountNumber = dto.EmpBankDetails.AccountNumber;
                bank.TypeofAccount = dto.EmpBankDetails.TypeofAccount;
                bank.BankName = dto.EmpBankDetails.BankName;
                bank.BranchName = dto.EmpBankDetails.BranchName;
                bank.Micr = dto.EmpBankDetails.Micr;
                bank.Ifsc = dto.EmpBankDetails.Ifsc;
                bank.BankDoc = dto.EmpBankDetails.BankDoc;
                #endregion

                #region Nominee
                var existingNominees = employee.NomineeDetails.ToList();

                // delete removed
                foreach (var oldNominee in existingNominees)
                {
                    if (!dto.NomineeDetails.Any(x => x.Id == oldNominee.Id))
                    {
                        _context.NomineeDetails.Remove(oldNominee);
                    }
                }

                foreach (var item in dto.NomineeDetails)
                {
                    var nominee = employee.NomineeDetails
                        .FirstOrDefault(x => x.Id == item.Id);

                    if (nominee == null)
                    {
                        nominee = new NomineeDetail
                        {
                            EmployeeId = empId
                        };

                        employee.NomineeDetails.Add(nominee);
                    }

                    nominee.Name = item.Name;
                    nominee.Dob = item.Dob;
                    nominee.Relationship = item.Relationship;
                    nominee.Address = item.Address;
                    nominee.StateId = item.StateId;
                    nominee.DistrictId = item.DistrictId;
                    nominee.Pincode = item.Pincode;
                    nominee.IsFamilyMember = item.IsFamilyMember;
                }
                #endregion

                #region  Family  

                var existingFamily = employee.FamilyParticulars.ToList();

                // delete removed
                foreach (var oldFamily in existingFamily)
                {
                    if (!dto.FamilyParticulars.Any(x => x.Id == oldFamily.Id))
                    {
                        _context.FamilyParticulars.Remove(oldFamily);
                    }
                }

                foreach (var item in dto.FamilyParticulars)
                {
                    var family = employee.FamilyParticulars
                        .FirstOrDefault(x => x.Id == item.Id);

                    if (family == null)
                    {
                        family = new FamilyParticular
                        {
                            EmployeeId = empId
                        };

                        employee.FamilyParticulars.Add(family);
                    }

                    family.Name = item.Name;
                    family.Dob = item.Dob;
                    family.Relationship = item.Relationship;
                    family.Gender = item.Gender;
                    family.ResidingWith = item.ResidingWith;
                    family.StateIdofResiding = item.StateIdofResiding;
                    family.DistrictIdofResiding = item.DistrictIdofResiding;
                    family.MemberPhotoPath = item.MemberPhotoPath;


                    var existingDocs = family.FamilyParticularsDocuments.ToList();

                    foreach (var oldDoc in existingDocs)
                    {
                        if (!item.familyParticularsDocumentDTOs
                            .Any(x => x.Id == oldDoc.Id))
                        {
                            _context.FamilyParticularsDocuments.Remove(oldDoc);
                        }
                    }


                    foreach (var docItem in item.familyParticularsDocumentDTOs)
                    {
                        FamilyParticularsDocument? doc = null;

                        if (docItem.Id > 0)
                        {
                            doc = family.FamilyParticularsDocuments
                                .FirstOrDefault(x => x.Id == docItem.Id);
                        }

                        if (doc == null)
                        {
                            doc = new FamilyParticularsDocument();
                            family.FamilyParticularsDocuments.Add(doc);
                        }

                        doc.DocName = docItem.DocName;
                        doc.DocPath = docItem.DocPath;
                        doc.CreatedDate = DateTime.Now;
                    }


                }

                #endregion

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return employee.EmployeeId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }
        public async Task<byte[]> ExportEmployeeFullReportAsync(EmployeeListRowDto search)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var employeeQuery = BuildEmployeeQuery(search, out List<SqlParameter> employeeParams);
            var nomineeQuery = BuildNomineeQuery(search, out List<SqlParameter> nomineeParams);
            var familyQuery = BuildFamilyQuery(search, out List<SqlParameter> familyParams);

            DataTable employeeTable = await ExecuteQueryAsync(connectionString, employeeQuery, employeeParams);
            DataTable nomineeTable = await ExecuteQueryAsync(connectionString, nomineeQuery, nomineeParams);
            DataTable familyTable = await ExecuteQueryAsync(connectionString, familyQuery, familyParams);

            using var workbook = new XLWorkbook();

            AddWorksheet(workbook, employeeTable, "Employee Details");
            AddWorksheet(workbook, nomineeTable, "Nominee Details");
            AddWorksheet(workbook, familyTable, "Family Details");

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();

        }
        private static void AddWorksheet(XLWorkbook workbook, DataTable dt, string sheetName)
        {
            var ws = workbook.Worksheets.Add(dt, sheetName);

            // Header style
            var headerRange = ws.Range(1, 1, 1, dt.Columns.Count);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.RichBlack;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Columns().AdjustToContents();

            // Optional: freeze first row
            ws.SheetView.FreezeRows(1);
        }
        private async Task<DataTable> ExecuteQueryAsync(string connectionString, string query, List<SqlParameter> parameters)
        {
            var dt = new DataTable();

            await using var con = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(query, con);

            if (parameters != null && parameters.Count > 0)
                cmd.Parameters.AddRange(parameters.ToArray());

            await con.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();
            dt.Load(reader);

            return dt;
        }
        private string BuildEmployeeQuery(EmployeeListRowDto search, out List<SqlParameter> parameters)
        {
            parameters = new List<SqlParameter>();

            var sql = new StringBuilder(@" SELECT  e.EmployeeId, CASE WHEN e.IsESICAvailable = 1 THEN 'Yes' ELSE 'No' END AS [Is ESIC Available],
                                CASE WHEN e.IsESICDisabled = 1 THEN 'Yes' ELSE 'No' END AS [Is IP Disabled], e.TypeOfDisability [Type Of Disability], e.Name, e.FatherOrHusband [Father Or Husband],
                                e.FatherOrHusbandName [Father Or Husband Name], e.DOB, e.MaritalStatus [Marital Status],e.Gender, e.AadhaarNo, e.PanNo, e.IP_Number, ea.PstAddress [Present Address],
                                ps.Name AS [Present State], di.Name AS [Present District], ea.PstPinCode [Present PinCode], ea.PstMobile [Present Mobile], 
                                ea.PstEmail [Present Email], ea.PmtAddress [Permanent Address],
                                pms.Name AS [Permanent State], pmdi.Name AS [Permanent District], ea.PmtPinCode [Permanent PinCode],
                                ea.PmtMobile [Residence Mobile No], ea.PmtEmail [Permanent Email], d.DispensaryOrIMPOrmEUDForIP [Dispensary Or IMP Or mEUD For IP],
                                dsip.Name AS [State For IP], dsdip.Name AS [District For IP], d.DispensaryNameForIP [Dispensary Name For IP],
                                d.DispensaryOrIMPOrmEUDForFamily [Dispensary Or IMP Or mEUD For Family],
                                dsf.Name AS [State For Family], dsdf.Name AS [District For Family], d.DispensaryNameForFamily [Dispensary Name For Family],
                                ed.DOJofCurrentEmployer [Date of Appointment],
                                CASE WHEN ed.HasPreviousEmployer = 1 THEN 'Yes' ELSE 'No' END AS [Has Previous Employer], ed.EmployerCode [Employer Code],
                                ed.PreviousInsuarenceNo [Previous Insuarence No],
                                ed.EmployerName [Employer Name], ed.EmployerAddress [Employer Address], eds.Name AS [Employer State], edd.Name AS [Employer District], ed.Pincode,
                                eb.AccountNumber, eb.TypeofAccount, eb.BankName, eb.BranchName, eb.MICR, eb.IFSC
                                FROM EmployeeRegistration e
                                INNER JOIN EmpAddress ea ON e.EmployeeId = ea.EmployeeId
                                INNER JOIN DispenseryDetails d ON e.EmployeeId = d.EmployeeId
                                INNER JOIN EmploymentDetails ed ON e.EmployeeId = ed.EmployeeId
                                INNER JOIN States ps ON ps.Id = ea.PstStateId
                                INNER JOIN States pms ON pms.Id = ea.PmtStateId
                                INNER JOIN Districts di ON di.Id = ea.PstDistrictId
                                INNER JOIN Districts pmdi ON pmdi.Id = ea.PmtDistrictId
                                INNER JOIN States dsip ON dsip.Id = d.StateIdForIP
                                INNER JOIN States dsf ON dsf.Id = d.StateIdForFamily
                                INNER JOIN Districts dsdip ON dsdip.Id = d.DistrictIdForIP
                                INNER JOIN Districts dsdf ON dsdf.Id = d.DistrictIdForFamily
                                LEFT JOIN EmpBankDetails eb ON e.EmployeeId = eb.EmployeeId
                                LEFT JOIN States eds ON eds.Id = ed.StateId
                                LEFT JOIN Districts edd ON edd.Id = ed.DistrictId
                                WHERE 1=1 ");

            AppendCommonFilters(sql, parameters, search, "e", includeMobileFilter: true);

            sql.Append(" ORDER BY e.EmployeeId DESC");

            return sql.ToString();
        }
        private string BuildNomineeQuery(EmployeeListRowDto search, out List<SqlParameter> parameters)
        {
            parameters = new List<SqlParameter>();

            var sql = new StringBuilder(@"SELECT  n.EmployeeId, n.Name, n.DOB, n.Relationship, n.Address, s.Name AS State, d.Name AS District,
                                         n.Pincode, n.IsFamilyMember [Is Family Member]
                                        FROM EmployeeRegistration e
                                        INNER JOIN NomineeDetails n ON e.EmployeeId = n.EmployeeId
                                        INNER JOIN States s ON s.Id = n.StateId
                                        INNER JOIN Districts d ON d.Id = n.DistrictId
                                        WHERE 1=1 ");

            AppendCommonFilters(sql, parameters, search, "e", includeMobileFilter: false);

            sql.Append(" ORDER BY n.EmployeeId DESC");

            return sql.ToString();
        }
        private string BuildFamilyQuery(EmployeeListRowDto search, out List<SqlParameter> parameters)
        {
            parameters = new List<SqlParameter>();

            var sql = new StringBuilder(@" SELECT f.EmployeeId, f.Name, f.DOB, f.Relationship, f.Gender, f.ResidingWith [Residing With], s.Name AS State, d.Name AS District,
                                            fd.DocName [Type Of Proof]
                                            FROM EmployeeRegistration e
                                            INNER JOIN FamilyParticulars f ON e.EmployeeId = f.EmployeeId
                                            INNER JOIN FamilyParticularsDocuments fd on fd.FamilyParticualrId=f.Id
                                            INNER JOIN States s ON s.Id = f.StateIdofResiding
                                            INNER JOIN Districts d ON d.Id = f.DistrictIdofResiding
                                            WHERE 1=1 ");
            AppendCommonFilters(sql, parameters, search, "e", includeMobileFilter: false);

            sql.Append(" ORDER BY f.EmployeeId DESC");

            return sql.ToString();

        }
        private void AppendCommonFilters(StringBuilder sql, List<SqlParameter> parameters, EmployeeListRowDto search, string employeeAlias, 
                                         bool includeMobileFilter)
        {
            if (search.EmployeeId > 0)
            {
                sql.Append($" AND {employeeAlias}.EmployeeId = @EmployeeId");
                parameters.Add(new SqlParameter("@EmployeeId", search.EmployeeId));
            }

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                sql.Append($" AND {employeeAlias}.Name LIKE @Name");
                parameters.Add(new SqlParameter("@Name", $"%{search.Name.Trim()}%"));
            }

            if (!string.IsNullOrWhiteSpace(search.AadhaarNo))
            {
                sql.Append($" AND {employeeAlias}.AadhaarNo LIKE @AadhaarNo");
                parameters.Add(new SqlParameter("@AadhaarNo", $"%{search.AadhaarNo.Trim()}%"));
            }

            if (!string.IsNullOrWhiteSpace(search.PanNo))
            {
                sql.Append($" AND {employeeAlias}.PanNo LIKE @PanNo");
                parameters.Add(new SqlParameter("@PanNo", $"%{search.PanNo.Trim()}%"));
            }

            if (includeMobileFilter && !string.IsNullOrWhiteSpace(search.Mobile))
            {
                sql.Append(" AND (ea.PstMobile LIKE @MobileNo OR ea.PmtMobile LIKE @MobileNo)");
                parameters.Add(new SqlParameter("@MobileNo", $"%{search.Mobile.Trim()}%"));
            }
        }

    }
}