using System;
using System.Collections.Generic;

namespace ChemistryApp.Models;

public partial class UserCompletedTask
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompletedTaskId { get; set; }

    public virtual CompletedTask CompletedTask { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
