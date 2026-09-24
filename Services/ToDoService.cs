using ToDoProject.Data;
using ToDoProject.Dtos;
using ToDoProject.Entities;

namespace ToDoProject.Services;

public class ToDoService : IToDoService
{
    private readonly ToDoContext _context;

    public ToDoService(ToDoContext context)
    {
        _context = context;
    }

    public ToDo Create(CreateToDoDto dto, int userId)
    {
        var todo = new ToDo
        {
            Title = dto.Title,
            Description = dto.Description,
            IsPublic = dto.IsPublic,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.ToDos.Add(todo);
        _context.SaveChanges();

        return todo;
    }

    public ToDo? GetById(int id, int userId)
    {
        var todo = _context.ToDos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return null;
        }

        if (todo.UserId != userId && !todo.IsPublic)
        {
            return null;
        }

        return todo;
    }

    public List<ToDo> GetAll()
    {
        return _context.ToDos.ToList();
    }
    
    public bool Like(int id)
    {
        var todo = _context.ToDos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return false; 
        }

        todo.Likes += 1;
        _context.SaveChanges();
        return true;
    }

    public bool Dislike(int id)
    {
        var todo = _context.ToDos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return false;
        }

        todo.Dislikes += 1;
        _context.SaveChanges();
        return true;
    }

    public ToDo? SetVisibility(int id, bool isPublic, int userId)
    {
        var todo = _context.ToDos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return null;
        }

        if (todo.UserId != userId)
        {
            throw new UnauthorizedAccessException("Only the owner can change visibility.");
        }

        todo.IsPublic = isPublic;
        _context.SaveChanges();

        return todo;
    }
}