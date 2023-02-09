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

    public List<ToDoItem> GetItemsByCategory(int id)
    {
        List<ToDoItem> allToDoItems = this.GetAll();
        List<ToDoItem> categorizedToDoItems = new List<ToDoItem>();

        categorizedToDoItems = allToDoItems.FindAll(x => x.CategoryId == id);

        return categorizedToDoItems;
    }

    public List<ToDoItem> GetItemsByDate(string date)
    {
        List<ToDoItem> allToDoItems = this.GetAll();
        List<ToDoItem> filteredToDoItems = new List<ToDoItem>();

        filteredToDoItems = allToDoItems.FindAll(x => x.Date.ToString("yyyy-MM-dd") == date);

        return filteredToDoItems;
    }

    public List<ToDoItem> GetItemsByCategoryAndDate(int categoryId, string date)
    {
        List<ToDoItem> allToDoItems = this.GetAll();
        List<ToDoItem> filteredToDoItems = new List<ToDoItem>();

        filteredToDoItems = allToDoItems.FindAll(x => x.CategoryId == categoryId && x.Date.ToString("yyyy-MM-dd") == date);

        return filteredToDoItems;
    }

    public List<ToDoItem> GetItemsByUser(int id)
    {
        List<ToDoItem> allToDoItems = this.GetAll();
        List<ToDoItem> myToDoItems = new List<ToDoItem>();

        myToDoItems = allToDoItems.FindAll(x => x.UserId == id);

        return myToDoItems;

        // SqlConnection connection = new SqlConnection(
        //     ApplicationContext.Instance.GetConnectionString()
        // );

        // connection.Open();

        // String sql = "SELECT * FROM ToDoItems WHERE UserId =  @userId";
        // SqlCommand command = new SqlCommand(sql, connection);
        
        // command.Parameters.AddWithValue("@userId", id);
        // command.ExecuteNonQuery();

        // SqlDataReader reader = command.ExecuteReader();

        // while (reader.Read()) 
        // {
        //     int toDoItemId = (int) reader["Id"];
        //     string name  = reader["Name"].ToString();
        //     int categoryId = (int) reader["CategoryId"];
        //     DateTime date   = (DateTime) reader["Date"];
        //     string startTime = reader["StartTime"].ToString();
        //     string endTime = reader["EndTime"].ToString();
        //     User user = this.GetUserById((int) reader["UserId"]);

        //     ToDoItem item = new ToDoItem(toDoItemId, name, categoryId, date, startTime, endTime, user.Id);

        //     toDoItems.Add(item);
        // }

        // connection.Close();
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
            temp.UserId = item.UserId;
            temp.StatusId = item.StatusId;
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