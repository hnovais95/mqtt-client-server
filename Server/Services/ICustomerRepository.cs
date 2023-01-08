using System.Collections.Generic;
using DataAccess.Entities;

namespace Server
{
    public interface ICustomerRepository
    {
        public IEnumerable<CustomerEntity> GetAll();
    }
}
