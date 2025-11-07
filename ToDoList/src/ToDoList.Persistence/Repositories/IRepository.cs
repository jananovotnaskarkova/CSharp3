namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public interface IRepository<T>
    where T : class
{
    public void Create(T item);
    public List<ToDoItem> Read();
    public ToDoItem? ReadById(int toDoItemId);
    public bool UpdateById(int toDoItemId, TodoItemUpdateRequestDto request);
    public bool DeletById(int id);

}
