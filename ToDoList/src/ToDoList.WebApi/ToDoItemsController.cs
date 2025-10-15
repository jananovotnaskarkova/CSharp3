namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost:5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    public readonly List<ToDoItem> items = [];

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
        return CreatedAtAction(actionName: nameof(ReadById), // Which method to use to get the resource
                               routeValues: new { toDoItemId = item.ToDoItemId }, // Parameters needed to call that method
                               value: ToDoItemGetResponseDto.FromDomain(item) // The created item to return
                               ); //201
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        //try to read all items
        try
        {
            var dtos = items.Select(ToDoItemGetResponseDto.FromDomain).ToList();
            return Ok(dtos); //200 with data
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
    {
        //try to read an item
        try
        {
            //find the item to read
            var itemToRead = items.Find(o => o.ToDoItemId == toDoItemId);

            if (itemToRead == null)
            {
                return NotFound(); //404
            }
            else
            {
                //convert domain object to dto
                var dto = ToDoItemGetResponseDto.FromDomain(itemToRead);
                return Ok(dto); //200 with data
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> UpdateById(int toDoItemId, [FromBody] TodoItemUpdateRequestDto request)
    {
        //create domain object from request
        var itemUpdated = request.ToDomain();
        itemUpdated.ToDoItemId = toDoItemId;

        //try to update an item
        try
        {
            //find index of the item to update
            int index = items.FindIndex(o => o.ToDoItemId == toDoItemId);

            if (index == -1)
            {
                return NotFound(); //404
            }
            else
            {
                //update the item at the found index
                items[index] = itemUpdated;
                var dto = ToDoItemGetResponseDto.FromDomain(items[index]);
                return Ok(dto); //200
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        //try to delete an item
        try
        {
            //find the item to delete
            var itemToDelete = items.Find(o => o.ToDoItemId == toDoItemId);

            if (itemToDelete == null)
            {
                return NotFound(); //404
            }
            else
            {
                //delete the item
                items.Remove(itemToDelete);
                return NoContent(); //204
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    public void AddItemToStorage(ToDoItem item) => items.Add(item);
}


