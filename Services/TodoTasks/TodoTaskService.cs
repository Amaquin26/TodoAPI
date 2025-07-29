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
        var tasks = await _todoTaskRepository.GetAllAsync();
        
        var taskDtos = tasks.Select(task => 
            new TodoTaskReadDto {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
            })
        .ToList();

        return taskDtos;
    }
    
    public async Task<TodoTaskReadDto?> GetTodoTaskById(int id)
    {
        var task = await _todoTaskRepository.GetByIdAsync(id);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {id} not found.");
        }

        var taskDto = new TodoTaskReadDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
        };

        return taskDto;
    }
    
    public async Task<int> AddTodoTask(TodoTaskAddDto taskDto)
    {
        var task = new TodoTask
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
        };

        await _todoTaskRepository.AddAsync(task);
        await _unitOfWork.CompleteAsync();
        return task.Id;
    }
    
    public async Task UpdateTodoTask(TodoTaskUpdateDto taskDto)
    {
        var existingTask = await _todoTaskRepository.GetByIdAsync(taskDto.Id);

        if (existingTask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {taskDto.Id} not found.");
        }
        
        existingTask.Title = taskDto.Title;
        existingTask.Description = taskDto.Description;

        _todoTaskRepository.Update(existingTask);
        await _unitOfWork.CompleteAsync();
    }
    
    public async Task DeleteTodoTask(int id)
    {
        var existingTask = await _todoTaskRepository.GetByIdAsync(id);
        
        if (existingTask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {id} not found.");
        }
        
        _todoTaskRepository.Remove(existingTask);
        await _unitOfWork.CompleteAsync();
    }
}