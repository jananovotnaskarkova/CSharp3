namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class GetUnitTests : ControllerUnitTestBase
{
    private static List<ToDoItem> fakeList =
    [
        new()
        {
            ToDoItemId = 1,
            Name = "jmeno1",
            Description = "popis1",
            IsCompleted = false
        },
        new()
        {
            ToDoItemId = 2,
            Name = "jmeno2",
            Description = "popis2",
            IsCompleted = true
        },
    ];
    private static List<ToDoItem> FakeGetData() => fakeList;
    private static ToDoItem FakeGetData(int i) => fakeList.Single(item => item.ToDoItemId == i);

    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        RepositoryMock.Read().Returns(FakeGetData());

        // Act
        var result = Controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
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
    }

    [Fact]
    public void Get_ItemById_ReturnsItemById()
    {
        // Arrange
        RepositoryMock.ReadById(1).Returns(FakeGetData(1));

        // Act
        var result = Controller.ReadById(1); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.NotNull(value); // the returned item should not be null
        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        // check its properties
        Assert.Equal(1, value.Id);
        Assert.Equal("jmeno1", value.Name);
        Assert.Equal("popis1", value.Description);
        Assert.False(value.IsCompleted);
    }

    [Fact]
    public void Get_ItemById_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.ReadById(3).ReturnsNull();

        // Act
        var result = Controller.ReadById(3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.Null(value); // the returned item should be null since the item does not exist
        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult
    }

    [Fact]
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        RepositoryMock.Read().Returns(FakeGetData());

        // Act
        var result = Controller.Read();

        // Assert
        Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
        RepositoryMock.Received(1).Read();
    }

    [Fact]
    public void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public void Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        // Arrange

        // Act

        // Assert
    }
}
