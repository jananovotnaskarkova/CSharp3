namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class PostTests
{
    private readonly string dataPath = "Data Source=../../../IntegrationTests/data/localdb_test.db";
    private readonly ToDoItemCreateRequestDto toDoItem1 = new(Name: "jmeno1", Description: "popis1", IsCompleted: false);
    private readonly ToDoItemCreateRequestDto toDoItem2 = new(Name: "jmeno2", Description: "popis2", IsCompleted: true);
    [Fact]
    public void Create_ReturnsCreatedItems()
    {
        // Act
        var contextTest = new ToDoItemsContext(dataPath);
        var controllerTest = new ToDoItemsController(contextTest);
        var resultCreate1 = controllerTest.Create(toDoItem1); // ActionResult<ToDoItemGetResponseDto>
        var resultCreate2 = controllerTest.Create(toDoItem2); // ActionResult<ToDoItemGetResponseDto>

        var resultRead = controllerTest.Read(); // ActionResult<IEnumerable<ToDoItemGetResponseDto>>
        var valueRead = resultRead.GetValue(); // IEnumerable<ToDoItemGetResponseDto>?

        // Assert
        Assert.NotNull(valueRead); // the returned collection should not be null
        Assert.Equal(2, valueRead.Count()); // we expect exactly 2 items

        Assert.IsType<CreatedAtActionResult>(resultCreate1.Result); // the result should be of type CreatedAtActionResult
        Assert.IsType<CreatedAtActionResult>(resultCreate2.Result); // the result should be of type CreatedAtActionResult

        var valueCreate1 = resultCreate1.GetValue(); // ToDoItemGetResponseDto
        // check its properties
        Assert.Equal("jmeno1", valueCreate1.Name);
        Assert.Equal("popis1", valueCreate1.Description);
        Assert.False(valueCreate1.IsCompleted);

        var valueCreate2 = resultCreate2.GetValue(); // ToDoItemGetResponseDto
        // check its properties
        Assert.Equal("jmeno2", valueCreate2.Name);
        Assert.Equal("popis2", valueCreate2.Description);
        Assert.True(valueCreate2.IsCompleted);

        // Cleanup
        controllerTest.DeleteById(valueCreate1.Id);
        controllerTest.DeleteById(valueCreate2.Id);
    }
}
