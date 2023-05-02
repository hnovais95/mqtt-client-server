using System;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Models;
using Mqtt;
using Server.Domain;

namespace Server.Presentation
{
    [
        Subscribe("sys/sensor/+/rain/get/+"),
        Subscribe("sys/sensor/+/rain/add/+"),
        Subscribe("sys/sensor/+/humidity/get/+"),
        Subscribe("sys/sensor/+/humidity/add/+")
    ]
    class SensorController
    {
        private readonly NotificationCenter _notificationCenter;
        private readonly ISensorService _sensorService;

        public SensorController(NotificationCenter notificationCenter, ISensorService customerService)
        {
            _notificationCenter = notificationCenter;
            _notificationCenter.OnGetRainMeansurements += NotificationCenter_OnGetRainMeansurements;
            _notificationCenter.OnAddRainMeansurement += NotificationCenter_OnAddRainMeansurement;
            _notificationCenter.OnGetHumidityMeansurements += NotificationCenter_OnGetHumidityMeansurements;
            _notificationCenter.OnAddHumidityMeansurement += NotificationCenter_OnAddHumidityMeansurement;
            _sensorService = customerService;
        }

        private void NotificationCenter_OnGetRainMeansurements(MqttMessage mqttMessage)
        {
            Task.Run(async () =>
            {
                var result = new RequestResult();

                try
                {
                    var rainMeansurements = await _sensorService.GetAllRainMeansurements();
                    result.ResultCode = RequestResultCode.Success;
                    result.Body = rainMeansurements;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao tratar requisção. Tópico: {mqttMessage.Topic}; Payload: {mqttMessage.Payload}; Exc.: {e}");
                    result.ResultCode = RequestResultCode.Failure;
                    result.Message = "Erro recuperar medições de chuva.";
                }
                finally
                {
                    try
                    {
                        _notificationCenter.Publish(ServerCommand.GetRainMeansurementResponse, result, mqttMessage.GetID());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }

        private void NotificationCenter_OnAddRainMeansurement(MqttMessage mqttMessage)
        {
            Task.Run(() =>
            {
                try
                {
                    var rainSensor = JsonSerializer.Deserialize<RainSensor>((string)mqttMessage.Payload);
                    _sensorService.AddRainMeansurement(rainSensor);
                    Console.WriteLine($"Medição de chuva adicionada com sucesso. ID do Sensor: {rainSensor.Name}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao adicionar medição de chuva. Tópico: {mqttMessage.Topic}; Payload: {mqttMessage.Payload}; Exc.: {e}");
                }
            });
        }

        private void NotificationCenter_OnGetHumidityMeansurements(MqttMessage mqttMessage)
        {
            Task.Run(async () =>
            {
                var result = new RequestResult();

                try
                {
                    var humidityMeansurements = await _sensorService.GetAllHumidityMeansurements();
                    result.ResultCode = RequestResultCode.Success;
                    result.Body = humidityMeansurements;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao tratar requisção. Tópico: {mqttMessage.Topic}; Payload: {mqttMessage.Payload}; Exc.: {e}");
                    result.ResultCode = RequestResultCode.Failure;
                    result.Message = "Erro recuperar medições de humidade.";
                }
                finally
                {
                    try
                    {
                        _notificationCenter.Publish(ServerCommand.GetRainMeansurementResponse, result, mqttMessage.GetID());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }

        private void NotificationCenter_OnAddHumidityMeansurement(MqttMessage mqttMessage)
        {
            Task.Run(() =>
            {
                try
                {
                    var humiditySensor = JsonSerializer.Deserialize<HumiditySensor>((string)mqttMessage.Payload);
                    _sensorService.AddHumidityMeansurement(humiditySensor);
                    Console.WriteLine($"Medição de humidade adicionada com sucesso. ID do Sensor: {humiditySensor.Name}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao adicionar medição de humidade. Tópico: {mqttMessage.Topic}; Payload: {mqttMessage.Payload}; Exc.: {e}");
                }
            });
        }
    }
}
