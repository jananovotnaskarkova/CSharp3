namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class DeleteTests : ControllerTestBase
{
    private readonly ToDoItem toDoItem1 = new()
    {
        ToDoItemId = 1,
        Name = "jmeno1",
        Description = "popis1",
        IsCompleted = false
    };
    private readonly ToDoItem toDoItem2 = new()
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
        var connectionString = "Data Source=../../../IntegrationTests/data/localdb_test.db";
        using var context = new ToDoItemsContext(connectionString);
        var controller = new ToDoItemsController(context: context, repository: null);
        context.ToDoItems.Add(toDoItem1);
        context.ToDoItems.Add(toDoItem2);
        context.SaveChanges();

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
        Assert.Equal(2, singleItem.Id);
        Assert.Equal("jmeno2", singleItem.Name);
        Assert.Equal("popis2", singleItem.Description);
        Assert.True(singleItem.IsCompleted);
    }

    [Fact]
    public void Delete_ReturnsNotFound()
    {
        // Arrange
        Context.ToDoItems.Add(toDoItem1);
        Context.ToDoItems.Add(toDoItem2);
        Context.SaveChanges();

        // Act
        var resultDelete = Controller.DeleteById(3); // IActionResult
        var resultRead = Controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Equal(2, valueRead.Count()); // both items should remain

        Assert.IsType<NotFoundResult>(resultDelete); // the result should be of type NotFoundResult
    }
}
