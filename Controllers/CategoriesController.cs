namespace DailyPlannerServices.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DailyPlannerServices.Operations;
using DailyPlannerServices.Interfaces;
using System.Text.Json.Serialization;

[ApiController]
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    BuildCategoryFromPayload builder;
    JsonSerializerOptions seralizerOptions;

    public CategoriesController(ICategoryService categoryService)
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
        try
        {
            var categories = _categoryService.GetAll();
            var payload = JsonSerializer.Serialize(categories, seralizerOptions);

            return Ok(payload);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error getting categories");
        }
    }

    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        try
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
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error in getting category");
        }
    }

    [HttpPost("")]
    public IActionResult Save([FromBody] object payloadObj)
    {
        try
        {
            Dictionary<string, object> hash = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadObj.ToString());

            ValidateSaveCategory validator = new ValidateSaveCategory(hash);
            validator.Execute();

            if (validator.HasErrors())
            {
                return UnprocessableEntity(validator.Errors);
            }
            else
            {
                builder = new BuildCategoryFromPayload(hash);
                builder.Run();

                _categoryService.Save(builder.Category);
                Dictionary<string, object> message = new Dictionary<string, object>();
                message.Add("message", "Category created");

                return Ok(message);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error in saving category");
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
