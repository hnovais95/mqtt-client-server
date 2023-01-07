using System.Collections.Generic;
using DataAccess.Entities;

namespace MqttServer
{
    public interface ICustomerRepository
    {
        public IEnumerable<CustomerEntity> GetAll();
    }
}
