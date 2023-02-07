namespace DailyPlannerServices.Operations;

using DailyPlannerServices.Models;
using DailyPlannerServices.Interfaces;

public class BuildUserFromPayload {
    private Dictionary<string, object> data;
    public User User { get; private set; }

    public BuildUserFromPayload(Dictionary<string, object> data)
    {
        this.data = data;
    }

    public void Run()
    {
        int id = 0;
        if(data.ContainsKey("id"))
        {
            id = Convert.ToInt32(data["id"].ToString());
        }
        
        string firstName = data["firstName"].ToString();
        string lastName = data["lastName"].ToString();
        string emailAddress = data["emailAddress"].ToString();

        User = new User(id, firstName, lastName, emailAddress);
    }
}