namespace DailyPlannerServices.Operations;

using DailyPlannerServices.Models;
using DailyPlannerServices.Interfaces;

public class BuildToDoItemFromPayload {
    private readonly IToDoItemService _toDoItemService;
    private Dictionary<string, object> data;
    public ToDoItem Item { get; private set; }

    public BuildToDoItemFromPayload(Dictionary<string, object> data, IToDoItemService toDoItemService)
    {
        _toDoItemService = toDoItemService;
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
        int categoryId = Convert.ToInt32(data["category_id"].ToString());
        DateTime date = DateTime.ParseExact(data["date"].ToString(), "yyyy-MM-dd", null);
        string starTime = data["start_time"].ToString();
        string endTime = data["end_time"].ToString();       
        int userId = Convert.ToInt32(data["user_id"].ToString());

        Item = new ToDoItem(id, name, categoryId, date, starTime, endTime, userId);
    }
}