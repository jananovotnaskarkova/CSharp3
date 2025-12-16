namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;

public class GetTests : ControllerTestBase
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
    public async Task Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        await Context.ToDoItems.AddAsync(toDoItem1);
        await Context.ToDoItems.AddAsync(toDoItem2);
        await Context.SaveChangesAsync();

        // Act
        var result = await Controller.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var value = result.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(value); // the returned collection should not be null
        Assert.Equal(2, value.Count()); // we expect exactly 2 items

        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        var firstToDo = value.First(); // get the first item
        // check its properties
        Assert.Equal(1, firstToDo.Id);
        Assert.Equal("jmeno1", firstToDo.Name);
        Assert.Equal("popis1", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);
        Assert.Equal("kategorie1", firstToDo.Category);

        var lastToDo = value.Last(); // get the last item
        // check its properties
        Assert.Equal(2, lastToDo.Id);
        Assert.Equal("jmeno2", lastToDo.Name);
        Assert.Equal("popis2", lastToDo.Description);
        Assert.True(lastToDo.IsCompleted);
        Assert.Equal("kategorie2", lastToDo.Category);
    }

    [Fact]
    public async Task Get_ItemById_ReturnsItemById()
    {
        // Arrange
        await Context.ToDoItems.AddAsync(toDoItem1);
        await Context.ToDoItems.AddAsync(toDoItem2);
        await Context.SaveChangesAsync();

        // Act
        var result = await Controller.ReadById(1); // ActionResult<ToDoItemGetResponseDto>
        Assert.NotNull(result);
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.NotNull(value); // the returned item should not be null

        Assert.IsType<OkObjectResult>(result.Result); // the result should be of type OkObjectResult

        // check its properties
        Assert.Equal(1, value.Id);
        Assert.Equal("jmeno1", value.Name);
        Assert.Equal("popis1", value.Description);
        Assert.False(value.IsCompleted);
        Assert.Equal("kategorie1", value.Category);
    }

    [Fact]
    public async Task Get_ItemById_ReturnsNotFound()
    {
        // Arrange
        await Context.ToDoItems.AddAsync(toDoItem1);
        await Context.ToDoItems.AddAsync(toDoItem2);
        await Context.SaveChangesAsync();


        // Act
        var result = await Controller.ReadById(3); // ActionResult<ToDoItemGetResponseDto>
        Assert.NotNull(result);
        var value = result.GetValue(); // ToDoItemGetResponseDto?

        // Assert
        Assert.Null(value); // the returned item should be null since the item does not exist

        Assert.IsType<NotFoundResult>(result.Result); // the result should be of type NotFoundResult
    }
}
