using Mqtt;

namespace MqttServer
{
    public class CustomersControllerFactory
    {
        public static CustomersController MakeController(IRouter router)
        {
            var customerRepository = new CustomerRepository();
            var customerService = new CustomerService(customerRepository);
            return new CustomersController(router, customerService);
        }
    }
}
