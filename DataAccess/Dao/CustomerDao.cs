using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using DataAccess.Entities;

namespace DataAccess.Dao
{
    public class CustomerDao
    {
        public IEnumerable<Customer> GetAll()
        {
            using (var conn = ConnectionFactory.GetConnection())
            {
                return conn.GetAll<Customer>();
            }
        }
    }
}