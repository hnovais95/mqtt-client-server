using System;

namespace Server.Domain
{
    public class Customer
    {
        private string _customerID;
        public string CustomerID { 
            get { return _customerID; }
            set
            {
                if (value.Length == 0)
                {
                    throw new Exception("CustomerID é obrigatório.");
                }
                _customerID = value;
            }
        }

        public string CompanyName { get; set; }

        private string _contactName;
        public string ContactName
        {
            get { return _contactName; }
            set
            {
                if (value.Length == 0)
                {
                    throw new Exception("ContactName é obrigatório.");
                }
                _contactName = value;
            }
        }

        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value.Length == 0)
                {
                    throw new Exception("Phone é obrigatório.");
                }
                _phone = value;
            }
        }

        public string Fax { get; set; }
    }
}
