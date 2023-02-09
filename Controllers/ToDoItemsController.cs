namespace DailyPlannerServices.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DailyPlannerServices.Operations;
using DailyPlannerServices.Interfaces;
using System.Text.Json.Serialization;

[ApiController]
[Route("todo_items")]
public class ToDoItemsController : ControllerBase
{
    private readonly IToDoItemService _toDoItemService;
    BuildToDoItemFromPayload builder;
    JsonSerializerOptions seralizerOptions;

    public ToDoItemsController(IToDoItemService toDoItemService)
    {
        _toDoItemService = toDoItemService;

        seralizerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var items = _toDoItemService.GetAll();
        var sorted = items.OrderBy(x => x.StartTime)
                                    .ThenBy(x => x.Name)
                                    .ToList();
        var payload = JsonSerializer.Serialize(sorted, seralizerOptions);

        return Ok(payload);
    }

    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        var item = _toDoItemService.GetItemById(id);

        if (item != null)
        {
            var payload = JsonSerializer.Serialize(item, seralizerOptions);
            return Ok(payload);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("category/{id}/date/{date}")]
     public IActionResult FilterCategoryAndDate(int id, string date)
    {
        var toDoItems = _toDoItemService.GetItemsByCategoryAndDate(id, date);
        var sorted = toDoItems.OrderBy(x => x.StartTime)
                                    .ThenBy(x => x.Name)
                                    .ToList();
        var payload = JsonSerializer.Serialize(sorted, seralizerOptions);

        return Ok(payload);
    }

    [HttpGet("category/{id}")]
     public IActionResult FilterByCategory(int id)
    {
        var toDoItems = _toDoItemService.GetItemsByCategory(id);
        var sorted = toDoItems.OrderBy(x => x.StartTime)
                                    .ThenBy(x => x.Name)
                                    .ToList();
        var payload = JsonSerializer.Serialize(sorted, seralizerOptions);

        return Ok(payload);
    }

    [HttpGet("date/{date}")]
     public IActionResult FilterByDate(string date)
    {
        var toDoItems = _toDoItemService.GetItemsByDate(date);
        var sorted = toDoItems.OrderBy(x => x.StartTime)
                                    .ThenBy(x => x.Name)
                                    .ToList();
        var payload = JsonSerializer.Serialize(sorted, seralizerOptions);

        return Ok(payload);
    }

    [HttpGet("user/{id}")]
    public IActionResult IndexByUser(int id)
    {
        var toDoItems = _toDoItemService.GetItemsByUser(id);
        var payload = JsonSerializer.Serialize(toDoItems, seralizerOptions);

        return Ok(payload);
    }

    [HttpPost("")]
    public IActionResult Save([FromBody] object payloadObj)
    {
        Dictionary<string, object> hash = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadObj.ToString());

        ValidateSaveToDoItem validator = new ValidateSaveToDoItem(hash);
        validator.Execute();

        if (validator.HasErrors())
        {
            return UnprocessableEntity(validator.Errors);
        }
        else {
            builder = new BuildToDoItemFromPayload(hash, _toDoItemService);
            builder.Run();

            _toDoItemService.Save(builder.Item);
            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("message", "Item created");

            return Ok(message);
        }
    }

    [HttpPut("")]
    public IActionResult Update([FromBody] object payloadObj)
    {
        Dictionary<string, object> hash = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadObj.ToString());

        ValidateSaveToDoItem validator = new ValidateSaveToDoItem(hash);
        validator.Execute();

        if (validator.HasErrors())
        {
            return UnprocessableEntity(validator.Errors);
        }
        else {
            builder = new BuildToDoItemFromPayload(hash, _toDoItemService);
            builder.Run();

            _toDoItemService.Save(builder.Item);
            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("message", "Item updated");

            return Ok(message);
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var itemToDelete = _toDoItemService.GetItemById(id);

            if (itemToDelete == null)
            {
                return NotFound($"Item with id {id} not found");
            }

            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("message", "Ok");

            _toDoItemService.Delete(id);
            return Ok(message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting item");
        }
    }
}
