namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;

public class PutTests : ControllerTestBase
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
    private readonly TodoItemUpdateRequestDto toDoItem3 = new(Name: "jmeno3", Description: "popis3", IsCompleted: false);

    [Fact]
    public void Update_ReturnsUpdatedItems()
    {
        // Arrange
        context.ToDoItems.Add(toDoItem1);
        context.ToDoItems.Add(toDoItem2);
        context.SaveChanges();

        // Act
        var result = Controller.UpdateById(2, toDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.NotNull(value); // the updated item should not be null

        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        // check its properties
        Assert.Equal(2, value.Id);
        Assert.Equal("jmeno3", value.Name);
        Assert.Equal("popis3", value.Description);
        Assert.False(value.IsCompleted);
    }

    [Fact]
    public void Update_ReturnsNotFound()
    {
        // Arrange
        context.ToDoItems.Add(toDoItem1);
        context.ToDoItems.Add(toDoItem2);
        context.SaveChanges();

        // Act
        var result = Controller.UpdateById(3, toDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.Null(value); // the returned item should be null since the updated item does not exist

        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult
    }
}
