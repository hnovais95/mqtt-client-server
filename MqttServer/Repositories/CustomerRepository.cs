using System.Collections.Generic;
using DataAccess;
using DataAccess.Entities;

namespace MqttServer
{
    public interface ICustomerRepository
    {
        public IEnumerable<CustomerEntity> GetAll();
    }

    public class CustomerRepository : ICustomerRepository
    {
        public IEnumerable<CustomerEntity> GetAll()
        {
            var customerDao = new CustomerDao();
            return customerDao.GetAll();
        }
    }
}
