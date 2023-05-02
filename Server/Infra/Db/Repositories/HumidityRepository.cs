using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dommel;
using Server.Domain;

namespace Server.Infra.Db
{

    public class HumidityRepository : IRepository<HumiditySensor>
    {
        public async Task<IEnumerable<HumiditySensor>> GetAll()
        {
            using var conn = DbFactory.CreateConnection();
            return await conn.GetAllAsync<HumiditySensor>();
        }

        public void Insert(HumiditySensor sensor, int timeout = 0)
        {
            using var conn = DbFactory.CreateConnection();
            conn.Open();

            var sql = new StringBuilder("INSERT INTO rain(");
            sql.Append(" id,");
            sql.Append(" sensor,");
            sql.Append(" timestamp,");
            sql.Append(" region,");
            sql.Append(" rain");
            sql.Append(") values (");
            sql.Append(" @id,");
            sql.Append(" @sensor,");
            sql.Append(" @timestamp,");
            sql.Append(" @region,");
            sql.Append(" @rain");
            sql.Append(')');

            var cmd = DbFactory.CreateCommand(sql.ToString(), conn);
            cmd.CommandTimeout = timeout;
            cmd.Parameters.Add(DbFactory.CreateParameter("id", -1, DbType.Int64));
            cmd.Parameters.Add(DbFactory.CreateParameter("sensor", sensor.Name, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("timestamp", sensor.Timestamp, DbType.DateTime));
            cmd.Parameters.Add(DbFactory.CreateParameter("region", sensor.Region, DbType.Int32));
            cmd.Parameters.Add(DbFactory.CreateParameter("rain", sensor.Humidity, DbType.Double));

            if (cmd.ExecuteNonQuery() == 0)
            {
                throw new Exception("Erro na inserção do registro na tabela customers.");
            }
        }
    }
}