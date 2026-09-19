using ToDoProject.Dtos;
using ToDoProject.Entities;

namespace ToDoProject.Services;

public class InMemoryToDoService: IToDoService
{
    private readonly List<ToDo> Todos = new List<ToDo>();
    private int _id = 0;

    public ToDo Create(CreateToDoDto dto)
    {
        var todo = new ToDo
        {
            Id = ++_id,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        Todos.Add(todo);
        return todo;
    }

    public ToDo GetById(int id)
    {
        
        ToDo ourToDo =  Todos.FirstOrDefault(t => t.Id == id);

        if (ourToDo == null)
        {
            return null;
        }

        return ourToDo;
    }

    public List<ToDo> GetAll()
    {
        return Todos;
    }
    
    public bool Like(int id)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return false;
        }

        todo.Likes += 1;
        return true;
    }
    
}