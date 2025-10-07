namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private static List<ToDoItem> items = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //pouzijeme DTO = Data Transfer Object
    {
        //create domain object from request
        var item = request.ToDomain();

        //try to create an item
        try
        {
            //generate new Id
            if (items.Count == 0)
            {
                item.ToDoItemId = 1;
            }
            else
            {
                item.ToDoItemId = items.Max(o => o.ToDoItemId) + 1;
            }
            //add item to the list
            items.Add(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        return Created();
    }

    [HttpGet]
    public IActionResult Read()
    {
        try
        {
            if (items.Count > 0)
            {
                var dtos = items.Select(ToDoItemGetResponseDto.FromDomain).ToList();
                return Ok(dtos); //200 with data
            }
            else
            {
                return NotFound(); //404
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId)
    {
        try
        {
            throw new Exception("Neco se nepovedlo.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] TodoItemUpdateRequestDto request)
    {
        return Ok();
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        return Ok();
    }
}
