namespace ToDoList.Test.UnitTests;

using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerTestBaseUnitTests : IDisposable
{
    protected readonly ToDoItemsController Controller;
    public ControllerTestBaseUnitTests()
    {
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        Controller = new ToDoItemsController(context: null, repository: repositoryMock);
    }

    public void Dispose() { }
}
