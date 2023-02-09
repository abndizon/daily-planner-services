namespace DailyPlannerServices.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DailyPlannerServices.Operations;
using DailyPlannerServices.Interfaces;
using System.Text.Json.Serialization;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    BuildUserFromPayload builder;
    JsonSerializerOptions seralizerOptions;

    public UsersController(IUserService userService)
    {
        _userService = userService;

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
            var users = _userService.GetAll();
            var payload = JsonSerializer.Serialize(users, seralizerOptions);

            return Ok(payload);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error getting users");
        }
    }

    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        try
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
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error getting users");
        }
    }

    [HttpPost("")]
    public IActionResult Save([FromBody] object payloadObj)
    {
        try
        {
            Dictionary<string, object> hash = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadObj.ToString());

            ValidateSaveUser validator = new ValidateSaveUser(hash);
            validator.Execute();

            if (validator.HasErrors())
            {
                return UnprocessableEntity(validator.Errors);
            }
            else
            {
                builder = new BuildUserFromPayload(hash);
                builder.Run();

                _userService.Save(builder.User);
                Dictionary<string, object> message = new Dictionary<string, object>();
                message.Add("message", "User created");

                return Ok(message);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error saving user");
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
