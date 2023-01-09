using DataAccess;

namespace Server
{
    class CustomersControllerFactory
    {
        public static CustomersController MakeController(IServerNotificationCenter notificationCenter)
        {
            var customerDao = new CustomerDao();
            var customerService = new CustomerService(customerDao);
            return new CustomersController(notificationCenter, customerService);
        }
    }
}
