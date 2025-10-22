namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using ToDoList.Persistence;

public class PutTests
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
    private static readonly TodoItemUpdateRequestDto ToDoItem3 = new(Name: "jmeno3", Description: "popis3", IsCompleted: false);

    [Fact]
    public void Update_ReturnsUpdatedItems()
    {
        // Arrange
        ContextTest.Add(ToDoItem1);
        ContextTest.Add(ToDoItem2);
        ContextTest.SaveChanges();

        // Act
        var result = controllerTest.UpdateById(2, ToDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.NotNull(value); // the updated item should not be null

        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        // check its properties
        Assert.Equal(2, value.Id);
        Assert.Equal("jmeno3", value.Name);
        Assert.Equal("popis3", value.Description);
        Assert.False(value.IsCompleted);

        // Cleanup
        ContextTest.Remove(ToDoItem1);
        ContextTest.Remove(ToDoItem2);
        ContextTest.SaveChanges();
    }

    [Fact]
    public void Update_ReturnsNotFound()
    {
        // Arrange
        ContextTest.Add(ToDoItem1);
        ContextTest.Add(ToDoItem2);
        ContextTest.SaveChanges();

        // Act
        var result = controllerTest.UpdateById(3, ToDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?
        // Assert
        Assert.Null(value); // the returned item should be null since the updated item does not exist

        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult

        // Cleanup
        ContextTest.Remove(ToDoItem1);
        ContextTest.Remove(ToDoItem2);
        ContextTest.SaveChanges();
    }
}
