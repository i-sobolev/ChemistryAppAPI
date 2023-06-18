namespace ChemistryApp.Models;

public class UserInfo
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? CompletedTasksAmount { get; set; }
    public int? CompletedChaptersAmount { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}