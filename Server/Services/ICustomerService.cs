using System.Collections.Generic;
using DataAccess.Entities;

namespace Server
{
    public interface ICustomerService
    {
        public IEnumerable<CustomerEntity> GetAllCustomers();
    }
}
