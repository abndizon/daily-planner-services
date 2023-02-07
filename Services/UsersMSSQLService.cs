namespace DailyPlannerServices.Services;

using System.Collections.Generic;
using DailyPlannerServices.Interfaces;
using DailyPlannerServices.Models;
using DailyPlannerServices.Data;

public class UsersMSSQLService : IUserService
{
    private readonly DataContext _dataContext;
    private readonly IToDoItemService _toDoItemService;

    public UsersMSSQLService(DataContext dataContext, IToDoItemService toDoItemService)
    {
        _dataContext = dataContext;
        _toDoItemService = toDoItemService;
    }

    public List<User> GetAll()
    {
        return _dataContext.Users.ToList();
    }

    public User GetUserById(int id)
    {
        return _dataContext.Users.SingleOrDefault(x => x.Id == id);
    }

    public List<ToDoItem> GetAllToDoItems(int id)
    {
        List<ToDoItem> allToDoItems = _toDoItemService.GetAll();
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

    public List<ToDoItem> GetToDoItemsByCategory(int id, int categoryId)
    {
        List<ToDoItem> allToDoItems = _toDoItemService.GetAll();
        List<ToDoItem> categorizedToDoItems = new List<ToDoItem>();

        categorizedToDoItems = allToDoItems.FindAll(x => x.UserId == id && x.CategoryId == categoryId);

        return categorizedToDoItems;
        // List<ToDoItem> toDoItems = new List<ToDoItem>();

        // SqlConnection connection = new SqlConnection(
        //     ApplicationContext.Instance.GetConnectionString()
        // );

        // connection.Open();

        // String sql = "SELECT * FROM ToDoItems WHERE UserId =  @userId AND CategoryId = @categoryId";
        // SqlCommand command = new SqlCommand(sql, connection);
        
        // command.Parameters.AddWithValue("@userId", id);
        // command.Parameters.AddWithValue("@categoryId", categoryId);
        // command.ExecuteNonQuery();

        // SqlDataReader reader = command.ExecuteReader();

        // while (reader.Read()) 
        // {
        //     int toDoItemId = (int) reader["Id"];
        //     string name  = reader["Name"].ToString();
        //     int catId = (int) reader["CategoryId"];
        //     DateTime date   = (DateTime) reader["Date"];
        //     string startTime = reader["StartTime"].ToString();
        //     string endTime = reader["EndTime"].ToString();
        //     User user = this.GetUserById((int) reader["UserId"]);

        //     ToDoItem item = new ToDoItem(toDoItemId, name, catId, date, startTime, endTime, user.Id);

        //     toDoItems.Add(item);
        // }

        // connection.Close();

        
    }

    public User Save(User user)
    {
        if (user.Id == null || user.Id == 0)
        {
            _dataContext.Users.Add(user);
        }
        else
        {
            _dataContext.Update(user);
        }
        
        _dataContext.SaveChanges();

        return user;
    }

    public void Delete(int id)
    {
        User userMatch = GetUserById(id);
        _dataContext.Remove(userMatch);
        _dataContext.SaveChanges();
    }
}