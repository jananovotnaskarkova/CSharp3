namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;

public class PutTests
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
    private static readonly TodoItemUpdateRequestDto ToDoItem3 = new(Name: "jmeno3", Description: "popis3", IsCompleted: false);

    [Fact]
    public void Update_ReturnUpdatedItems()
    {
        // Arrange
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(ToDoItem1);
        controller.AddItemToStorage(ToDoItem2);

        // Act
        var result = controller.UpdateById(2, ToDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

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
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(ToDoItem1);
        controller.AddItemToStorage(ToDoItem2);

        // Act
        var result = controller.UpdateById(3, ToDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.Null(value); // the returned item should be null since the updated item does not exist

        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult
    }
}
