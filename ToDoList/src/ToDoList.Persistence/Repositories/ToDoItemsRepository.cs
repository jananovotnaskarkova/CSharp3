namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class ToDoItemsRepository : IRepository<ToDoItem>
{
    public readonly ToDoItemsContext context;
    public ToDoItemsRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public void Create(ToDoItem item)
    {
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    public List<ToDoItem> Read() => context.ToDoItems.ToList();

    public ToDoItem ReadById(int toDoItemId) => context.ToDoItems.Find(toDoItemId);

    public bool UpdateById(int toDoItemId, TodoItemUpdateRequestDto request)
    {
        var itemUpdated = request.ToDomain();
        var item = context.ToDoItems.SingleOrDefault(i => i.ToDoItemId == toDoItemId);

        if (item != null)
        {
            item.Name = itemUpdated.Name;
            item.Description = itemUpdated.Description;
            item.IsCompleted = itemUpdated.IsCompleted;
            context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool DeletById(int id)
    {
        var item = context.ToDoItems.SingleOrDefault(i => i.ToDoItemId == id);

        if (item != null)
        {
            context.ToDoItems.Remove(item);
            context.SaveChanges();
            return true;
        }
        return false;
    }


}
