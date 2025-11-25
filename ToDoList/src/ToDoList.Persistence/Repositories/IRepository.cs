namespace ToDoList.Persistence.Repositories;

using System.Collections.Generic;
using ToDoList.Domain.DTOs;

public interface IRepository<T>
    where T : class
{
    public void Create(ToDoItemCreateRequestDto request);
    public IEnumerable<T> Read();
    public T? ReadById(int id);
    public T? UpdateById(int id, TodoItemUpdateRequestDto request);
    public bool DeleteById(int id);

}
