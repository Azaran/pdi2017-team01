using MqttService.Logging;
using MQTTnet.Core;

namespace MqttService.Client
{
    public partial class Client
    {
        public async void Publish(string topic, string payload)
        {
            if (!this.IsConnected)
            {
                Logger.Error("Client is not connected");
                return;
            }

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag(false)
                .Build();
            await this._Client.PublishAsync(message);
        }
    }
}
