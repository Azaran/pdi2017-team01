using System;
using System.Collections.Generic;
using MqttService.Persistence;
using MqttService.Logging;
using MQTTnet;
using MQTTnet.Core.Client;

namespace MqttService.Client
{
    public partial class Client
    {
        private IMqttClient   _Client { get; set; }
        public List<string>   SubscribedTopics { get; private set; }
        public bool           IsConnected { get; private set; }
        private SqlRepository Repository { get; set; }
        public string         BrokerAddress { get; private set; }

        public Client(string brokerUrl)
        {
            this.IsConnected      = false;
            this.BrokerAddress    = brokerUrl;
            this.SubscribedTopics = new List<string>();
            this.Repository       = new SqlRepository();
            this._Client          = new MqttFactory().CreateMqttClient();

            this._Client.ApplicationMessageReceived += OnMessageReceived;

            Connect();
            Subscribe(Topic.McuAnnounce());
            SubscribeToPowerStrip();
            SubscribeToSavedMcus();
        }

        public void Disconnect()
        {
            if(this.IsConnected)
            {
                this._Client.DisconnectAsync().RunSynchronously();
                this.IsConnected = false;
            }
        }

        public void Connect()
        {
            Logger.Info("Trying secured connection to '{0}' ...", this.BrokerAddress);
            if(!this.IsConnected)
            {
                var options = new MqttClientOptionsBuilder()
                    .WithWebSocketServer(this.BrokerAddress)
                    .WithClientId(Guid.NewGuid().ToString() + "-PDI-team01");

                try
                {
                    var t = this._Client.ConnectAsync(options.WithTls().Build());
                    t.Wait();
                }
                catch(Exception)
                {
                    Logger.Error("Failed to connect to {0} securely", this.BrokerAddress);
                    Logger.Info("Trying unsecured connection ...");
                    try
                    {
                        var t = this._Client.ConnectAsync(options.Build());
                        t.Wait();
                    }
                    catch(Exception)
                    {
                        Logger.Error("Failed to connect to '{0}'", this.BrokerAddress);
                    }
                }
                this.IsConnected = this._Client.IsConnected;
                Logger.Info(this.IsConnected ? "Connected" : "Not connected");
            }
        }
    }
}
