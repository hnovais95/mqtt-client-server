using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Mqtt;
using Common.Models;
using Server.Domain;

namespace Server.Presentation
{
    [Subscribe("sys/client/+/customers/get/+"), Subscribe("sys/client/+/customers/add/+")]
    class CustomersController
    {
        private readonly IServerNotificationCenter _notificationCenter;
        private readonly ICustomerService _customerService;

        public CustomersController(IServerNotificationCenter notificationCenter, ICustomerService customerService)
        {
            _notificationCenter = notificationCenter;
            _notificationCenter.OnRequestCustomers += NotificationCenter_OnRequestCustomers;
            _notificationCenter.OnAddCustomer += NotificationCenter_OnAddCustomer;
            _customerService = customerService;
        }

        private void NotificationCenter_OnRequestCustomers(MqttMessage mqttMessage)
        {
            Task.Run(async () =>
            {
                var result = new RequestResult();

                try
                {
                    var customers = await _customerService.GetAllCustomers();
                    result.ResultCode = RequestResultCode.Success;
                    result.Body = customers.Select(x => new CustomerDTO()
                    {
                        CustomerID = x.CustomerID,
                        CompanyName = x.CompanyName,
                        ContactName = x.ContactName,
                        ContactTitle = x.ContactTitle,
                        Address = x.Address,
                        City = x.City,
                        Region = x.Region,
                        PostalCode = x.PostalCode,
                        Country = x.Country,
                        Phone = x.Phone,
                        Fax = x.Fax
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao tratar requisção. Tópico: {mqttMessage.Topic}; Payload: {mqttMessage.Payload}; Exc.: {e}");
                    result.ResultCode = RequestResultCode.Failure;
                    result.Message = "Erro recuperar clientes.";
                }
                finally
                {
                    try
                    {
                        _notificationCenter.Publish(ServerCommand.GetCustomersResponse, result, mqttMessage.GetID());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }

        private void NotificationCenter_OnAddCustomer(MqttMessage mqttMessage)
        {
            Task.Run(() =>
            {
                var result = new RequestResult();

                try
                {
                    var customerDto = JsonSerializer.Deserialize<CustomerDTO>((string)mqttMessage.Payload);
                    var customer = new Customer() 
                    {
                        CustomerID = customerDto.CustomerID,
                        CompanyName = customerDto.CompanyName,
                        ContactName = customerDto.ContactName,
                        ContactTitle = customerDto.ContactTitle,
                        Address = customerDto.Address,
                        City = customerDto.City,
                        Region = customerDto.Region,
                        PostalCode = customerDto.PostalCode,
                        Country = customerDto.Country,
                        Phone = customerDto.Phone,
                        Fax = customerDto.Fax
                    };

                    _customerService.AddCustomer(customer);
                    result.ResultCode = RequestResultCode.Success;
                    result.Message = "Cliente adicionado com sucesso.";
                    Console.WriteLine($"Cliente adicionado com sucesso. ID do Cliente: {customer.CustomerID}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao tratar requisção. Tópico: {mqttMessage.Topic}; Payload: {mqttMessage.Payload}; Exc.: {e}");
                    result.ResultCode = RequestResultCode.Failure;
                    result.Message = "Erro ao adicionar cliente";
                }
                finally
                {
                    try
                    {
                        _notificationCenter.Publish(ServerCommand.AddCustomerResponse, result, mqttMessage.GetID());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }
    }
}
