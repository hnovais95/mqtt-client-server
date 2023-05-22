using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Mqtt;
using Common.Models;
using Server.Domain;

namespace Server.Presentation
{
    [Subscribe("sys/client/+/calibrations/predictions/get/+")]
    class CalibrationController
    {
        private readonly ServerNotificationCenter _notificationCenter;
        private readonly ICalibrationService _calibrationService;

        public CalibrationController(ServerNotificationCenter notificationCenter, ICalibrationService calibrationService)
        {
            _notificationCenter = notificationCenter;
            _notificationCenter.OnGetPredictions += NotificationCenter_OnGetPredictions;
            _calibrationService = calibrationService;
        }

        private void NotificationCenter_OnGetPredictions(MqttMessage mqttMessage)
        {
            Task.Run(async () =>
            {
                var result = new RequestResult();

                try
                {
                    var calibrationParams = JsonSerializer.Deserialize<List<CalibrationParamsDTO>>((string)mqttMessage.Payload);
                    var predictions = await _calibrationService.GetPredictions(calibrationParams);
                    result.ResultCode = RequestResultCode.Success;
                    result.Body = predictions;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao tratar requisção. Tópico: {mqttMessage.Topic}; Payload: {mqttMessage.Payload}; Exc.: {e}");
                    result.ResultCode = RequestResultCode.Failure;
                    result.Message = "Erro obter predições de umidade.";
                }
                finally
                {
                    try
                    {
                        _notificationCenter.Publish(ServerCommand.GetCustomersResponse, result, mqttMessage.GetID());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }
    }
}
