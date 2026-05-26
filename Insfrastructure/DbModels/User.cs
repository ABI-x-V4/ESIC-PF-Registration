using System;
using System.Collections.Generic;

namespace Insfrastructure.DbModels;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}
