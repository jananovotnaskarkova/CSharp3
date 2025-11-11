namespace ToDoList.Test.UnitTests;

using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerUnitTestBase
{
    protected readonly IRepository<ToDoItem> RepositoryMock;
    protected readonly ToDoItemsController Controller;
    public ControllerUnitTestBase()
    {
        RepositoryMock = Substitute.For<IRepository<ToDoItem>>();
        Controller = new ToDoItemsController(RepositoryMock);
    }
}
