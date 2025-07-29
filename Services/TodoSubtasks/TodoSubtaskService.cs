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
        var subtasks = await _todoSubtaskRepository.GetAllAsync();
        
        var subtaskDtos = subtasks.Select(subtask =>
            new TodoSubtaskReadDto
            {
                Id = subtask.Id,
                TodoTaskId = subtask.TodoTaskId,
                Name = subtask.Name,
                IsChecked = subtask.IsChecked
            })
        .ToList();

        return subtaskDtos;
    }

    public async Task<IEnumerable<TodoSubtaskReadDto>> GetAllTodoSubtaskByTaskId(int taskId)
    {
        var subtasks = await _todoSubtaskRepository.GetAllByTodoTaskId(taskId);
        
        var subtaskDtos = subtasks.Select(subtask =>
            new TodoSubtaskReadDto
            {
                Id = subtask.Id,
                TodoTaskId = subtask.TodoTaskId,
                Name = subtask.Name,
                IsChecked = subtask.IsChecked
            })
        .ToList();

        return subtaskDtos;
    }

    public async Task<TodoSubtaskReadDto?> GetTodoSubtaskById(int id)
    {
        var subtask = await _todoSubtaskRepository.GetByIdAsync(id);

        if (subtask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {id} not found.");
        }

        var subtaskDto = new TodoSubtaskReadDto
        {
            Id = subtask.Id,
            TodoTaskId = subtask.TodoTaskId,
            Name = subtask.Name,
            IsChecked = subtask.IsChecked
        };

        return subtaskDto;
    }

    public async Task<int> AddTodoSubtask(TodoSubtaskAddDto subtaskDto)
    {
        var subtask = new TodoSubtask
        {
            Name = subtaskDto.Name,
            TodoTaskId = subtaskDto.TodoTaskId,
        };

        await _todoSubtaskRepository.AddAsync(subtask);
        await _unitOfWork.CompleteAsync();

        return subtask.Id;
    }

    public async Task UpdateTodoSubtask(TodoSubtaskUpdateDto subtaskDto)
    {
        var existingSubtask = await _todoSubtaskRepository.GetByIdAsync(subtaskDto.Id);

        if (existingSubtask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {subtaskDto.Id} not found.");
        }
        
        existingSubtask!.Name = subtaskDto.Name;
        
        _todoSubtaskRepository.Update(existingSubtask);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteTodoSubtask(int id)
    {
        var existingSubtask = await _todoSubtaskRepository.GetByIdAsync(id);
        
        if (existingSubtask == null)
        {
            throw new KeyNotFoundException($"Subtask with ID {id} not found.");
        }
        
        _todoSubtaskRepository.Remove(existingSubtask);
        await _unitOfWork.CompleteAsync();
    }
}