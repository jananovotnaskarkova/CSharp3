namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemViews = new List<ToDoItemView>();
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems")
            ?? [];

        toDoItemViews = [.. response.Select(dto => new ToDoItemView(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.IsCompleted,
            dto.Category
            ))];

        return toDoItemViews;
    }
}
