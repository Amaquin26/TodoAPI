using TodoAPI.DTOs.TodoTasks;

namespace TodoAPI.Services.TodoTasks;

public interface ITodoTaskService
{
    Task<IEnumerable<TodoTaskReadDto>> GetAllTodoTask();
    Task<TodoTaskReadDto?> GetTodoTaskById(int id);
    Task<int> AddTodoTask(TodoTaskAddDto taskDto);
    Task UpdateTodoTask(TodoTaskUpdateDto taskDto);
    Task DeleteTodoTask(int id);
}