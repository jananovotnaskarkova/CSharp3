namespace ToDoList.Test.UnitTests;

using ToDoList.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

public class PostTests : ControllerUnitTestBase
{
    private readonly ToDoItemCreateRequestDto toDoItem = new(Name: "jmeno", Description: "popis", IsCompleted: true);

    [Fact]
    public void Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        RepositoryMock.Create(toDoItem.ToDomain());

        // Act
        var result = Controller.Create(toDoItem);
        var value = result.GetValue();

        // Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);

        Assert.NotNull(value);
        Assert.Equal("jmeno", value.Name);
        Assert.Equal("popis", value.Description);
        Assert.True(value.IsCompleted);
    }

    // [Fact]
    // public void Post_CreateUnhandledException_ReturnsInternalServerError()
    // {
    //     // Arrange
    //     RepositoryMock.When(r => r.Create(toDoItem1)).Do(r => throw new Exception());

    //     // Act
    //     var result = Controller.Create(toDoItem);

    //     // Assert
    //     Assert.IsType<ObjectResult>(result);
    //     Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);
    // }
}
