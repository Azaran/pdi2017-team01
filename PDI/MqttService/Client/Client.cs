using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MqttService.Models;
using MqttService.Persistence;
using MqttService.Logging;
using MQTTnet;
using MQTTnet.Core;
using MQTTnet.Core.Client;

namespace MqttService.Client
{
    public class Client
    {
        private IMqttClient _Client { get; set; }
        public List<string> SubscribedTopics { get; private set; }
        public bool IsConnected { get; private set; }
        private SqlRepository Repository { get; set; }
        public string BrokerAddress { get; private set; }

        public Client(string brokerUrl)
        {
            this.IsConnected      = false;
            this.BrokerAddress    = brokerUrl;
            this.SubscribedTopics = new List<string>();
            this.Repository       = new SqlRepository();
            this._Client          = new MqttFactory().CreateMqttClient();

            this._Client.ApplicationMessageReceived += OnMessageReceived;

            Connect();
            SubscribeToSavedDevices();
        }

        private void SubscribeToSavedDevices()
        {
            if (!this.IsConnected)
            {
                Logger.Error("Client is not connected");
                return;
            }

            foreach (var m in this.Repository.Microcontrollers.All())
                SubscribeToDevice(DeviceType.Microcontroller, m.DeviceId);
            foreach (var p in this.Repository.PowerStrips.All())
                SubscribeToDevice(DeviceType.PowerStrip, p.DeviceId);
        }

        public async void Subscribe(string topic)
        {
            if (!this.IsConnected)
            {
                Logger.Error("Client is not connected");
                return;
            }

            Logger.Info("Subscribing to '{0}'", topic);
            if (!IsSubscribed(topic))
            {
                this.SubscribedTopics.Add(topic);
                await this._Client.SubscribeAsync(
                    new TopicFilterBuilder().WithTopic(topic).WithExactlyOnceQoS().Build()
                );
            }
        }

        public void Subscribe(string[] topics)
        {
            foreach (string t in topics)
                Subscribe(t);
        }

        public bool IsSubscribed(string topic)
        {
            return this.SubscribedTopics.Contains(topic);
        }

        public async void Unsubscribe(string topic)
        {
            if (!this.IsConnected)
            {
                Logger.Error("Client is not connected");
                return;
            }

            if (IsSubscribed(topic))
            {
                this.SubscribedTopics.Remove(topic);
                await this._Client.UnsubscribeAsync(topic);
            }
        }

        public void Unsubscribe(string[] topics)
        {
            foreach (string t in topics)
                Unsubscribe(t);
        }

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

        public void Disconnect()
        {
            if(this.IsConnected)
            {
                this._Client.DisconnectAsync().Wait();
                this.IsConnected = false;
            }
        }

        public void Connect()
        {
            Logger.Info("Trying secured connection to '{0}' ...", this.BrokerAddress);
            if(!this.IsConnected)
            {
                var options = new MqttClientOptionsBuilder()
                    .WithWebSocketServer(this.BrokerAddress);
                    //.WithClientId(Guid.NewGuid().ToString() + "-PDI-xklima22");

                try
                {
                    this._Client.ConnectAsync(options.WithTls().Build()).Wait();
                }
                catch(Exception)
                {
                    Logger.Error("Secured connection to {0} failed", this.BrokerAddress);
                    Logger.Info("Trying unsecured connection ...");
                    try
                    {
                        this._Client.ConnectAsync(options.Build()).Wait();
                    }
                    catch(Exception)
                    {
                        Logger.Error("Unsecured connection to {0} failed", this.BrokerAddress);
                    }
                }
                this.IsConnected = this._Client.IsConnected;
                Logger.Info("Connected: {0}", this.IsConnected);
            }
        }

        private void SubscribeToDevice(DeviceType type, string devId)
        {
            if (!this.IsConnected)
            {
                Logger.Error("Client is not connected");
                return;
            }

            List<string> topics = new List<string>
            {
                Topic.DeviceState(devId),
                Topic.DeviceStatus(devId)
            };
            if (type == DeviceType.Microcontroller)
                topics.Add(Topic.DeviceTemperature(devId));
            Subscribe(topics.ToArray());
        }

        private void OnMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            string topic   = e.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Logger.Info("Received message '{0}' in topic: '{1}'", payload, topic);
            if(topic == Topic.DeviceAnnounce())
            {
                List<string> parts = payload.Split('/').ToList();
                string devType     = parts[0];
                string devId       = parts[1];
                if(parts.Count == 2)
                {
                    if(devType == "mcu")
                    {
                        Logger.Info("Trying to add MCU '{0}'", devId);
                        if (!Repository.Microcontrollers.Contains(devId) &&
                        !Repository.PowerStrips.Contains(devId))
                        {
                            this.Repository.Microcontrollers.Add(new Microcontroller(devId));
                            SubscribeToDevice(DeviceType.Microcontroller, devId);
                        }
                        else
                        {
                            Logger.Warn("MCU '{0}' is already in the database", devId);
                        }
                    }
                    else if(devType == "strip")
                    {
                        Logger.Info("Trying to add power strip '{0}'", devId);
                        if (!Repository.Microcontrollers.Contains(devId) &&
                        !Repository.PowerStrips.Contains(devId))
                        {
                            this.Repository.PowerStrips.Add(new PowerStrip(devId));
                            SubscribeToDevice(DeviceType.PowerStrip, devId);
                        }
                        else
                        {
                            Logger.Warn("Power strip '{0}' is already in the database", devId);
                        }
                    }
                    else
                    {
                        Logger.Error("Invalid device type '{0}'", devId);
                    }
                }
                else
                {
                    Logger.Error("Invalid device identification '{0}'");
                }
            }
        }
    }
}
