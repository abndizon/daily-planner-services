namespace DailyPlannerServices.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DailyPlannerServices.Operations;
using DailyPlannerServices.Interfaces;
using System.Text.Json.Serialization;
using DailyPlannerServices.Models;

[ApiController]
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    BuildCategoryFromPayload builder;
    JsonSerializerOptions seralizerOptions;

    public CategoriesController(IToDoItemService toDoItemService, ICategoryService categoryService)
    {
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
        var categories = _categoryService.GetAll();
        var payload = JsonSerializer.Serialize(categories, seralizerOptions);

        return Ok(payload);
    }

    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        var category = _categoryService.GetCategoryById(id);

        if (category != null)
        {
            var payload = JsonSerializer.Serialize(category, seralizerOptions);
            return Ok(payload);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("{id}/todo_items")]
     public IActionResult ToDoItems(int id)
    {
        List<ToDoItem> toDoItems = _categoryService.GetToDoItemsByCategory(id);
        var payload = JsonSerializer.Serialize(toDoItems, seralizerOptions);

        return Ok(payload);
    }

    [HttpPost("")]
    public IActionResult Save([FromBody] object payloadObj)
    {
        Dictionary<string, object> hash = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadObj.ToString());

        ValidateSaveCategory validator = new ValidateSaveCategory(hash);
        validator.Execute();

        if (validator.HasErrors())
        {
            return UnprocessableEntity(validator.Errors);
        }
        else {
            builder = new BuildCategoryFromPayload(hash, _categoryService);
            builder.Run();

            _categoryService.Save(builder.Category);
            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("message", "Category created");

            return Ok(message);
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var categoryToDelete = _categoryService.GetCategoryById(id);

            if (categoryToDelete == null)
            {
                return NotFound($"Category with id {id} not found");
            }

            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("message", "Ok");

            _categoryService.Delete(id);
            return Ok(message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting category");
        }
    }
}
