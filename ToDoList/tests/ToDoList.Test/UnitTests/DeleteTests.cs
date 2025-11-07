namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class DeleteTests : ControllerUnitTestBase
{
    private readonly ToDoItemCreateRequestDto toDoItem1 = new(Name: "jmeno1", Description: "popis1", IsCompleted: false);
    private readonly ToDoItemCreateRequestDto toDoItem2 = new(Name: "jmeno2", Description: "popis2", IsCompleted: true);

    [Fact]
    public void Delete_DeleteOneItemById()
    {
        // Arrange
        Controller.Create(toDoItem1);
        Controller.Create(toDoItem2);

        // Act
        var resultDelete = Controller.DeleteById(1); // IActionResult
        var resultRead = Controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Single(valueRead); // we expect exactly 1 item remaining

        Assert.IsType<NoContentResult>(resultDelete); // the result should be of type NoContentResult

        // Assert properties of the remaining item
        var singleItem = valueRead.Single();
        Assert.Equal(0, singleItem.Id);
        Assert.Equal("jmeno1", singleItem.Name);
        Assert.Equal("popis1", singleItem.Description);
        Assert.False(singleItem.IsCompleted);
    }

    [Fact]
    public void Delete_ReturnsNotFound()
    {
        // Arrange
        Controller.Create(toDoItem1);
        Controller.Create(toDoItem2);

        // Act
        var resultDelete = Controller.DeleteById(3); // IActionResult

        // Assert
        Assert.IsType<NotFoundResult>(resultDelete); // the result should be of type NotFoundResult
    }
}
