using System.Collections.Generic;
using DataAccess.Entities;

namespace MqttServer
{
    public interface ICustomerService
    {
        public IEnumerable<CustomerEntity> GetAllCustomers();
    }
}
