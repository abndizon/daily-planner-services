using DailyPlannerServices.Models;

namespace DailyPlannerServices.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User GetUserById(int id);
        User Save(User user);
        void Delete(int id);
    }
}