using ToDoProject.Dtos;
using ToDoProject.Entities;

namespace ToDoProject.Services;

public interface IToDoService
{
    ToDo Create(CreateToDoDto dto, int userId);
    ToDo? GetById(int id,int userId);
    List<ToDo> GetAll();
    bool Like(int id);
    bool Dislike(int id);
    ToDo SetVisibility(int id, bool isPublic, int userId);
}