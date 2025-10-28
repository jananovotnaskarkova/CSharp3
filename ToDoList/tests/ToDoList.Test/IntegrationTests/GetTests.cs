namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class GetTests
{
    private readonly string dataPath = "Data Source=../../../IntegrationTests/data/localdb_test.db";
    private readonly ToDoItem toDoItem1 = new()
    {
        ToDoItemId = 1,
        Name = "jmeno1",
        Description = "popis1",
        IsCompleted = false
    };
    private readonly ToDoItem toDoItem2 = new()
    {
        ToDoItemId = 2,
        Name = "jmeno2",
        Description = "popis2",
        IsCompleted = true
    };

    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var contextTest = new ToDoItemsContext(dataPath);
        var controllerTest = new ToDoItemsController(contextTest);
        contextTest.ToDoItems.Add(toDoItem1);
        contextTest.ToDoItems.Add(toDoItem2);
        contextTest.SaveChanges();

        // Act
        var result = controllerTest.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var value = result.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(value); // the returned collection should not be null
        Assert.Equal(2, value.Count()); // we expect exactly 2 items

        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        var firstToDo = value.First(); // get the first item
        // check its properties
        Assert.Equal(1, firstToDo.Id);
        Assert.Equal("jmeno1", firstToDo.Name);
        Assert.Equal("popis1", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);

        var lastToDo = value.Last(); // get the last item
        // check its properties
        Assert.Equal(2, lastToDo.Id);
        Assert.Equal("jmeno2", lastToDo.Name);
        Assert.Equal("popis2", lastToDo.Description);
        Assert.True(lastToDo.IsCompleted);

        // Cleanup
        contextTest.ToDoItems.Remove(toDoItem1);
        contextTest.ToDoItems.Remove(toDoItem2);
        contextTest.SaveChanges();
    }

    [Fact]
    public void Get_ItemById_ReturnsItemById()
    {
        // Arrange
        var contextTest = new ToDoItemsContext(dataPath);
        var controllerTest = new ToDoItemsController(contextTest);
        contextTest.ToDoItems.Add(toDoItem1);
        contextTest.ToDoItems.Add(toDoItem2);
        contextTest.SaveChanges();

        // Act
        var result = controllerTest.ReadById(1); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.NotNull(value); // the returned item should not be null

        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        // check its properties
        Assert.Equal(1, value.Id);
        Assert.Equal("jmeno1", value.Name);
        Assert.Equal("popis1", value.Description);
        Assert.False(value.IsCompleted);

        // Cleanup
        contextTest.ToDoItems.Remove(toDoItem1);
        contextTest.ToDoItems.Remove(toDoItem2);
        contextTest.SaveChanges();
    }

    [Fact]
    public void Get_ItemById_ReturnsNotFound()
    {
        // Arrange
        var contextTest = new ToDoItemsContext(dataPath);
        var controllerTest = new ToDoItemsController(contextTest);
        contextTest.ToDoItems.Add(toDoItem1);
        contextTest.ToDoItems.Add(toDoItem2);
        contextTest.SaveChanges();


        // Act
        var result = controllerTest.ReadById(3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.Null(value); // the returned item should be null since the item does not exist

        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult

        // Cleanup
        contextTest.ToDoItems.Remove(toDoItem1);
        contextTest.ToDoItems.Remove(toDoItem2);
        contextTest.SaveChanges();
    }
}
