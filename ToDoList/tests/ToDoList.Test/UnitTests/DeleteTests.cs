namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;

public class DeleteTests : ControllerUnitTestBase
{
    private static List<ToDoItem> fakeDeleteList =
    [
        new()
        {
            ToDoItemId = 1,
            Name = "jmeno1",
            Description = "popis1",
            IsCompleted = false
        },
    ];
    private static List<ToDoItem> FakeDeleteData() => fakeDeleteList;

    [Fact]
    public void Delete_DeleteOneItemById()
    {
        // Arrange
        RepositoryMock.DeleteById(2).Returns(true);
        RepositoryMock.Read().Returns(FakeDeleteData());

        // Act
        var resultDelete = Controller.DeleteById(2); // IActionResult
        var resultRead = Controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Single(valueRead); // we expect exactly 1 item remaining

        Assert.IsType<NoContentResult>(resultDelete); // the result should be of type NoContentResult

        // Assert properties of the remaining item
        var singleItem = valueRead.Single();
        Assert.Equal(1, singleItem.Id);
        Assert.Equal("jmeno1", singleItem.Name);
        Assert.Equal("popis1", singleItem.Description);
        Assert.False(singleItem.IsCompleted);
    }

    [Fact]
    public void Delete_ReturnsNotFound()
    {
        // Arrange
        RepositoryMock.DeleteById(3).Returns(false);

        // Act
        var resultDelete = Controller.DeleteById(3); // IActionResult

        // Assert
        Assert.IsType<NotFoundResult>(resultDelete); // the result should be of type NotFoundResult
    }
}
