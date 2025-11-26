namespace ToDoList.Test.UnitTests;

using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerUnitTestBase
{
    protected IRepositoryAsync<ToDoItem> RepositoryMock { get; }
    protected ToDoItemsController Controller { get; }
    public ControllerUnitTestBase()
    {
        RepositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        Controller = new ToDoItemsController(RepositoryMock);
    }
}
