namespace DailyPlannerServices.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DailyPlannerServices.Operations;
using DailyPlannerServices.Interfaces;
using System.Text.Json.Serialization;

[ApiController]
[Route("statuses")]
public class StatusesController : ControllerBase
{
    private readonly IStatusService _statusService;
    BuildStatusFromPayload builder;
    JsonSerializerOptions seralizerOptions;

    public StatusesController(IStatusService statusService)
    {
        _statusService = statusService;

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
            var categories = _statusService.GetAll();
            var payload = JsonSerializer.Serialize(categories, seralizerOptions);

            return Ok(payload);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error getting statuses");
        }
    }

    [HttpGet("{id}")]
    public IActionResult Show(int id)
    {
        try
        {
            var status = _statusService.GetStatusById(id);

            if (status != null)
            {
                var payload = JsonSerializer.Serialize(status, seralizerOptions);
                return Ok(payload);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error in getting status");
        }
    }

    [HttpPost("")]
    public IActionResult Save([FromBody] object payloadObj)
    {
        try
        {
            Dictionary<string, object> hash = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadObj.ToString());

            ValidateSaveStatus validator = new ValidateSaveStatus(hash);
            validator.Execute();

            if (validator.HasErrors())
            {
                return UnprocessableEntity(validator.Errors);
            }
            else
            {
                builder = new BuildStatusFromPayload(hash);
                builder.Run();

                _statusService.Save(builder.Status);
                Dictionary<string, object> message = new Dictionary<string, object>();
                message.Add("message", "Status created");

                return Ok(message);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error in saving status");
        }
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var statusToDelete = _statusService.GetStatusById(id);

            if (statusToDelete == null)
            {
                return NotFound($"Status with id {id} not found");
            }

            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("message", "Ok");

            _statusService.Delete(id);
            return Ok(message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting status");
        }
    }
}
