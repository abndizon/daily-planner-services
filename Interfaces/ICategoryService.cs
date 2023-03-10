using DailyPlannerServices.Models;

namespace DailyPlannerServices.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category GetCategoryById(int id);
        Category Save(Category category);
        void Delete(int id);
    }
}