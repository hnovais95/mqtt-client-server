using System;
using System.Windows.Forms;
using Mqtt;

namespace Client
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mqttClient = new MqttClient("127.0.0.1", 1883);
            Client.Configure(mqttClient);
            Client.Start();
        }
    }
}
