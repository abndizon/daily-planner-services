namespace DailyPlannerServices.Models;

public class Status
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ToDoItem> ToDoItems { get; set; }

    public Status(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}
