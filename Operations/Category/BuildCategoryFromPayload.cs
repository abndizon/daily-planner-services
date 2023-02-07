namespace DailyPlannerServices.Operations;

using DailyPlannerServices.Models;
using DailyPlannerServices.Interfaces;

public class BuildCategoryFromPayload {
    private readonly ICategoryService _categoryService;
    private Dictionary<string, object> data;
    public Category Category { get; private set; }

    public BuildCategoryFromPayload(Dictionary<string, object> data, ICategoryService categoryService)
    {
        _categoryService = categoryService;
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

        Category = new Category(id, name);
    }
}