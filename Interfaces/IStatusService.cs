using DailyPlannerServices.Models;

namespace DailyPlannerServices.Interfaces
{
    public interface IStatusService
    {
        List<Status> GetAll();
        Status GetStatusById(int id);
        Status Save(Status status);
        void Delete(int id);
    }
}