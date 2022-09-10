using Mqtt;

namespace MqttServer
{
    public class CustomersControllerFactory
    {
        public static CustomersController MakeController(IMqtt mqttClient)
        {
            var customerRepository = new CustomerRepository();
            var customerService = new CustomerService(customerRepository);
            return new CustomersController(mqttClient, customerService);
        }
    }
}
