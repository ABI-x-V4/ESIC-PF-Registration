using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class FamilyParticularsDocument
{
    public int Id { get; set; }

    public int FamilyParticualrId { get; set; }

    public string? DocName { get; set; }

    public string? DocPath { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual FamilyParticular FamilyParticualr { get; set; } = null!;
}
