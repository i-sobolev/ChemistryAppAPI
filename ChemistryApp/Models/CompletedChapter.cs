using System;
using System.Collections.Generic;

namespace ChemistryApp.Models;

public partial class CompletedChapter
{
    public int Id { get; set; }

    public virtual ICollection<UserCompletedChapter> UserCompletedChapters { get; set; } = new List<UserCompletedChapter>();
}
