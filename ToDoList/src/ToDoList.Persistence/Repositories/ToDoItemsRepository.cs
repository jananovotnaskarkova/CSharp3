namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext context) : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext context = context;

    public async Task Create(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();
        await context.ToDoItems.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoItem>> Read() => await context.ToDoItems.ToListAsync();

    public async Task<ToDoItem?> ReadById(int id) => await context.ToDoItems.FindAsync(id);

    public async Task<ToDoItem?> UpdateById(int id, TodoItemUpdateRequestDto request)
    {
        var itemUpdated = request.ToDomain();
        var item = await context.ToDoItems.SingleOrDefaultAsync(i => i.ToDoItemId == id);

        if (item != null)
        {
            item.Name = itemUpdated.Name;
            item.Description = itemUpdated.Description;
            item.IsCompleted = itemUpdated.IsCompleted;
            await context.SaveChangesAsync();
        }
        return item;
    }

    public async Task<bool> DeleteById(int id)
    {
        bool is_deleted;
        var item = await context.ToDoItems.SingleOrDefaultAsync(i => i.ToDoItemId == id);

        if (item is null)
        {
            is_deleted = false;
        }
        else
        {
            context.ToDoItems.Remove(item);
            await context.SaveChangesAsync();
            is_deleted = true;
        }
        return is_deleted;
    }
}
