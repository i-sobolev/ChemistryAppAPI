using System;
using System.Collections.Generic;

namespace ChemistryApp.Models;

public partial class CompletedTask
{
    public int Id { get; set; }

    public virtual ICollection<UserCompletedTask> UserCompletedTasks { get; set; } = new List<UserCompletedTask>();
}
