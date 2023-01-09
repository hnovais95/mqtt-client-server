using DataAccess.Entities;
using Common.Models;

namespace Server
{
    static class CustomerModelExtensions
    {
        public static CustomerEntity ConvertToEntity(this CustomerModel customer)
        {
            return new CustomerEntity
            {
                CustomerID = customer.CustomerID,
                CompanyName = customer.CompanyName,
                ContactName = customer.ContactName,
                ContactTitle = customer.ContactTitle,
                Address = customer.Address,
                City = customer.City,
                Region = customer.Region,
                PostalCode = customer.PostalCode,
                Country = customer.Country,
                Phone = customer.Phone,
                Fax = customer.Fax,
            };
        }
    }
}
