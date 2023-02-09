namespace DailyPlannerServices.Services;

using System.Collections.Generic;
using DailyPlannerServices.Interfaces;
using DailyPlannerServices.Models;
using DailyPlannerServices.Data;

public class UsersMSSQLService : IUserService
{
    private readonly DataContext _dataContext;

    public UsersMSSQLService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public List<User> GetAll()
    {
        return _dataContext.Users.ToList();
    }

    public User GetUserById(int id)
    {
        return _dataContext.Users.SingleOrDefault(x => x.Id == id);
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