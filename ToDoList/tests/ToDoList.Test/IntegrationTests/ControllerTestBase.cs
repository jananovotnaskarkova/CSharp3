namespace ToDoList.Test.IntegrationTests;

using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerTestBase : IDisposable
{
    protected readonly ToDoItemsContext Context;
    protected readonly IRepository<ToDoItem> Repository;
    protected readonly ToDoItemsController Controller;
    public ControllerTestBase()
    {
        Context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        Repository = new ToDoItemsRepository(Context);
        Controller = new ToDoItemsController(Repository);
    }

    public void Dispose()
    {
        Context.ToDoItems.RemoveRange(Context.ToDoItems.ToList());
        Context.SaveChanges();
        Context.Dispose();
    }
}
