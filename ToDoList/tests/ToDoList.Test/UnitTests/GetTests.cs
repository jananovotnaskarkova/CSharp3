namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class GetTests : ControllerUnitTestBase
{
    private readonly ToDoItemCreateRequestDto toDoItem1 = new(Name: "jmeno1", Description: "popis1", IsCompleted: false);
    private readonly ToDoItemCreateRequestDto toDoItem2 = new(Name: "jmeno2", Description: "popis2", IsCompleted: true);

    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        Controller.Create(toDoItem1);
        Controller.Create(toDoItem2);

        // Act
        var result = Controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var value = result.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(value); // the returned collection should not be null
        Assert.Equal(2, value.Count()); // we expect exactly 2 items

        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        var firstToDo = value.First(); // get the first item
        // check its properties
        Assert.Equal(0, firstToDo.Id);
        Assert.Equal("jmeno1", firstToDo.Name);
        Assert.Equal("popis1", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);

        var lastToDo = value.Last(); // get the last item
        // check its properties
        Assert.Equal(1, lastToDo.Id);
        Assert.Equal("jmeno2", lastToDo.Name);
        Assert.Equal("popis2", lastToDo.Description);
        Assert.True(lastToDo.IsCompleted);
    }

    [Fact]
    public void Get_ItemById_ReturnsItemById()
    {
        // Arrange
        Controller.Create(toDoItem1);
        Controller.Create(toDoItem2);

        // Act
        var result = Controller.ReadById(1); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.NotNull(value); // the returned item should not be null

        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        // check its properties
        Assert.Equal(1, value.Id);
        Assert.Equal("jmeno2", value.Name);
        Assert.Equal("popis2", value.Description);
        Assert.True(value.IsCompleted);
    }

    [Fact]
    public void Get_ItemById_ReturnsNotFound()
    {
        // Arrange
        Controller.Create(toDoItem1);
        Controller.Create(toDoItem2);

        // Act
        var result = Controller.ReadById(3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.Null(value); // the returned item should be null since the item does not exist

        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult
    }
}
