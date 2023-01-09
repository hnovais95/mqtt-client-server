using System.Collections.Generic;
using DataAccess;
using DataAccess.Entities;

namespace Server
{
    public class CustomerService: ICustomerService
    {
        private readonly CustomerDao _customerDao;

        public CustomerService(CustomerDao customerDao)
        {
            _customerDao = customerDao;
        }

        public IEnumerable<CustomerEntity> GetAllCustomers()
        {
            return _customerDao.GetAll();
        }

        public void AddCustomer(CustomerEntity customer)
        {
            _customerDao.Insert(customer);
        }
    }
}
