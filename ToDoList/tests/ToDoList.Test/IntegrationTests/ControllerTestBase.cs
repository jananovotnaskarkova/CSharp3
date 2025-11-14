namespace ToDoList.Test.IntegrationTests;

using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerTestBase : IDisposable
{
    protected ToDoItemsContext Context { get; private set; }
    protected IRepository<ToDoItem> Repository { get; private set; }
    protected ToDoItemsController Controller { get; private set; }
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
        GC.SuppressFinalize(this);
    }
}
