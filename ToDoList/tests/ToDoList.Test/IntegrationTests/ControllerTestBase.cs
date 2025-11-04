namespace ToDoList.Test.IntegrationTests;

using ToDoList.Persistence;
using ToDoList.WebApi;
public class ControllerTestBase : IDisposable
{
    protected readonly ToDoItemsController Controller;
    protected readonly ToDoItemsContext context;
    public ControllerTestBase()
    {
        context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        Controller = new ToDoItemsController(context);
    }

    public void Dispose()
    {
        context.ToDoItems.RemoveRange(context.ToDoItems.ToList());
        context.SaveChanges();
        context.Dispose();
    }
}
