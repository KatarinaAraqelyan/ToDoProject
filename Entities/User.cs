namespace ToDoProject.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}