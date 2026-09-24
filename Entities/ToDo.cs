namespace ToDoProject.Entities;

public class ToDo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;
    public bool IsPublic { get; set; } = false;
    public int UserId { get; set; }
    
}