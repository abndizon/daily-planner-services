namespace DailyPlannerServices.Services;

using System.Collections.Generic;
using DailyPlannerServices.Interfaces;
using DailyPlannerServices.Models;
using DailyPlannerServices.Data;

public class StatusesMSSQLService : IStatusService
{
    private readonly DataContext _dataContext;

    public StatusesMSSQLService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public List<Status> GetAll()
    {
        return _dataContext.Status.ToList<Status>();
    }

    public Status GetStatusById(int id)
    {
        return _dataContext.Status.SingleOrDefault(x => x.Id == id);
    }

    public Status Save(Status status)
    {
        if (status.Id == null || status.Id == 0)
        {
            _dataContext.Status.Add(status);
        }
        else
        {
            Status temp = this.GetStatusById(status.Id);
            temp.Name = status.Name;
        }

        _dataContext.SaveChanges();

        return status;
    }

    public void Delete(int id)
    {
        Status statusMatch = GetStatusById(id);
        _dataContext.Remove(statusMatch);
        _dataContext.SaveChanges();
    }
}