namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly IRepository<ToDoItem> repository;
    public ToDoItemsController(IRepository<ToDoItem> repository)
    {
        this.repository = repository;
    }

    [HttpPost]
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request) //pouzijeme DTO = Data Transfer Object
    {
        //create domain object from request
        var item = request.ToDomain();

        //try to create an item
        try
        {
            repository.Create(item);
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

        //try to read all items
        try
        {
            itemsToGet = repository.Read();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
        //respond to client
        return Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain)); //200 with data
    }

    [HttpGet("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
    {
        ToDoItem itemToGet;

        //try to read an item
        try
        {
            itemToGet = repository.ReadById(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
        //respond to client
        return (itemToGet is null)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet)); //200 with data
    }

    [HttpPut("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> UpdateById(int toDoItemId, [FromBody] TodoItemUpdateRequestDto request)
    {
        bool is_updated;

        //try to update an item
        try
        {
            is_updated = repository.UpdateById(toDoItemId, request);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
        //respond to client
        if (is_updated)
        {
            return Ok(ToDoItemGetResponseDto.FromDomain(repository.ReadById(toDoItemId))); //200 with data
        }
        else
        {
            return NotFound(); //404
        }
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        bool is_deleted;

        //try to delete an item
        try
        {
            is_deleted = repository.DeletById(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
        //respond to client
        return is_deleted
            ? NoContent() //204
            : NotFound(); //404
    }
}
