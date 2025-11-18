namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

public class DeleteTests : ControllerUnitTestBase
{
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
        RepositoryMock.When(r => r.DeleteById(Arg.Any<int>())).Do(r => throw new InvalidOperationException());

        // Act
        var result = Controller.DeleteById(someId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
        RepositoryMock.Received(1).DeleteById(someId);
    }
}
