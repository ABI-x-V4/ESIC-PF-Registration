using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class EmployeeRegistrationDTO
    {
        public int EmployeeId { get; set; }        
        public int? IsEsicAvailable { get; set; }
        [Required(ErrorMessage = "Is IP Disabled is required.")]
        public int IsEsicDisabled { get; set; }
        public string? TypeOfDisability { get; set; }
        [Required(ErrorMessage = "Name of IP is required.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "FATHER/ HUSBAND is required.")]
        public string FatherOrHusband { get; set; } = null!;

        [Required(ErrorMessage = "Name of  FATHER/ HUSBAND is required.")]
        public string FatherOrHusbandName { get; set; } = null!;
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime? Dob { get; set; }
        [Required(ErrorMessage = "Marital Status is required.")]
        public string MaritalStatus { get; set; } = null!;
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = null!;

        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhaar number must be exactly 12 digits.")]
        public string? AadhaarNo { get; set; }

        [RegularExpression(@"^[A-Z]{3}P[A-Z][0-9]{4}[A-Z]$", ErrorMessage = "Invalid PAN format")]
        public string? PanNo { get; set; }
        public string? PhotoPath { get; set; }
        public string? IpNumber { get; set; }
        public string? AadhaarPath { get; set; }
        public string? PanPath { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public IFormFile? AadhaarFile { get; set; }
        public IFormFile? PanFile { get; set; }
        public List<IFormFile>? FamilyMemberPhotos { get; set; } = new();
        public List<IFormFile>? FamilyProofDocs { get; set; } = new();
        public EmpAddressDTO EmpAddresses { get; set; } = new EmpAddressDTO();
        public DispenseryDetailDTO DispenseryDetails { get; set; } = new DispenseryDetailDTO();
        public EmploymentDetailDTO EmploymentDetails { get; set; } = new EmploymentDetailDTO();
        public List<NomineeDetailDTO> NomineeDetails { get; set; } = new List<NomineeDetailDTO>();
        public List<FamilyParticularDTO> FamilyParticulars { get; set; } = new List<FamilyParticularDTO>();
        public EmpBankDetailDTO EmpBankDetails { get; set; } = new EmpBankDetailDTO();
        public List<SelectListItem> StateList { get; set; } = new();
        public List<SelectListItem> DistrictList { get; set; } = new();


    }
    public class EmpAddressDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Present Address is required.")]
        public string PstAddress { get; set; } = null!;
        [Required(ErrorMessage = "Present State is required.")]
        public int PstStateId { get; set; }
        [Required(ErrorMessage = "Present District is required.")]
        public int PstDistrictId { get; set; }
        public string? PstStateName { get; set; }
        public string? PstDistrictName { get; set; }
        public int? PstPinCode { get; set; }
        [Required(ErrorMessage = "Personal Mobile No is required.")]
        public string PstMobile { get; set; } = null!;
        public string? PstEmail { get; set; }
        [Required(ErrorMessage = "Permanent Address is required.")]
        public string PmtAddress { get; set; } = null!;
        [Required(ErrorMessage = "Permanent State is required.")]
        public int PmtStateId { get; set; }
        [Required(ErrorMessage = "Permanent District is required.")]
        public int PmtDistrictId { get; set; }
        public string? PmtStateName { get; set; }
        public string? PmtDistrictName { get; set; }
        public int? PmtPinCode { get; set; }
        [Required(ErrorMessage = "Residence Mobile No is required.")]
        public string PmtMobile { get; set; } = null!;
        public string? PmtEmail { get; set; }
    }
    public class DispenseryDetailDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? DispensaryOrImpormEudforIp { get; set; }
        public int? StateIdForIp { get; set; }
        public int? DistrictIdForIp { get; set; }
        public string? StateNameForIp { get; set; }
        public string? DistrictNameForIp { get; set; }
        public string? DispensaryNameForIp { get; set; }
        public string? DispensaryOrImpormEudforFamily { get; set; }
        public int? StateIdForFamily { get; set; }
        public int? DistrictIdForFamily { get; set; }
        public string? StateNameForFamily { get; set; }
        public string? DistrictNameForFamily { get; set; }
        public string? DispensaryNameForFamily { get; set; }
    }
    public class EmploymentDetailDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Required (ErrorMessage ="Date of Joining of Current Employer is required.")]
        public DateTime? DojofCurrentEmployer { get; set; }
        public string? HasPreviousEmployer { get; set; }
        public string? EmployerCode { get; set; }
        public string? PreviousInsuarenceNo { get; set; }
        public string? EmployerName { get; set; }
        public string? EmployerAddress { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public string? StateName { get; set; }
        public string? DistrictName { get; set; }
        public int? Pincode { get; set; }
    }
    public class NomineeDetailDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime Dob { get; set; }
        [Required(ErrorMessage = "Relationship is required.")]
        public string Relationship { get; set; } = null!;
        [Required (ErrorMessage ="Address is required.")]
        public string Address { get; set; } = null!;
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "District is required.")]
        public int DistrictId { get; set; }
        public string? StateName { get; set; }
        public string? DistrictName { get; set; }
        public int? Pincode { get; set; }
        public string? IsFamilyMember { get; set; }
    }
    public partial class FamilyParticularDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime Dob { get; set; }
        [Required(ErrorMessage = "Relationship is required.")]
        public string Relationship { get; set; } = null!;
        public string? Gender { get; set; }
        public string? ResidingWith { get; set; }
        public int? StateIdofResiding { get; set; }
        public int? DistrictIdofResiding { get; set; }
        public string? StateName { get; set; }
        public string? DistrictName { get; set; }
        public string? MemberPhotoPath { get; set; }
        public string? ProofDocPath { get; set; }
    }
    public class EmpBankDetailDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Account Number is required.")]
        public string AccountNumber { get; set; } = null!;
        [Required(ErrorMessage = "Type of Account is required.")]
        public string TypeofAccount { get; set; } = null!;
        [Required (ErrorMessage = "Bank Name is required.")]
        public string BankName { get; set; } = null!;
        [Required(ErrorMessage = "Branch Name is required.")]
        public string BranchName { get; set; } = null!;
        public string? Micr { get; set; }
        [Required(ErrorMessage = "IFSC is required.")]
        public string Ifsc { get; set; } = null!;
    }
}
