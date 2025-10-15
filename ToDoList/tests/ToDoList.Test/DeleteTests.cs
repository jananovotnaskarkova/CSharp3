namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class DeleteTests
{
    private static readonly ToDoItem ToDoItem1 = new()
    {
        ToDoItemId = 1,
        Name = "jmeno1",
        Description = "popis1",
        IsCompleted = false
    };
    private static readonly ToDoItem ToDoItem2 = new()
    {
        ToDoItemId = 2,
        Name = "jmeno2",
        Description = "popis2",
        IsCompleted = true
    };

    [Fact]
    public void Delete_DeleteOneItemById()
    {
        // Arrange
        var controller = new ToDoItemsController();

        controller.AddItemToStorage(ToDoItem1);
        controller.AddItemToStorage(ToDoItem2);

        // Act
        var resultDeletion = controller.DeleteById(1); // ActionResult
        var resultRead = controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Single(valueRead); // we expect exactly 1 item remaining

        Assert.IsType<NoContentResult>(resultDeletion); // the result should be of type NoContentResult

        // Assert properties of the remaining item
        var singleItem = valueRead.Single();
        Assert.Equal(2, singleItem.Id);
        Assert.Equal("jmeno2", singleItem.Name);
        Assert.Equal("popis2", singleItem.Description);
        Assert.True(singleItem.IsCompleted);
    }

    [Fact]
    public void Delete_ReturnsNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(ToDoItem1);
        controller.AddItemToStorage(ToDoItem2);

        // Act
        var resultDeletion = controller.DeleteById(3); // ActionResult
        var resultRead = controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Equal(2, valueRead.Count()); // both items should remain

        Assert.IsType<NotFoundResult>(resultDeletion); // the result should be of type NotFoundResult
    }
}
