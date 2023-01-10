using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace Server.Domain
{
    public interface ICustomerService
    {
        public Task<IEnumerable<Customer>> GetAllCustomers();
        public void AddCustomer(Customer customer);
    }
}
