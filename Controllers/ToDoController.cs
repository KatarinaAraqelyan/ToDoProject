using Microsoft.AspNetCore.Mvc;
using ToDoProject.Dtos;
using ToDoProject.Services;

namespace ToDoProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _toDoService;

    public ToDoController(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    [HttpPost]
    public IActionResult Create([FromForm] CreateToDoDto dto)
    {
        try
        {
            var result = _toDoService.Create(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Еrror occurred", error = ex.Message });
        }
    }

    [HttpGet("{index:int}")]
    public IActionResult GetById(int index)
    {
        try
        {
            var result = _toDoService.GetById(index);
            
            if (result == null)
            {
                return NotFound(new { message = $"ToDo with ID {index} not found" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Еrror occurred", error = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var result = _toDoService.GetAll();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Еrror occurred", error = ex.Message });
        }
    }

    [HttpPatch("{id}/like")]
    public IActionResult Like(int id)
    {
        try
        {
            var result = _toDoService.Like(id);

            if (!result)
            {
                return NotFound(new { message = $"ToDo with ID {id} not found" });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Еrror occurred", error = ex.Message });
        }
    }
}