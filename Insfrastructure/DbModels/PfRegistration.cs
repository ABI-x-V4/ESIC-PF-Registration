using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class PfRegistration
{
    public int Id { get; set; }

    public string AadhaarFullName { get; set; } = null!;

    public DateTime AadhaarDob { get; set; }

    public string AadhaarGender { get; set; } = null!;

    public string AadhaarNo { get; set; } = null!;

    public string PanFullName { get; set; } = null!;

    public DateTime PanDob { get; set; }

    public string PanGender { get; set; } = null!;

    public string PanNo { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string FatherOrHusbandname { get; set; } = null!;

    public string FatherOrHusband { get; set; } = null!;

    public string MaritalStatus { get; set; } = null!;

    public string? MobileNo { get; set; }

    public string? Emailid { get; set; }

    public string? Qualification { get; set; }

    public DateTime Doj { get; set; }

    public string Aadhaarpath { get; set; } = null!;

    public string PanPath { get; set; } = null!;

    public string Uan { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public int? EmployeeId { get; set; }

    public virtual EmployeeRegistration? Employee { get; set; }
}
