using Dapper.FluentMap.Dommel.Mapping;
using Server.Domain;

namespace Server.Infra.Db
{
    public class RainSensorMap: DommelEntityMap<RainSensor>
    {
        public RainSensorMap()
        {
            ToTable("rain");
            Map(x => x.Name).ToColumn("sensor");
            Map(x => x.Timestamp).ToColumn("timestamp");
            Map(x => x.Region).ToColumn("region");
            Map(x => x.Rain).ToColumn("rain");
        }
    }
}