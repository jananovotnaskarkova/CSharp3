namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.Models;

public class GetUnitTests : ControllerUnitTestBase
{
    private readonly List<ToDoItem> list =
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
    private readonly int someId = 1;

    [Fact]
    public async Task Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        RepositoryMock.ReadAsync().Returns(list);

        // Act
        var result = await Controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.Equal(2, value.Count());
        Assert.IsType<OkObjectResult>(result.Result);
        await RepositoryMock.Received(1).ReadAsync();
    }

    [Fact]
    public async Task Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.ReadAsync().Returns([]);

        // Act
        var result = await Controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        await RepositoryMock.Received(1).ReadAsync();
    }

    [Fact]
    public async Task Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.ReadAsync().Throws(new InvalidOperationException());

        // Act
        var result = await Controller.Read();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.True(objectResult.StatusCode.HasValue);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode.Value);
        await RepositoryMock.Received(1).ReadAsync();
    }

    [Fact]
    public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        RepositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(list[0]);

        // Act
        var result = await Controller.ReadById(someId);
        Assert.NotNull(result);
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.IsType<OkObjectResult>(result.Result);
        await RepositoryMock.Received(1).ReadByIdAsync(someId);

        Assert.Equal(someId, value.Id);
        Assert.Equal("jmeno1", value.Name);
        Assert.Equal("popis1", value.Description);
        Assert.False(value.IsCompleted);
    }

    [Fact]
    public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.ReadByIdAsync(Arg.Any<int>()).ReturnsNull();

        // Act
        var result = await Controller.ReadById(someId);
        Assert.NotNull(result);
        var value = result.GetValue();

        // Assert
        Assert.Null(value);
        Assert.IsType<NotFoundResult>(result.Result);
        await RepositoryMock.Received(1).ReadByIdAsync(someId);
    }

    [Fact]
    public async Task Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.ReadByIdAsync(Arg.Any<int>()).Throws(new InvalidOperationException());

        // Act
        var result = await Controller.ReadById(someId);

        // Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.True(objectResult.StatusCode.HasValue);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode.Value);
        await RepositoryMock.Received(1).ReadByIdAsync(someId);
    }
}
