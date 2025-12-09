namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;

public class DeleteTests : ControllerTestBase
{
    private readonly ToDoItem toDoItem1 = new()
    {
        ToDoItemId = 1,
        Name = "jmeno1",
        Description = "popis1",
        IsCompleted = false,
        Category = "kategorie1"
    };
    private readonly ToDoItem toDoItem2 = new()
    {
        ToDoItemId = 2,
        Name = "jmeno2",
        Description = "popis2",
        IsCompleted = true,
        Category = "kategorie2"
    };

    [Fact]
    public async Task Delete_DeleteOneItemById()
    {
        // Arrange
        await Context.ToDoItems.AddAsync(toDoItem1);
        await Context.ToDoItems.AddAsync(toDoItem2);
        await Context.SaveChangesAsync();

        // Act
        var resultDelete = await Controller.DeleteById(1); // IActionResult
        var resultRead = await Controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
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
        Assert.Equal("kategorie2", singleItem.Category);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound()
    {
        // Arrange
        await Context.ToDoItems.AddAsync(toDoItem1);
        await Context.ToDoItems.AddAsync(toDoItem2);
        await Context.SaveChangesAsync();

        // Act
        var resultDelete = await Controller.DeleteById(3); // IActionResult
        var resultRead = await Controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Equal(2, valueRead.Count()); // both items should remain

        Assert.IsType<NotFoundResult>(resultDelete); // the result should be of type NotFoundResult
    }
}
