using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class State
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual ICollection<EmpAddress> EmpAddressPmtStates { get; set; } = new List<EmpAddress>();

    public virtual ICollection<EmpAddress> EmpAddressPstStates { get; set; } = new List<EmpAddress>();

    public virtual ICollection<NomineeDetail> NomineeDetails { get; set; } = new List<NomineeDetail>();
}
