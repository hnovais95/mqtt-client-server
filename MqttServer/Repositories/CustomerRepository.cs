using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Dao;

namespace MqttServer.Repositories
{
    public interface ICustomerRepository
    {
        public IEnumerable<Customer> GetAll();
    }

    public class CustomerRepository : ICustomerRepository
    {
        public IEnumerable<Customer> GetAll()
        {
            var customerDao = new CustomerDao();
            return customerDao.GetAll();
        }
    }
}
