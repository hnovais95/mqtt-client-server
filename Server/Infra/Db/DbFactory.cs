using System;
using System.Data;
using Npgsql;

namespace Server.Infra.Db
{
    public class DbFactory
    {
        public static IDbConnection CreateConnection()
        {
            var connectionString = "Server=localhost;Database=northwind;Port=5432;User Id=postgres;Password=admin;";
            var conn = new NpgsqlConnection(connectionString);
            return conn;
        }

        public static IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            var cmd = new NpgsqlCommand(commandText, (NpgsqlConnection)connection);
            return cmd;
        }

        public static IDbDataParameter CreateParameter(string name, object value, DbType dbType)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var param = new NpgsqlParameter(name, value);
            param.DbType = dbType;
            return param;
        }
    }
}
