using Microsoft.AspNetCore.Mvc;
using TodoAPI.DTOs.TodoSubtasks;
using TodoAPI.DTOs.TodoTasks;
using TodoAPI.Repositories.TodoTasks;
using TodoAPI.Services.TodoSubtasks;
using TodoAPI.Services.TodoTasks;

namespace TodoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoSubaskController: ControllerBase
{
    private readonly ITodoSubtaskService _todoSubtaskService;
    
    public TodoSubaskController(ITodoSubtaskService todoSubtaskService)
    {
        _todoSubtaskService = todoSubtaskService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoSubtaskReadDto>>> GetAllTodoSubtask()
    {
        var subtasks = await _todoSubtaskService.GetAllTodoSubtask();
        return Ok(subtasks);
    }
    
    [HttpGet("task/{taskId:int}")]
    public async Task<ActionResult<IEnumerable<TodoSubtaskReadDto>>> GetAllTodoSubtask(int taskId)
    {
        var subtasks = await _todoSubtaskService.GetAllTodoSubtaskByTaskId(taskId);
        return Ok(subtasks);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoSubtaskReadDto>> GetTodoSubtaskById(int id)
    {
        var subtask = await _todoSubtaskService.GetTodoSubtaskById(id);
        return Ok(subtask);
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> AddTodoSubtask([FromBody] TodoSubtaskAddDto dto)
    {
        var newSubtaskId = await _todoSubtaskService.AddTodoSubtask(dto);
        return Ok(newSubtaskId);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTodoSubtask([FromBody] TodoSubtaskUpdateDto dto)
    {
        await _todoSubtaskService.UpdateTodoSubtask(dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTodoSubtask(int id)
    {
        await _todoSubtaskService.DeleteTodoSubtask(id);
        return NoContent();
    }
}