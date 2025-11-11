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
        Name = "jmeno1",
        Description = "popis1",
        IsCompleted = false
    };

    [Fact]
    public void Delete_DeleteOneItemById()
    {
        // Arrange
        RepositoryMock.ReadById(Arg.Any<int>()).Returns(item);

        // Act
        var resultDelete = Controller.DeleteById(2); // IActionResult

        // Assert
        Assert.IsType<NoContentResult>(resultDelete); // the result should be of type NoContentResult
    }

    [Fact]
    public void Delete_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.ReadById(Arg.Any<int>()).ReturnsNull();

        // Act
        var result = Controller.DeleteById(3); // IActionResult

        // Assert
        Assert.IsType<NotFoundResult>(result); // the result should be of type NotFoundResult
    }

    // Delete_ValidItemId_ReturnsNoContent()
    // Delete_InvalidItemId_ReturnsNotFound()
    // Delete_AnyItemIdExceptionOccurredDuringReadById_ReturnsInternalServerError()
    // Delete_AnyItemIdExceptionOccurredDuringDeleteById_ReturnsInternalServerError()

    [Fact]
    public void Delete_ValidItemId_ReturnsNoContent()
    {
        // Arrange
        RepositoryMock.ReadById(Arg.Any<int>()).Returns(item);

        // Act
        var result = Controller.DeleteById(10);

        // Assert
        Assert.IsType<NoContentResult>(result);
        RepositoryMock.Received(1).DeleteById(10);
    }

    [Fact]
    public void Delete_InvalidItemId_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.ReadById(Arg.Any<int>()).ReturnsNull();

        // Act
        var result = Controller.DeleteById(10);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        RepositoryMock.Received(1).DeleteById(10);
    }

    // [Fact]
    // public void Delete_AnyItemIdExceptionOccurredDuringReadById_ReturnsInternalServerError()
    // {
    //     // Arrange
    //     RepositoryMock.ReadById(Arg.Any<int>()).Throws(new Exception());

    //     // Act
    //     var result = Controller.DeleteById(10);

    //     // Assert
    //     Assert.IsType<ObjectResult>(result);
    //     RepositoryMock.Received(1).ReadById(20);
    //     Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    // }

    [Fact]
    public void Delete_AnyItemIdExceptionOccurredDuringReadById_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.ReadById(Arg.Any<int>()).Throws(new Exception());
        var someId = 1;

        // Act
        var result = Controller.DeleteById(someId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        RepositoryMock.Received(1).ReadById(someId);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    }

    // [Fact]
    // public void Delete_AnyItemIdExceptionOccurredDuringDeleteById_ReturnsInternalServerError()
    // {
    //     // Arrange
    //     RepositoryMock.When(r => r.DeleteById(Arg.Any<int>())).Do(r => throw new Exception(ArgumentOutOfRangeException));

    //     // Act
    //     var result = Controller.DeleteById(10);

    //     // Assert
    //     Assert.IsType<ObjectResult>(result);
    //     RepositoryMock.Received(1).ReadById(10);
    //     RepositoryMock.Received(1).DeleteById(34);
    //     Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
    // }
}
