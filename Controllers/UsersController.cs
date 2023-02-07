namespace DailyPlannerServices.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DailyPlannerServices.Operations;
using DailyPlannerServices.Interfaces;
using DailyPlannerServices.Models;
using System.Text.Json.Serialization;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IToDoItemService _toDoItemService;
    BuildUserFromPayload builder;
    JsonSerializerOptions seralizerOptions;

    public UsersController(IUserService userService, IToDoItemService toDoItemService)
    {
        _userService = userService;
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
        var users = _userService.GetAll();
        var payload = JsonSerializer.Serialize(users, seralizerOptions);

        return Ok(payload);
    }

    [HttpGet("{id}/todo_items")]
    public IActionResult ToDoItems(int id)
    {
        List<ToDoItem> toDoItems = _userService.GetAllToDoItems(id);
        var payload = JsonSerializer.Serialize(toDoItems, seralizerOptions);

        return Ok(payload);
    }

    [HttpGet("{id}/todo_items/category/{categoryId}")]
    public IActionResult ToDoItems(int id, int categoryId)
    {
        List<ToDoItem> toDoItems = _userService.GetToDoItemsByCategory(id, categoryId);
        var payload = JsonSerializer.Serialize(toDoItems, seralizerOptions);

        return Ok(payload);
    }

    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        var user = _userService.GetUserById(id);

        if (user != null)
        {
            var payload = JsonSerializer.Serialize(user, seralizerOptions);
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

        ValidateSaveUser validator = new ValidateSaveUser(hash);
        validator.Execute();

        if (validator.HasErrors())
        {
            return UnprocessableEntity(validator.Errors);
        }
        else {
            builder = new BuildUserFromPayload(hash);
            builder.Run();

            _userService.Save(builder.User);
            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("message", "User created");

            return Ok(message);
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var userToDelete = _userService.GetUserById(id);

            if (userToDelete == null)
            {
                return NotFound($"User with id {id} not found");
            }

            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("message", "Ok");

            _userService.Delete(id);
            return Ok(message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting user");
        }
    }
}
