using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class EmployeeRegistration
{
    public int EmployeeId { get; set; }

    public int? IsEsicavailable { get; set; }

    public int IsEsicdisabled { get; set; }

    public string? TypeOfDisability { get; set; }

    public string Name { get; set; } = null!;

    public string FatherOrHusband { get; set; } = null!;

    public string FatherOrHusbandName { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string MaritalStatus { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? AadhaarNo { get; set; }

    public string? PanNo { get; set; }

    public string? PhotoPath { get; set; }

    public string? IpNumber { get; set; }

    public string? AadhaarPath { get; set; }

    public string? PanPath { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<DispenseryDetail> DispenseryDetails { get; set; } = new List<DispenseryDetail>();

    public virtual ICollection<EmpAddress> EmpAddresses { get; set; } = new List<EmpAddress>();

    public virtual ICollection<EmpBankDetail> EmpBankDetails { get; set; } = new List<EmpBankDetail>();

    public virtual ICollection<EmploymentDetail> EmploymentDetails { get; set; } = new List<EmploymentDetail>();

    public virtual ICollection<FamilyParticular> FamilyParticulars { get; set; } = new List<FamilyParticular>();

    public virtual ICollection<NomineeDetail> NomineeDetails { get; set; } = new List<NomineeDetail>();
}
