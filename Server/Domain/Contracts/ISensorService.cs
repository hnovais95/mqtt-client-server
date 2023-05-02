using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace Server.Domain
{
    public interface ISensorService
    {
        public Task<IEnumerable<RainSensor>> GetAllRainMeansurements();
        public void AddRainMeansurement(RainSensor sensor);

        public Task<IEnumerable<HumiditySensor>> GetAllHumidityMeansurements();
        public void AddHumidityMeansurement(HumiditySensor sensor);
    }
}
