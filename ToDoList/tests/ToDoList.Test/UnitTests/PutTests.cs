namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : ControllerUnitTestBase
{
    private readonly TodoItemUpdateRequestDto toDoItem3 = new(Name: "jmeno3", Description: "popis3", IsCompleted: false);
    private static ToDoItem FakeUpdateList() =>
        new()
        {
            ToDoItemId = 1,
            Name = "jmeno3",
            Description = "popis3",
            IsCompleted = false
        };

    [Fact]
    public void Update_ReturnsUpdatedItems()
    {
        // Arrange
        RepositoryMock.UpdateById(1, toDoItem3).Returns(FakeUpdateList());

        // Act
        var result = Controller.UpdateById(1, toDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.NotNull(value); // the updated item should not be null
        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        // check its properties
        Assert.Equal(1, value.Id);
        Assert.Equal("jmeno3", value.Name);
        Assert.Equal("popis3", value.Description);
        Assert.False(value.IsCompleted);
    }

    [Fact]
    public void Update_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.UpdateById(3, toDoItem3).ReturnsNull();

        // Act
        var result = Controller.UpdateById(3, toDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.Null(value); // the returned item should be null since the updated item does not exist
        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult
    }
}
