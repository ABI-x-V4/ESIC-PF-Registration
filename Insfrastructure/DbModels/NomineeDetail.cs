using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class NomineeDetail
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string Relationship { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int StateId { get; set; }

    public int DistrictId { get; set; }

    public int? Pincode { get; set; }

    public string? IsFamilyMember { get; set; }

    public virtual District District { get; set; } = null!;

    public virtual EmployeeRegistration Employee { get; set; } = null!;

    public virtual State State { get; set; } = null!;
}
