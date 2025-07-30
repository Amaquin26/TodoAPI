using TodoAPI.DTOs.TodoSubtasks;
using TodoAPI.Repositories.TodoSubtasks;
using TodoAPI.UnitOfWork;
using TodoAPI.Entities;

namespace TodoAPI.Services.TodoSubtasks;

public class TodoSubtaskService: ITodoSubtaskService
{
    private readonly ITodoSubtaskRepository _todoSubtaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public TodoSubtaskService(ITodoSubtaskRepository todoSubtaskRepository, IUnitOfWork unitOfWork)
    {
        _todoSubtaskRepository = todoSubtaskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TodoSubtaskReadDto>> GetAllTodoSubtask()
    {
        var todoSubtasks = await _todoSubtaskRepository.GetAllAsync();
        
        var todoSubtaskDtos = todoSubtasks.Select(TodoSubtaskReadDto.MapTodoSubtaskToTodoSubtaskReadDto)
        .ToList();

        return todoSubtaskDtos;
    }

    public async Task<IEnumerable<TodoSubtaskReadDto>> GetAllTodoSubtaskByTaskId(int todoTaskId)
    {
        var todoSubtasks = await _todoSubtaskRepository.GetAllByTodoTaskId(todoTaskId);
        
        var todoSubtaskDtos = todoSubtasks.Select(TodoSubtaskReadDto.MapTodoSubtaskToTodoSubtaskReadDto)
        .ToList();

        return todoSubtaskDtos;
    }

    public async Task<TodoSubtaskReadDto?> GetTodoSubtaskById(int id)
    {
        var todoSubtask = await _todoSubtaskRepository.GetByIdAsync(id);

        if (todoSubtask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {id} not found.");
        }

        var todoSubtaskDto = TodoSubtaskReadDto.MapTodoSubtaskToTodoSubtaskReadDto(todoSubtask);

        return todoSubtaskDto;
    }

    public async Task<int> AddTodoSubtask(TodoSubtaskAddDto todoSubtaskDto)
    {
        var todoSubtask = new TodoSubtask
        {
            Name = todoSubtaskDto.Name,
            TodoTaskId = todoSubtaskDto.TodoTaskId,
        };

        await _todoSubtaskRepository.AddAsync(todoSubtask);
        await _unitOfWork.CompleteAsync();

        return todoSubtask.Id;
    }

    public async Task UpdateTodoSubtask(TodoSubtaskUpdateDto todoSubtaskDto)
    {
        var existingTodoSubtask = await _todoSubtaskRepository.GetByIdAsync(todoSubtaskDto.Id);

        if (existingTodoSubtask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {todoSubtaskDto.Id} not found.");
        }
        
        existingTodoSubtask!.Name = todoSubtaskDto.Name;
        
        _todoSubtaskRepository.Update(existingTodoSubtask);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteTodoSubtask(int id)
    {
        var existingTodoSubtask = await _todoSubtaskRepository.GetByIdAsync(id);
        
        if (existingTodoSubtask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {id} not found.");
        }
        
        _todoSubtaskRepository.Remove(existingTodoSubtask);
        await _unitOfWork.CompleteAsync();
    }
}