using Dapper.FluentMap.Dommel.Mapping;
using Server.Domain;

namespace Server.Infra.Db
{
    public class HumiditySensorMap : DommelEntityMap<HumiditySensor>
    {
        public HumiditySensorMap()
        {
            ToTable("humidity");
            Map(x => x.Name).ToColumn("sensor");
            Map(x => x.Timestamp).ToColumn("timestamp");
            Map(x => x.Region).ToColumn("region");
            Map(x => x.Humidity).ToColumn("humidity");
        }
    }
}