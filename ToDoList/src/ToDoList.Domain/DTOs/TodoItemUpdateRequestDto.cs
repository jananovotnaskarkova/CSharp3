namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record TodoItemUpdateRequestDto(int Id, string Name, string Description, bool IsCompleted) //let client know the Id
{
    public ToDoItem ToDomain() => new() { ToDoItemId = Id, Name = Name, Description = Description, IsCompleted = IsCompleted };
}
