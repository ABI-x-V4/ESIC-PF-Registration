using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class District
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int StateId { get; set; }

    public virtual ICollection<EmpAddress> EmpAddressPmtDistricts { get; set; } = new List<EmpAddress>();

    public virtual ICollection<EmpAddress> EmpAddressPstDistricts { get; set; } = new List<EmpAddress>();

    public virtual ICollection<NomineeDetail> NomineeDetails { get; set; } = new List<NomineeDetail>();

    public virtual State State { get; set; } = null!;
}
