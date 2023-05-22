using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Server.Domain;

namespace Server.Data
{
    public class CustomerService: ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerDao)
        {
            _customerRepository = customerDao;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetAll();
        }

        public void AddCustomer(Customer customer)
        {
            _customerRepository.Insert(customer, 5);
        }
    }
}
