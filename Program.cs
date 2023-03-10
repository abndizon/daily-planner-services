using Microsoft.EntityFrameworkCore;
using DailyPlannerServices.Interfaces;
using DailyPlannerServices.Services;
using DailyPlannerServices.Data;

namespace DailyPlannerServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            
            var corsConfigName = "CORS-Config";

            builder.Services.AddCors(options => {
                options.AddPolicy(name: corsConfigName, policy => {
                    policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                });
            });
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddDbContext<DataContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MSSqlConnection"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add services
            builder.Services.AddScoped<IToDoItemService, ToDoItemsMSSQLService>();
            builder.Services.AddScoped<ICategoryService, CategoriesMSSQLService>();
            builder.Services.AddScoped<IStatusService, StatusesMSSQLService>();
            builder.Services.AddScoped<IUserService, UsersMSSQLService>();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(corsConfigName);
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}


