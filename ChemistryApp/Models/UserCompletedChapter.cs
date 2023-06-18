using System;
using System.Collections.Generic;

namespace ChemistryApp.Models;

public partial class UserCompletedChapter
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompletedChapterId { get; set; }

    public virtual CompletedChapter CompletedChapter { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
