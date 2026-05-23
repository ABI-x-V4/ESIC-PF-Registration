using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class DispenseryDetail
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string? DispensaryOrImpormEudforIp { get; set; }

    public int? StateIdForIp { get; set; }

    public int? DistrictIdForIp { get; set; }

    public string? DispensaryNameForIp { get; set; }

    public string? DispensaryOrImpormEudforFamily { get; set; }

    public int? StateIdForFamily { get; set; }

    public int? DistrictIdForFamily { get; set; }

    public string? DispensaryNameForFamily { get; set; }

    public virtual EmployeeRegistration Employee { get; set; } = null!;
}
