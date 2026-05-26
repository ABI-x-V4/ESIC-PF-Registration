using DataModels;
using Insfrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.Employee;

namespace Services.Employee
{
    public class DALClass : IEmployee
    {

        private readonly EsicPfRegistrationDbContext _context;
        public DALClass(EsicPfRegistrationDbContext context)
        {
            _context = context;
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
                            PmtEmail = a.PmtEmail
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
                            Ifsc = b.Ifsc
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
                            ProofDocPath = f.ProofDocPath
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
        public async Task<string> SaveEmployee(EmployeeRegistrationDTO dto)
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
                        .FirstOrDefaultAsync(x => x.EmployeeId == dto.EmployeeId);

                    if (employee == null)
                    {
                        return "Employee not found";
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
                    family.ProofDocPath = item.ProofDocPath;
                }

                #endregion

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Success";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }
    }
}
