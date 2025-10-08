namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record TodoItemUpdateRequestDto(string Name, string Description, bool IsCompleted)
{
    public ToDoItem ToDomain() => new() { Name = Name, Description = Description, IsCompleted = IsCompleted };
}
