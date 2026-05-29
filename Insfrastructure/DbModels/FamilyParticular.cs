using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class FamilyParticular
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string Relationship { get; set; } = null!;

    public string? Gender { get; set; }

    public string? ResidingWith { get; set; }

    public int? StateIdofResiding { get; set; }

    public int? DistrictIdofResiding { get; set; }

    public string? MemberPhotoPath { get; set; }

    public string? ProofDocPath { get; set; }

    public string? TypeOfProof { get; set; }

    public virtual EmployeeRegistration Employee { get; set; } = null!;
}
