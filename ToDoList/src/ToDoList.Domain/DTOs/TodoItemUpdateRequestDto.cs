namespace ToDoList.Domain.DTOs;

public record TodoItemUpdateRequestDto(string Name, string Description, bool IsCompleted)
{

}
