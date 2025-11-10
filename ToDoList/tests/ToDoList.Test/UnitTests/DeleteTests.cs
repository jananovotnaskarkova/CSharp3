namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;

public class DeleteTests : ControllerUnitTestBase
{
    [Fact]
    public void Delete_DeleteOneItemById()
    {
        // Arrange
        RepositoryMock.DeleteById(2).Returns(true);

        // Act
        var resultDelete = Controller.DeleteById(2); // IActionResult

        // Assert
        Assert.IsType<NoContentResult>(resultDelete); // the result should be of type NoContentResult
    }

    [Fact]
    public void Delete_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.DeleteById(3).Returns(false);

        // Act
        var result = Controller.DeleteById(3); // IActionResult

        // Assert
        Assert.IsType<NotFoundResult>(result); // the result should be of type NotFoundResult
    }
}
