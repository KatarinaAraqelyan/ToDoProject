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

    public ToDo Create(CreateToDoDto dto)
    {
        var todo = new ToDo
        {
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        _context.ToDos.Add(todo);
        _context.SaveChanges(); 

        return todo;
    }

    public ToDo? GetById(int id)
    {
        return _context.ToDos.FirstOrDefault(t => t.Id == id);
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
}