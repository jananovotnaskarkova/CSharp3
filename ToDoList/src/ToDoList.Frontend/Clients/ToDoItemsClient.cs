namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    private readonly HttpClient httpClient = httpClient;

    public List<ToDoItemView> ReadItems()
    {
        var toDoItemViews = new List<ToDoItemView>();
        var response = httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");

        toDoItemViews = [.. response.Result.Select(dto => new ToDoItemView(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.IsCompleted
            ))];

        return toDoItemViews;
    }
}
