using DailyPlannerServices.Models;

namespace DailyPlannerServices.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User GetUserById(int id);
        List<ToDoItem> GetAllToDoItems(int id);
        List<ToDoItem> GetToDoItemsByCategory(int id, int categoryId);
        User Save(User user);
        void Delete(int id);
    }
}