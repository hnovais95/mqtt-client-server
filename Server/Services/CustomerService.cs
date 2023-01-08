using System.Collections.Generic;
using DataAccess.Entities;

namespace Server
{
    public class CustomerService: ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<CustomerEntity> GetAllCustomers()
        {
            return _customerRepository.GetAll();
        }
    }
}
