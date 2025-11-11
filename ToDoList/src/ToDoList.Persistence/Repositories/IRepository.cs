namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public interface IRepository<T>
    where T : class
{
    public void Create(T item);
    public IEnumerable<ToDoItem> Read();
    public ToDoItem? ReadById(int toDoItemId);
    public ToDoItem? UpdateById(int toDoItemId, TodoItemUpdateRequestDto request);
    public bool DeleteById(int id);

}
