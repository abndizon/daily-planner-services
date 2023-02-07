namespace DailyPlannerServices.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ToDoItem> ToDoItems { get; set; }

    public Category(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}
