using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class EmpAddress
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string PstAddress { get; set; } = null!;

    public int PstStateId { get; set; }

    public int PstDistrictId { get; set; }

    public int? PstPinCode { get; set; }

    public string PstMobile { get; set; } = null!;

    public string? PstEmail { get; set; }

    public string PmtAddress { get; set; } = null!;

    public int PmtStateId { get; set; }

    public int PmtDistrictId { get; set; }

    public int? PmtPinCode { get; set; }

    public string PmtMobile { get; set; } = null!;

    public string? PmtEmail { get; set; }

    public virtual EmployeeRegistration Employee { get; set; } = null!;

    public virtual District PmtDistrict { get; set; } = null!;

    public virtual State PmtState { get; set; } = null!;

    public virtual District PstDistrict { get; set; } = null!;

    public virtual State PstState { get; set; } = null!;
}
