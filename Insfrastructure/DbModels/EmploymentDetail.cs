using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class EmploymentDetail
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateTime DojofCurrentEmployer { get; set; }

    public string? HasPreviousEmployer { get; set; }

    public string? EmployerCode { get; set; }

    public string? PreviousInsuarenceNo { get; set; }

    public string? EmployerName { get; set; }

    public string? EmployerAddress { get; set; }

    public int? StateId { get; set; }

    public int? DistrictId { get; set; }

    public int? Pincode { get; set; }

    public virtual EmployeeRegistration Employee { get; set; } = null!;
}
