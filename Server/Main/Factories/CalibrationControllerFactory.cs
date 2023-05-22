using Server.Data;
using Server.Infra.Network;
using Server.Presentation;

namespace Server.Main.Factories
{
    class CalibrationControllerFactory
	{
        public static CalibrationController Create(ServerNotificationCenter notificationCenter)
        {
            var baseUrl = "https://model-mock.herokuapp.com";
            var httpClient = HttpClientFactory.Create(baseUrl);
            var calibrationService = new CalibrationService(httpClient);
            return new CalibrationController(notificationCenter, calibrationService);
        }
    }
}

