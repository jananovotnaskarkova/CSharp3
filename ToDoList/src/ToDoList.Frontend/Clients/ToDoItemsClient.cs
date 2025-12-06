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
    public async Task<ToDoItemView> ReadItemByIdAsync(int itemId)
    {
        var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}") ?? throw new InvalidOperationException($"ToDoItem with id {itemId} not found.");

        var toDoItem = new ToDoItemView(
            response.Id,
            response.Name,
            response.Description,
            response.IsCompleted,
            response.Category
        );

        return toDoItem;
    }

    public async Task UpdateItemAsync(ToDoItemView item)
    {
        var itemRequest = new TodoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);
        var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
    }

    public async Task DeleteItemAsync(int itemId) => await httpClient.DeleteAsync($"api/ToDoItems/{itemId}");

    public async Task CreateItemAsync(ToDoItemView item)
    {
        var itemRequest = new ToDoItemCreateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);
        var response = await httpClient.PostAsJsonAsync($"api/ToDoItems", itemRequest);
    }
}
