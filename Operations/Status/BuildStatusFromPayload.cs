namespace DailyPlannerServices.Operations;

using DailyPlannerServices.Models;
using DailyPlannerServices.Interfaces;

public class BuildStatusFromPayload {
    private readonly IStatusService _statusService;
    private Dictionary<string, object> data;
    public Status Status { get; private set; }

    public BuildStatusFromPayload(Dictionary<string, object> data, IStatusService statusService)
    {
        _statusService = statusService;
        this.data = data;
    }

    public void Run()
    {
        int id = 0;
        if(data.ContainsKey("id"))
        {
            id = Convert.ToInt32(data["id"].ToString());
        }
        
        string name = data["name"].ToString();

        Status = new Status(id, name);
    }
}