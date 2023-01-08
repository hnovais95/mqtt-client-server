namespace Server
{
    class CustomersControllerFactory
    {
        public static CustomersController MakeController(IServerNotificationCenter notificationCenter)
        {
            var customerRepository = new CustomerRepository();
            var customerService = new CustomerService(customerRepository);
            return new CustomersController(notificationCenter, customerService);
        }
    }
}
