using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using MqttService.Models;
using MqttService.Logging;
using MQTTnet.Core;
using MQTTnet.Core.Client;
using Newtonsoft.Json.Linq;

namespace MqttService.Client
{
    public partial class Client
    {
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

        private void SubscribeToSavedMcus()
        {
            if (!this.IsConnected)
            {
                Logger.Error("Client is not connected");
                return;
            }

            foreach (var m in this.Repository.Microcontrollers.All())
                SubscribeToMcu(m.DeviceId);
        }

        private void SubscribeToMcu(string devId)
        {
            if (!this.IsConnected)
            {
                Logger.Error("Client is not connected");
                return;
            }

            List<string> topics = new List<string>
            {
                Topic.McuState(devId),
                Topic.McuStatus(devId),
                Topic.McuTemperature(devId)
            };
            Subscribe(topics.ToArray());
        }

        private void SubscribeToPowerStrip()
        {
            List<string> topics = new List<string>
            {
                Topic.StripAnnounce(),
                Topic.StripEnergy(),
                Topic.StripPower()
            };
            Subscribe(topics.ToArray());
        }

        private void OnMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            string topic = e.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Logger.Info("Received message '{0}' in topic: '{1}'", payload, topic);
            if (topic == Topic.McuAnnounce())
            {
                ProcessMcuAnnounce(payload);
            }
            else if(topic == Topic.StripAnnounce())
            {
                ProcessStripAnnounce(payload);
            }
            else
            {
                if (Topic.IsMcuStatus(topic))
                {
                    Console.WriteLine("TODO: IsMcuStatus");
                }
                else if (Topic.IsMcuState(topic))
                {
                    Console.WriteLine("TODO: IsMcuState");
                }
            }
        }

        /// <summary>
        /// Parses payload which is expected to be JSON with at least keys Module and Version.
        /// Does nothing if JSON parsing faile or keys are missing.
        /// Only one power strip may be in DB at the same time.
        /// </summary>
        /// <param name="payload"></param>
        private void ProcessStripAnnounce(string payload)
        {
            if(Repository.PowerStrips.Count() >= 1)
            {
                Logger.Info("Some power strip is already in database");
                return;
            }

            JObject json = null;
            try
            {
                json = JObject.Parse(payload);
            }
            catch
            {
                Logger.Error("Power strip payload - Failed to parse JSON: '{0}'", payload);
                return;
            }
            var module  = json["Module"];
            var version = json["Version"];
            if (module == null || version == null)
            {
                Logger.Error("Invalid power strip announce payload: '{0}'", payload);
                return;
            }
            var name = module.ToString() + " " + version.ToString();
            Logger.Info("Adding power strip '{0}' into the database", name);
            this.Repository.PowerStrips.Add(new PowerStrip(name));
        }

        private void ProcessMcuAnnounce(string payload)
        {
            List<string> parts = payload.Split('/').ToList();
            string devType     = parts[0];
            string devId       = parts[1];
            if (parts.Count == 2)
            {
                if (devType == "mcu")
                {
                    Logger.Info("Trying to add MCU '{0}'", devId);
                    if (!Repository.Microcontrollers.Contains(devId) &&
                    !Repository.PowerStrips.Contains(devId))
                    {
                        this.Repository.Microcontrollers.Add(new Microcontroller(devId));
                        SubscribeToMcu(devId);
                    }
                    else
                    {
                        Logger.Warn("Device '{0}' is already in the database", devId);
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
