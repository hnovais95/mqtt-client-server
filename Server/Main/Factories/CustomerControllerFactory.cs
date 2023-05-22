using Server.Infra.Db;
using Server.Data;
using Server.Presentation;

namespace Server.Main
{
    class CustomerControllerFactory
    {
        public static CustomerController Create(ServerNotificationCenter notificationCenter)
        {
            var customerRepository = new CustomerRepository();
            var customerService = new CustomerService(customerRepository);
            return new CustomerController(notificationCenter, customerService);
        }
    }
}
