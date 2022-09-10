using System.Data;
using Npgsql;

namespace DataAccess
{
    public class ConnectionFactory
    {
        public static IDbConnection GetConnection()
        {
            var connectionString = "Server=localhost;Database=northwind;Port=5432;User Id=postgres;Password=admin;";
            var conn = new NpgsqlConnection(connectionString);
            return conn;
        }
    }
}
