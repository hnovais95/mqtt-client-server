using System.Collections.Generic;
using DataAccess;
using DataAccess.Entities;

namespace MqttServer
{

    public class CustomerRepository : ICustomerRepository
    {
        public IEnumerable<CustomerEntity> GetAll()
        {
            var customerDao = new CustomerDao();
            return customerDao.GetAll();
        }
    }
}
