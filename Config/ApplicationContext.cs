using Microsoft.Data.SqlClient;
using DailyPlannerServices.Models;

namespace DailyPlannerServices.Config
{
    public class ApplicationContext
    {
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
        {}

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
    }
}