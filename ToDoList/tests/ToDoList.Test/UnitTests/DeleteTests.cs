namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.Models;

public class DeleteTests : ControllerUnitTestBase
{
    private readonly ToDoItem item = new()
    {
        ToDoItemId = 1,
        Name = "jmeno",
        Description = "popis",
        IsCompleted = false
    };
    private readonly int someId = 1;

    [Fact]
    public void Delete_ValidItemId_ReturnsNoContent()
    {
        // Arrange
        RepositoryMock.DeleteById(Arg.Any<int>()).Returns(true);

        // Act
        var result = Controller.DeleteById(someId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        RepositoryMock.Received(1).DeleteById(someId);
    }

    [Fact]
    public void Delete_InvalidItemId_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.DeleteById(Arg.Any<int>()).Returns(false);

        // Act
        var result = Controller.DeleteById(someId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        RepositoryMock.Received(1).DeleteById(someId);
    }

    [Fact]
    public void Delete_AnyItemIdExceptionOccurredDuringDeleteById_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.When(r => r.DeleteById(Arg.Any<int>())).Do(r => throw new Exception());

        // Act
        var result = Controller.DeleteById(someId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        RepositoryMock.Received(1).DeleteById(someId);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }
}
