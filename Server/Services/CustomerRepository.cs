using System.Collections.Generic;
using DataAccess;
using DataAccess.Entities;

namespace Server
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
