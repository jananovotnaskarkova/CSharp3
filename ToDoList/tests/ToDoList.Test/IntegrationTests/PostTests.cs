namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;

public class PostTests
{
    private static readonly ToDoItemCreateRequestDto ToDoItem1 = new(Name: "jmeno1", Description: "popis1", IsCompleted: false);
    private static readonly ToDoItemCreateRequestDto ToDoItem2 = new(Name: "jmeno2", Description: "popis2", IsCompleted: true);

    [Fact]
    public void Create_ReturnsCreatedItems()
    {
        // Arrange
        var controller = new ToDoItemsController();

        // Act
        var resultCreate1 = controller.Create(ToDoItem1); // IActionResult
        var resultCreate2 = controller.Create(ToDoItem2); // IActionResult

        var resultRead = controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Equal(2, valueRead.Count()); // we expect exactly 2 items

        Assert.IsType<CreatedAtActionResult>(resultCreate1); // the result should be of type CreatedAtActionResult
        Assert.IsType<CreatedAtActionResult>(resultCreate2); // the result should be of type CreatedAtActionResult

        var toDo1 = valueRead.First();
        // check its properties
        Assert.Equal(1, toDo1.Id);
        Assert.Equal("jmeno1", toDo1.Name);
        Assert.Equal("popis1", toDo1.Description);
        Assert.False(toDo1.IsCompleted);

        var toDo2 = valueRead.Last();
        // check its properties
        Assert.Equal(2, toDo2.Id);
        Assert.Equal("jmeno2", toDo2.Name);
        Assert.Equal("popis2", toDo2.Description);
        Assert.True(toDo2.IsCompleted);
    }
}