﻿using System;
using System.Collections.Generic;
using MqttService.Persistence;
using MqttService.Logging;
using MQTTnet;
using MQTTnet.Core.Client;

namespace MqttService.Client
{
    public partial class Client
    {
        private string _brokerAddress;

        private IMqttClient   _Client { get; set; }
        public List<string>   SubscribedTopics { get; private set; }
        private SqlRepository _Repository { get; set; }

        public string BrokerAddress
        {
            get { return _brokerAddress; }
            set
            {
                if(value != _brokerAddress)
                {
                    _brokerAddress = value;
                    Connect();
                }
            }
        }

        public bool IsConnected
        {
            get
            {
                if (this._Client == null)
                    return false;
                return this._Client.IsConnected;
            }
        }

        public Client(string brokerAddress)
        {
            this.SubscribedTopics = new List<string>();
            this._Repository      = new SqlRepository();
            this._Client          = new MqttFactory().CreateMqttClient();
            this._Client.ApplicationMessageReceived += OnMessageReceived;
            this.BrokerAddress = brokerAddress; // This also calls Connect()
            Subscribe(Topic.McuAnnounce());
            SubscribeToPowerStrip();
            SubscribeToSavedMcus();
        }

        public void Disconnect()
        {
            if (this.IsConnected)
            {
                var t = this._Client.DisconnectAsync();
                t.Wait();
            }
        }

        public void Connect()
        {
            if(IsConnected)
                Disconnect();

            if(!(this.BrokerAddress.StartsWith("ws://") ||
            this.BrokerAddress.StartsWith("wss://")))
            {
                Logger.Error("Invalid protocol in '{0}'", this.BrokerAddress);
                return;
            }

            string useTls = null;
            var options = new MqttClientOptionsBuilder()
                .WithWebSocketServer(this.BrokerAddress)
                .WithClientId(Guid.NewGuid().ToString() + "-PDI-team01");
            if(this.BrokerAddress.StartsWith("wss://"))
            {
                options = options.WithTls();
                useTls = "with";
            }
            else
                useTls = "without";

            Logger.Info("Trying to connect to '{0}' {1} TLS", this.BrokerAddress, useTls);

            try
            {
                var t = this._Client.ConnectAsync(options.Build());
                t.Wait();
            }
            catch(Exception)
            {
                Logger.Error("Failed to connect to {0} {1} TLS", this.BrokerAddress, useTls);
            }
            Logger.Info(this.IsConnected ? "Connected" : "Not connected");
        }
    }
}
