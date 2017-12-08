using System;
using System.Collections.Generic;
using MqttService.Interfaces;
using MqttService.Models;
using System.Threading.Tasks;

namespace MqttService
{
    public class MqttService : IMqttService
    {
        private Client.Client client;
        private Persistence.SqlRepository repository;

        public MqttService()
        {
            client = new Client.Client("wss://mqtt-xklima22.azurewebsites.net:443/mqtt");
            repository = new Persistence.SqlRepository();
        }

        public void CommandMcuHardShutdown(string deviceId)
        {
            client.PublishMcuHardShutdownCommand(deviceId);
        }

        public void CommandMcuPower(string deviceId, int value)
        {
            client.PublishMcuPowerCommand(deviceId, value);
        }

        public void CommandMcuReset(string deviceId)
        {
            client.PublishMcuResetCommand(deviceId);
        }

        public void CommandStripPower(string deviceId, int value)
        {
            client.PublishStripPowerCommand(deviceId, value);
        }

        public void Dispose()
        {
            repository?.Dispose();
            client.Disconnect();
        }

        public IEnumerable<Microcontroller> GetMicrocontrollers()
        {
            return repository.Microcontrollers.All();
        }

        public IEnumerable<PowerStrip> GetPowerStrips()
        {
            return repository.PowerStrips.All();
        }

        public bool GetClientConnected()
        {
            return this.client.IsConnected;
        }

        public async Task ConnectClient()
        {
            await this.client.Connect();
        }
    }
}
