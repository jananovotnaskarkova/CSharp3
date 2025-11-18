namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class ToDoItemsRepository(ToDoItemsContext context) : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext context = context;

    public void Create(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    public IEnumerable<ToDoItem> Read() => [.. context.ToDoItems];

    public ToDoItem? ReadById(int id) => context.ToDoItems.Find(id);

    public ToDoItem? UpdateById(int id, TodoItemUpdateRequestDto request)
    {
        var itemUpdated = request.ToDomain();
        var item = context.ToDoItems.SingleOrDefault(i => i.ToDoItemId == id);

        if (item != null)
        {
            item.Name = itemUpdated.Name;
            item.Description = itemUpdated.Description;
            item.IsCompleted = itemUpdated.IsCompleted;
            context.SaveChanges();
        }
        return item;
    }

    public bool DeleteById(int id)
    {
        bool is_deleted;
        var item = context.ToDoItems.SingleOrDefault(i => i.ToDoItemId == id);

        if (item is null)
        {
            is_deleted = false;
        }
        else
        {
            context.ToDoItems.Remove(item);
            context.SaveChanges();
            is_deleted = true;
        }
        return is_deleted;
    }
}
