namespace TodoAPI.DTOs.TodoTasks;

public class TodoTaskReadDto
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}