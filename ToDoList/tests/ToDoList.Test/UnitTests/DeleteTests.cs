namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

public class DeleteTests : ControllerUnitTestBase
{
    private readonly int someId = 1;

    [Fact]
    public async Task Delete_ValidItemId_ReturnsNoContent()
    {
        // Arrange
        RepositoryMock.DeleteByIdAsync(Arg.Any<int>()).Returns(true);

        // Act
        var result = await Controller.DeleteById(someId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await RepositoryMock.Received(1).DeleteByIdAsync(someId);
    }

    [Fact]
    public async Task Delete_InvalidItemId_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.DeleteByIdAsync(Arg.Any<int>()).Returns(false);

        // Act
        var result = await Controller.DeleteById(someId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await RepositoryMock.Received(1).DeleteByIdAsync(someId);
    }

    [Fact]
    public async Task Delete_AnyItemIdExceptionOccurredDuringDeleteById_ReturnsInternalServerError()
    {
        // Arrange
        RepositoryMock.When(r => r.DeleteByIdAsync(Arg.Any<int>())).Do(r => throw new InvalidOperationException());

        // Act
        var result = await Controller.DeleteById(someId);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.True(objectResult.StatusCode.HasValue);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode.Value);
        await RepositoryMock.Received(1).DeleteByIdAsync(someId);
    }
}
