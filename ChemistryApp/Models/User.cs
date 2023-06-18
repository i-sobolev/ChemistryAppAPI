using System;
using System.Collections.Generic;

namespace ChemistryApp.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public virtual ICollection<Friend> FriendFriendNavigations { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendUsers { get; set; } = new List<Friend>();

    public virtual ICollection<UserCompletedChapter> UserCompletedChapters { get; set; } = new List<UserCompletedChapter>();

    public virtual ICollection<UserCompletedTask> UserCompletedTasks { get; set; } = new List<UserCompletedTask>();
}
