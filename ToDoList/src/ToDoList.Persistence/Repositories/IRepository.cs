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

public interface IRepositoryAsync<T>
    where T : class
{
    public Task Create(ToDoItemCreateRequestDto request);
    public Task<IEnumerable<T>> Read();
    public Task<T?> ReadById(int id);
    public Task<T?> UpdateById(int id, TodoItemUpdateRequestDto request);
    public Task<bool> DeleteById(int id);

}
