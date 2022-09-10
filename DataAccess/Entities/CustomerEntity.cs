using Dapper.Contrib.Extensions;

namespace DataAccess.Entities
{
    [Table("customers")]
    public class CustomerEntity
    {
        [ExplicitKey]
        [Column(Name = "customer_id")]
        public string CustomerID { get; set; }

        [Column(Name = "company_name")]
        public string CompanyName { get; set; }

        [Column(Name = "contact_name")]
        public string ContactName { get; set; }

        [Column(Name = "contact_title")]
        public string ContactTitle { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "city")]
        public string City { get; set; }

        [Column(Name = "region")]
        public string Region { get; set; }

        [Column(Name = "postal_code")]
        public string PostalCode { get; set; }

        [Column(Name = "country")]
        public string Country { get; set; }

        [Column(Name = "phone")]
        public string Phone { get; set; }

        [Column(Name = "fax")]
        public string Fax { get; set; }
    }
}
