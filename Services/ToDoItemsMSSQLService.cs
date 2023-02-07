namespace DailyPlannerServices.Services;

using System.Collections.Generic;
using DailyPlannerServices.Interfaces;
using DailyPlannerServices.Models;
using DailyPlannerServices.Data;

public class ToDoItemsMSSQLService : IToDoItemService
{
    private readonly DataContext _dataContext;

    public ToDoItemsMSSQLService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public List<ToDoItem> GetAll()
    {
        return _dataContext.ToDoItems.ToList<ToDoItem>();
    }
    public ToDoItem GetItemById(int id)
    {
        return _dataContext.ToDoItems.SingleOrDefault(x => x.Id == id);
    }

    public ToDoItem Save(ToDoItem item)
    {
        if (item.Id == null || item.Id == 0)
        {
            _dataContext.ToDoItems.Add(item);
        }
        else
        {
            ToDoItem temp = this.GetItemById(item.Id);
            temp.Name = item.Name;
            temp.CategoryId = item.CategoryId;
            temp.Date = item.Date;
            temp.StartTime = item.StartTime;
            temp.EndTime = item.EndTime;
            temp.Category = item.Category;
        }

        _dataContext.SaveChanges();

        return item;
    }

    public void Delete(int id)
    {
        ToDoItem toDoItemMatch = GetItemById(id);
        _dataContext.Remove(toDoItemMatch);
        _dataContext.SaveChanges();
    }
}