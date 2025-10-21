namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly List<ToDoItem> items = [];
    private readonly ToDoItemsContext context;

    public ToDoItemsController(ToDoItemsContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //pouzijeme DTO = Data Transfer Object
    {
        //create domain object from request
        var item = request.ToDomain();

        //try to create an item
        try
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return CreatedAtAction(actionName: nameof(ReadById), // Which method to use to get the resource
                               routeValues: new { toDoItemId = item.ToDoItemId }, // Parameters needed to call that method
                               value: ToDoItemGetResponseDto.FromDomain(item) // The created item to return
                               ); //201
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        List<ToDoItem> itemsToGet;

        try
        {
            itemsToGet = context.ToDoItems.ToList();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
        return (itemsToGet is null)
            ? NotFound() //404
            : Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain)); //200 with data
    }

    [HttpGet("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
    {
        ToDoItem itemToGet;

        //try to read an item
        try
        {
            itemToGet = context.ToDoItems.Find(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
        return (itemToGet is null)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet)); //200 with data
    }

    [HttpPut("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> UpdateById(int toDoItemId, [FromBody] TodoItemUpdateRequestDto request)
    {
        //create domain object from request
        var itemUpdated = request.ToDomain();
        int number;

        //try to update an item
        try
        {
            number = context.ToDoItems
            .Where(i => i.ToDoItemId == toDoItemId)
            .ExecuteUpdate(x => x
                .SetProperty(item => item.Name, itemUpdated.Name)
                .SetProperty(item => item.Description, itemUpdated.Description)
                .SetProperty(item => item.IsCompleted, itemUpdated.IsCompleted)
            );
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
        return (number is 0)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(context.ToDoItems.Find(toDoItemId))); //200 with data

    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        //try to delete an item
        try
        {
            context.ToDoItems.Remove(context.ToDoItems.Find(toDoItemId)!);
            context.SaveChanges();
            return NoContent(); //204
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    public void AddItemToStorage(ToDoItem item) => items.Add(item);
}


