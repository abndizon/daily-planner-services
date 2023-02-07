namespace DailyPlannerServices.Data;

using Microsoft.EntityFrameworkCore;
using DailyPlannerServices.Models;

public class DataContext : DbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {

    }
}