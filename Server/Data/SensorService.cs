using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Infra.Db;
using Common.Models;
using Server.Domain;

namespace Server.Data
{
    public class SensorService: ISensorService
    {
        private readonly IRepository<RainSensor> _rainRepository;
        private readonly IRepository<HumiditySensor> _humidityRepository;

        public SensorService(IRepository<RainSensor> rainRepository, IRepository<HumiditySensor> humidityRepository)
        {
            _rainRepository = rainRepository;
            _humidityRepository = humidityRepository;
        }

        public async Task<IEnumerable<RainSensor>> GetAllRainMeansurements()
        {
            return await _rainRepository.GetAll();
        }

        public void AddRainMeansurement(RainSensor sensor)
        {
            _rainRepository.Insert(sensor, 5);
        }

        public async Task<IEnumerable<HumiditySensor>> GetAllHumidityMeansurements()
        {
            return await _humidityRepository.GetAll();
        }

        public void AddHumidityMeansurement(HumiditySensor sensor)
        {
            _humidityRepository.Insert(sensor, 5);
        }
    }
}
