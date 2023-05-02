using Server.Infra.Db;
using Server.Data;
using Server.Presentation;

namespace Server.Main
{
    class SensorControllerFactory
    {
        public static SensorController CreateController(NotificationCenter notificationCenter)
        {
            var rainRepository = new RainRepository();
            var humidityRepository = new HumidityRepository();
            var sensorService = new SensorService(rainRepository, humidityRepository);
            return new SensorController(notificationCenter, sensorService);
        }
    }
}
