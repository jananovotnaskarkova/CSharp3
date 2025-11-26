namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : ControllerUnitTestBase
{
    private readonly TodoItemUpdateRequestDto toDoItem = new(Name: "nove jmeno", Description: "novy popis", IsCompleted: true, Category: "nova kategorie");
    private readonly ToDoItem updatedItem = new()
    {
        ToDoItemId = 1,
        Name = "nove jmeno",
        Description = "novy popis",
        IsCompleted = true
    };
    private readonly int someId = 1;

    [Fact]
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsOk()
    {
        // Arrange
        RepositoryMock.UpdateById(Arg.Any<int>(), Arg.Any<TodoItemUpdateRequestDto>()).Returns(updatedItem);

        // Act
        var result = await Controller.UpdateById(someId, toDoItem);
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.IsType<OkObjectResult>(result.Result);
        await RepositoryMock.Received(1).UpdateById(someId, toDoItem);

        Assert.Equal(someId, value.Id);
        Assert.Equal(updatedItem.Name, value.Name);
        Assert.Equal(updatedItem.Description, value.Description);
        Assert.True(value.IsCompleted);
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.UpdateById(Arg.Any<int>(), Arg.Any<TodoItemUpdateRequestDto>()).ReturnsNull();

        // Act
        var result = await Controller.UpdateById(someId, toDoItem);
        var value = result.GetValue();

        // Assert
        Assert.Null(value);
        Assert.IsType<NotFoundResult>(result.Result);
        await RepositoryMock.Received(1).UpdateById(someId, toDoItem);
    }

    [Fact]
    public async Task Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.UpdateById(Arg.Any<int>(), Arg.Any<TodoItemUpdateRequestDto>()).Throws(new InvalidOperationException());

        // Act
        var result = await Controller.UpdateById(someId, toDoItem);

        // Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.True(objectResult.StatusCode.HasValue);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode.Value);
        await RepositoryMock.Received(1).UpdateById(someId, toDoItem);
    }
}
