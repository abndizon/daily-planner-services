using DailyPlannerServices.Models;

namespace DailyPlannerServices.Interfaces
{
    public interface IToDoItemService
    {
        List<ToDoItem> GetAll();
        ToDoItem GetItemById(int id);
        List<ToDoItem> GetItemsByCategory(int id);
        List<ToDoItem> GetItemsByDate(string date);
        List<ToDoItem> GetItemsByCategoryAndDate(int id, string date);
        List<ToDoItem> GetItemsByUser(int id);
        ToDoItem Save(ToDoItem item);
        void Delete(int id);
    }
}