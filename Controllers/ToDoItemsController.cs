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
    private readonly ICategoryService _categoryService;
    BuildToDoItemFromPayload builder;
    JsonSerializerOptions seralizerOptions;

    public ToDoItemsController(IToDoItemService toDoItemService, ICategoryService categoryService)
    {
        _toDoItemService = toDoItemService;
        _categoryService = categoryService;

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
        var payload = JsonSerializer.Serialize(items, seralizerOptions);

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
