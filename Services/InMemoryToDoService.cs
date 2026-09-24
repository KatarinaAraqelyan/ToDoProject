using ToDoProject.Dtos;
using ToDoProject.Entities;

namespace ToDoProject.Services;

public class InMemoryToDoService: IToDoService
{
    private readonly List<ToDo> Todos = new List<ToDo>();
    private int _id = 0;

    public ToDo Create(CreateToDoDto dto, int userId)
    {
        var todo = new ToDo
        {
            Id = ++_id,
            Title = dto.Title,
            Description = dto.Description,
            IsPublic = dto.IsPublic,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        Todos.Add(todo);
        return todo;
    }

    public ToDo? GetById(int id, int userId)
    {
        var ourToDo = Todos.FirstOrDefault(t => t.Id == id);
        if (ourToDo == null)
        {
            return null;
        }

        if (ourToDo.UserId != userId && !ourToDo.IsPublic)
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
    
    public bool Dislike(int id)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return false;
        }

        todo.Dislikes += 1;
        return true;
    }

    public ToDo? SetVisibility(int id, bool isPublic,int userId)
    {
        var todo = Todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return null;
        }

        todo.IsPublic = isPublic;
        return todo;
    }
}