using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class EmpBankDetail
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string TypeofAccount { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string BranchName { get; set; } = null!;

    public string? Micr { get; set; }

    public string Ifsc { get; set; } = null!;

    public string? BankDoc { get; set; }

    public virtual EmployeeRegistration Employee { get; set; } = null!;
}
