namespace DailyPlannerServices.Operations;

using DailyPlannerServices.Models;
using DailyPlannerServices.Interfaces;

public class BuildStatusFromPayload {
    private Dictionary<string, object> data;
    public Status Status { get; private set; }

    public BuildStatusFromPayload(Dictionary<string, object> data)
    {
        this.data = data;
    }

    public void Run()
    {
        // Assign id, Default: 0
        int id = 0;
        if(data.ContainsKey("id"))
        {
            id = Convert.ToInt32(data["id"].ToString());
        }
        
        // Name validation
        string name = data["name"].ToString();

        Status = new Status(id, name);
    }
}