namespace DailyPlannerServices.Operations;

using DailyPlannerServices.Models;

public class BuildCategoryFromPayload {
    private Dictionary<string, object> data;
    public Category Category { get; private set; }

    public BuildCategoryFromPayload(Dictionary<string, object> data)
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
        
        string name = data["name"].ToString();

        Category = new Category(id, name);
    }
}