namespace ToDoList.Test.UnitTests;

using ToDoList.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSubstitute;

public class PostTests : ControllerUnitTestBase
{
    private readonly ToDoItemCreateRequestDto toDoItem = new(Name: "jmeno", Description: "popis", IsCompleted: true, Category: "kategorie");

    [Fact]
    public async Task Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        // Act
        var result = await Controller.Create(toDoItem);
        var value = result.GetValue();

        // Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);
        await RepositoryMock.Received(1).CreateAsync(toDoItem);

        Assert.NotNull(value);
        Assert.Equal("jmeno", value.Name);
        Assert.Equal("popis", value.Description);
        Assert.True(value.IsCompleted);
    }

    [Fact]
    public async Task Post_CreateUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.When(r => r.CreateAsync(toDoItem)).Do(r => throw new InvalidOperationException());

        // Act
        var result = await Controller.Create(toDoItem);

        // Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.True(objectResult.StatusCode.HasValue);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode.Value);
        await RepositoryMock.Received(1).CreateAsync(toDoItem);
    }
}
