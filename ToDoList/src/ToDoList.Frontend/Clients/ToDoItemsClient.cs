namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemViews = new List<ToDoItemView>();
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");
            if (response == null)
            {
                return toDoItemViews;
            }
            toDoItemViews = [.. response.Select(dto => new ToDoItemView(
            dto.Id,
            dto.Name,
            dto.Description,
            dto.IsCompleted,
            dto.Category
            ))];
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
        }
        return toDoItemViews;
    }
    public async Task<ToDoItemView?> ReadItemByIdAsync(int itemId)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}")
                ?? throw new InvalidOperationException($"ToDoItem with id {itemId} not found.");
            var toDoItem = new ToDoItemView(
                response.Id,
                response.Name,
                response.Description,
                response.IsCompleted,
                response.Category
            );
            return toDoItem;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
            return null;
        }
    }

    public async Task UpdateItemAsync(ToDoItemView item)
    {
        try
        {
            var itemRequest = new TodoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);
            var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
        }

    }

    public async Task DeleteItemAsync(int itemId)
    {
        try
        {
            _ = await httpClient.DeleteAsync($"api/ToDoItems/{itemId}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.Message}");
        }
    }

    public async Task CreateItemAsync(ToDoItemView item)
    {
        try
        {
            var itemRequest = new ToDoItemCreateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);
            var response = await httpClient.PostAsJsonAsync($"api/ToDoItems", itemRequest);
        }
        catch (Exception e)
        {

            Console.WriteLine($"Exception occurred: {e.Message}");
        }
    }
}
