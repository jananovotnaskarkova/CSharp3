namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class DeleteTests
{
    private static readonly ToDoItemsContext ContextTest = new("Data Source=../../../IntegrationTests/data/localdb_test.db");
    private readonly ToDoItemsController controllerTest = new(ContextTest);
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
        ContextTest.Add(ToDoItem1);
        ContextTest.Add(ToDoItem2);
        ContextTest.SaveChanges();

        // Act
        var resultDelete = controllerTest.DeleteById(1); // IActionResult
        var resultRead = controllerTest.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
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

        // Cleanup
        ContextTest.Remove(ToDoItem2);
        ContextTest.SaveChanges();
    }

    [Fact]
    public void Delete_ReturnsNotFound()
    {
        // Arrange
        ContextTest.Add(ToDoItem1);
        ContextTest.Add(ToDoItem2);
        ContextTest.SaveChanges();

        // Act
        var resultDelete = controllerTest.DeleteById(3); // IActionResult
        var resultRead = controllerTest.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Equal(2, valueRead.Count()); // both items should remain

        Assert.IsType<NotFoundResult>(resultDelete); // the result should be of type NotFoundResult

        // Cleanup
        ContextTest.Remove(ToDoItem1);
        ContextTest.Remove(ToDoItem2);
        ContextTest.SaveChanges();
    }
}
