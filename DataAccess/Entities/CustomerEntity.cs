using Dapper.FluentMap.Dommel.Mapping;

namespace DataAccess.Entities
{
    public class CustomerEntity
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }

    public class CustomerMap: DommelEntityMap<CustomerEntity>
    {
        public CustomerMap()
        {
            ToTable("customers");
            Map(x => x.CustomerID).ToColumn("customer_id").IsKey();
            Map(x => x.CompanyName).ToColumn("company_name");
            Map(x => x.ContactName).ToColumn("contact_name");
            Map(x => x.ContactTitle).ToColumn("contact_title");
            Map(x => x.Address).ToColumn("address");
            Map(x => x.City).ToColumn("city");
            Map(x => x.Region).ToColumn("region");
            Map(x => x.PostalCode).ToColumn("postal_code");
            Map(x => x.Country).ToColumn("country");
            Map(x => x.Phone).ToColumn("phone");
            Map(x => x.Fax).ToColumn("fax");
        }
    }
}