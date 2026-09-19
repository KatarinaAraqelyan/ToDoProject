using ToDoProject.Dtos;
using ToDoProject.Entities;

namespace ToDoProject.Services;

public interface IToDoService
{
    ToDo Create(CreateToDoDto dto);
    ToDo? GetById(int id);
    List<ToDo> GetAll();
    bool Like(int id);
}