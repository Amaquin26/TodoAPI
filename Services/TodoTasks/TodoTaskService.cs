using TodoAPI.DTOs.TodoTasks;
using TodoAPI.Repositories.TodoTasks;
using TodoAPI.UnitOfWork;
using TodoAPI.Entities;

namespace TodoAPI.Services.TodoTasks;

public class TodoTaskService: ITodoTaskService
{
    private readonly ITodoTaskRepository _todoTaskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TodoTaskService(ITodoTaskRepository todoTaskRepository, IUnitOfWork unitOfWork)
    {
        _todoTaskRepository = todoTaskRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<TodoTaskReadDto>> GetAllTodoTask()
    {
        var todoTasks = await _todoTaskRepository.GetAllAsync();
        
        var todoTaskDtos = todoTasks.Select(TodoTaskReadDto.MapTodoTaskToTodoTaskReadDto).ToList();

        return todoTaskDtos;
    }
    
    public async Task<TodoTaskReadDto?> GetTodoTaskById(int id)
    {
        var todoTask = await _todoTaskRepository.GetByIdAsync(id);

        if (todoTask == null)
        {
            throw new KeyNotFoundException($"Task with ID {id} not found.");
        }

        var todoTaskDto = TodoTaskReadDto.MapTodoTaskToTodoTaskReadDto(todoTask);
        
        return todoTaskDto;
    }
    
    public async Task<int> AddTodoTask(TodoTaskAddDto todoTaskDto)
    {
        var todoTask = new TodoTask
        {
            Title = todoTaskDto.Title,
            Description = todoTaskDto.Description,
        };

        await _todoTaskRepository.AddAsync(todoTask);
        await _unitOfWork.CompleteAsync();
        return todoTask.Id;
    }
    
    public async Task UpdateTodoTask(TodoTaskUpdateDto todoTaskDto)
    {
        var existingTodoTask = await _todoTaskRepository.GetByIdAsync(todoTaskDto.Id);

        if (existingTodoTask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {todoTaskDto.Id} not found.");
        }
        
        existingTodoTask.Title = todoTaskDto.Title;
        existingTodoTask.Description = todoTaskDto.Description;

        _todoTaskRepository.Update(existingTodoTask);
        await _unitOfWork.CompleteAsync();
    }
    
    public async Task DeleteTodoTask(int id)
    {
        var existingTodoTask = await _todoTaskRepository.GetByIdAsync(id);
        
        if (existingTodoTask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {id} not found.");
        }
        
        _todoTaskRepository.Remove(existingTodoTask);
        await _unitOfWork.CompleteAsync();
    }
}