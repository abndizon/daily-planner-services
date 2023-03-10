namespace DailyPlannerServices.Services;

using System.Collections.Generic;
using DailyPlannerServices.Interfaces;
using DailyPlannerServices.Models;
using DailyPlannerServices.Data;

public class CategoriesMSSQLService : ICategoryService
{
    private readonly DataContext _dataContext;

    public CategoriesMSSQLService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public List<Category> GetAll()
    {
        return _dataContext.Categories.ToList<Category>();
    }

    public Category GetCategoryById(int id)
    {
        return _dataContext.Categories.SingleOrDefault(x => x.Id == id);
    }

    public Category Save(Category category)
    {
        if (category.Id == null || category.Id == 0)
        {
            _dataContext.Categories.Add(category);
        }
        else
        {
            Category temp = this.GetCategoryById(category.Id);
            temp.Name = category.Name;
        }

        _dataContext.SaveChanges();

        return category;
    }

    public void Delete(int id)
    {
        Category categoryMatch = GetCategoryById(id);
        _dataContext.Remove(categoryMatch);
        _dataContext.SaveChanges();
    }
}