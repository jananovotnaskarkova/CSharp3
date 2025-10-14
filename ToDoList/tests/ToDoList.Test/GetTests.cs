namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class GetTests
{
    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "jmeno1",
            Description = "popis1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "jmeno2",
            Description = "popis2",
            IsCompleted = true
        };
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(toDoItem1);
        controller.AddItemToStorage(toDoItem2);

        // Act
        var result = controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        _ = Assert.IsType<OkObjectResult>(result.Result);

        var firstToDo = value.First();
        Assert.Equal(1, firstToDo.Id);
        Assert.Equal("jmeno1", firstToDo.Name);
        Assert.Equal("popis1", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);

        var lastToDo = value.Last();
        Assert.Equal(2, lastToDo.Id);
        Assert.Equal("jmeno2", lastToDo.Name);
        Assert.Equal("popis2", lastToDo.Description);
        Assert.True(lastToDo.IsCompleted);
    }

    [Fact]
    public void Get_ItemById_ReturnsItemById()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "jmeno1",
            Description = "popis1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "jmeno2",
            Description = "popis2",
            IsCompleted = true
        };
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(toDoItem1);
        controller.AddItemToStorage(toDoItem2);

        // Act
        var result = controller.ReadById(1);
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        _ = Assert.IsType<OkObjectResult>(result.Result);

        Assert.Equal(1, value.Id);
        Assert.Equal("jmeno1", value.Name);
        Assert.Equal("popis1", value.Description);
        Assert.False(value.IsCompleted);
    }

    [Fact]
    public void Get_ItemById_ReturnsNotFound()
    {
        // Arrange
        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "jmeno1",
            Description = "popis1",
            IsCompleted = false
        };
        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "jmeno2",
            Description = "popis2",
            IsCompleted = true
        };
        var controller = new ToDoItemsController();
        controller.AddItemToStorage(toDoItem1);
        controller.AddItemToStorage(toDoItem2);

        // Act
        var result = controller.ReadById(3);
        var value = result.GetValue();

        // Assert
        Assert.Null(value);

        _ = Assert.IsType<NotFoundResult>(result.Result);
    }
}
