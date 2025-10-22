namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;
using ToDoList.Persistence;

public class PutTests
{
    private readonly string dataPath = "Data Source=../../../IntegrationTests/data/localdb_test.db";
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
        var contextTest = new ToDoItemsContext(dataPath);
        var controllerTest = new ToDoItemsController(contextTest);
        contextTest.Add(toDoItem1);
        contextTest.Add(toDoItem2);
        contextTest.SaveChanges();

        // Act
        var result = controllerTest.UpdateById(2, toDoItem3); // ActionResult<ToDoItemGetResponseDto>
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
        contextTest.Remove(toDoItem1);
        contextTest.Remove(toDoItem2);
        contextTest.SaveChanges();
    }

    [Fact]
    public void Update_ReturnsNotFound()
    {
        // Arrange
        var contextTest = new ToDoItemsContext(dataPath);
        var controllerTest = new ToDoItemsController(contextTest);
        contextTest.Add(toDoItem1);
        contextTest.Add(toDoItem2);
        contextTest.SaveChanges();

        // Act
        var result = controllerTest.UpdateById(3, toDoItem3); // ActionResult<ToDoItemGetResponseDto>
        var value = result.GetValue(); // ToDoItemGetResponseDto?
        // Assert
        Assert.Null(value); // the returned item should be null since the updated item does not exist

        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult

        // Cleanup
        contextTest.Remove(toDoItem1);
        contextTest.Remove(toDoItem2);
        contextTest.SaveChanges();
    }
}
