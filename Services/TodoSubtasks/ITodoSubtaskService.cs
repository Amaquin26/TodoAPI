using TodoAPI.DTOs.TodoSubtasks;

namespace TodoAPI.Services.TodoSubtasks;

public interface ITodoSubtaskService
{
    Task<IEnumerable<TodoSubtaskReadDto>> GetAllTodoSubtask();
    Task<IEnumerable<TodoSubtaskReadDto>> GetAllTodoSubtaskByTaskId(int taskId);
    Task<TodoSubtaskReadDto?> GetTodoSubtaskById(int id);
    Task<int> AddTodoSubtask(TodoSubtaskAddDto subtaskDto);
    Task UpdateTodoSubtask(TodoSubtaskUpdateDto subtaskDto);
    Task DeleteTodoSubtask(int id);
}