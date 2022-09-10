using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using DataAccess.Entities;

namespace DataAccess
{
    public class CustomerDao
    {
        public IEnumerable<CustomerEntity> GetAll()
        {
            using (var conn = ConnectionFactory.GetConnection())
            {
                return conn.GetAll<CustomerEntity>();
            }
        }
    }
}