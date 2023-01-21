using Server.Infra.Db;
using Server.Data;
using Server.Presentation;

namespace Server.Main
{
    class CustomersControllerFactory
    {
        public static CustomersController CreateController(IServerNotificationCenter notificationCenter)
        {
            var customerRepository = new CustomerRepository();
            var customerService = new CustomerService(customerRepository);
            return new CustomersController(notificationCenter, customerService);
        }
    }
}
