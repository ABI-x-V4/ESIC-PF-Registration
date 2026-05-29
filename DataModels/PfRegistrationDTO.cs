using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class PfRegistrationDTO
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        [Required(ErrorMessage = "Full Name is required.")]
        public string AadhaarFullName { get; set; } = null!;
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime? AadhaarDob { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string AadhaarGender { get; set; } = null!;
        [Required(ErrorMessage = "Aadhaar Number is required.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhaar must be exactly 12 digits")]
        public string AadhaarNo { get; set; } = null!;
        [Required(ErrorMessage = "Full Name is required.")]
        public string PanFullName { get; set; } = null!;
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime? PanDob { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string PanGender { get; set; } = null!;
        [Required(ErrorMessage = "PAN Number is required.")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format")]
        public string PanNo { get; set; } = null!;
        [Required(ErrorMessage = "Nationality is required.")]
        public string Nationality { get; set; } = null!;
        [Required(ErrorMessage = "Father/Husband Name is required.")]
        public string FatherOrHusbandname { get; set; } = null!;
        [Required(ErrorMessage = "Relation of beside  is required.")]
        public string FatherOrHusband { get; set; } = null!;
        [Required(ErrorMessage = "Marital Status is required.")]
        public string MaritalStatus { get; set; } = null!;
        public string? MobileNo { get; set; }
        public string? Emailid { get; set; }
        public string? Qualification { get; set; }
        [Required(ErrorMessage = "Date of Joining is required.")]
        public DateTime? Doj { get; set; }
        public string? Aadhaarpath { get; set; }
        public string? PanPath { get; set; }
        [Required(ErrorMessage = "UAN is required.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "UAN must be exactly 12 digits")]
        public string Uan { get; set; } = null!;
        [Required(ErrorMessage = "Aadhaar File is required.")]
        public IFormFile AadhaarFile { get; set; } = null!;
        [Required(ErrorMessage = "PAN File is required.")]
        public IFormFile PanFile { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
    }
}
