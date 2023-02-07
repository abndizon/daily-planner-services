using DailyPlannerServices.Models;

namespace DailyPlannerServices.Interfaces
{
    public interface IToDoItemService
    {
        List<ToDoItem> GetAll();
        ToDoItem GetItemById(int id);
        ToDoItem Save(ToDoItem item);
        void Delete(int id);
    }
}