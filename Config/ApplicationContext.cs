using Microsoft.Data.SqlClient;
using DailyPlannerServices.Models;

namespace DailyPlannerServices.Config
{
    public class ApplicationContext
    {
        private List<ToDoItem> itemLists;
        private List<User> userLists;

        private static ApplicationContext instance = null;

        public static ApplicationContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApplicationContext();
                }
                return instance;
            }
        }

        public ApplicationContext()
        {
            this.itemLists = new List<ToDoItem>();
            this.userLists = new List<User>();
        }

        public String GetConnectionString()
        {
            String connectionString = "";

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.InitialCatalog = "DailyPlannerDB";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;

            connectionString = builder.ConnectionString;

            return connectionString;
        }

        public List<ToDoItem> GetItems()
        {
            return this.itemLists;
        }

        public List<User> GetUsers()
        {
            return this.userLists;
        }


    }
}