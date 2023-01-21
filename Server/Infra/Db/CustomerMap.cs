using Dapper.FluentMap.Dommel.Mapping;
using Server.Domain;

namespace Server.Infra.Db
{
    public class CustomerMap: DommelEntityMap<Customer>
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