using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MqttService.Models;
using MqttService.Persistence;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttService.Client
{
    public class Client
    {
        private MqttClient _Client { get; set; }
        public List<string> SubscribedTopics { get; private set; }
        public bool IsConnected { get; private set; }
        private SqlRepository Repository { get; set; }

        public Client(string brokerUrl)
        {
            this.SubscribedTopics = new List<string>();
            this.Repository = new SqlRepository();
            this._Client = new MqttClient(brokerUrl);

            this._Client.MqttMsgPublished       += OnMessagePublished;
            this._Client.MqttMsgSubscribed      += OnTopicSubscribed;
            this._Client.MqttMsgPublishReceived += OnMessageReceived;
            this._Client.MqttMsgUnsubscribed    += OnTopicUnsubscribed;

            Connect();
            SubscribeToSavedDevices();
        }

        private void SubscribeToSavedDevices()
        {
            foreach (var m in this.Repository.Microcontrollers.All())
                SubscribeToDevice(DeviceType.Microcontroller, m.DeviceId);
            foreach (var p in this.Repository.PowerStrips.All())
                SubscribeToDevice(DeviceType.PowerStrip, p.DeviceId);
        }

        public void Subscribe(string topic)
        {
            if (!IsSubscribed(topic))
            {
                this.SubscribedTopics.Add(topic);
                this._Client.Subscribe(
                    new string[] { topic },
                    new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }
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

        public void Unsubscribe(string topic)
        {
            if(IsSubscribed(topic))
            {
                this.SubscribedTopics.Remove(topic);
                this._Client.Unsubscribe(new string[] { topic });
            }
        }

        public void Unsubscribe(string[] topics)
        {
            foreach (string t in topics)
                Unsubscribe(t);
        }

        public void Publish(string topic, string message)
        {
            this._Client.Publish(
                topic, Encoding.UTF8.GetBytes(message),
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false
            );
        }

        public void Disconnect()
        {
            if(this.IsConnected)
            {
                this._Client.Disconnect();
                this.IsConnected = false;
            }
        }

        public void Connect()
        {
            if(!this.IsConnected)
            {
                this._Client.Connect(Guid.NewGuid().ToString() + "-PDI-xklima22");
                this.IsConnected = this._Client.IsConnected;
            }
        }

        private void SubscribeToDevice(DeviceType type, string devId)
        {
            List<string> topics = new List<string>
            {
                Topic.DeviceState(devId),
                Topic.DeviceStatus(devId)
            };
            if (type == DeviceType.Microcontroller)
                topics.Add(Topic.DeviceTemperature(devId));
            Subscribe(topics.ToArray());
        }

        private void OnTopicUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Console.WriteLine("Topic Unsubscribed: MessageId: {0}", e.MessageId);
        }

        private void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Message Received: Received: '{0}', Topic: '{1}'",
                Encoding.UTF8.GetString(e.Message), e.Topic
            );
            if(e.Topic == Topic.DeviceAnnounce())
            {
                string device      = Encoding.UTF8.GetString(e.Message);
                List<string> parts = device.Split('/').ToList();
                string devType     = parts[0];
                string devId       = parts[1];
                if(parts.Count == 2)
                {
                    if(devType == "mcu")
                    {
                        Console.WriteLine("Adding MCU '{0}'", devId);
                        if (!Repository.Microcontrollers.Contains(devId) &&
                        !Repository.PowerStrips.Contains(devId))
                        {
                            this.Repository.Microcontrollers.Add(new Microcontroller(devId));
                            SubscribeToDevice(DeviceType.Microcontroller, devId);
                        }
                        else
                        {
                            Console.WriteLine("MCU '{0}' is already in the database", devId);
                        }
                    }
                    else if(devType == "strip")
                    {
                        Console.WriteLine("Adding power strip '{0}'", devId);
                        if (!Repository.Microcontrollers.Contains(devId) &&
                        !Repository.PowerStrips.Contains(devId))
                        {
                            this.Repository.PowerStrips.Add(new PowerStrip(devId));
                            SubscribeToDevice(DeviceType.PowerStrip, devId);
                        }
                        else
                        {
                            Console.WriteLine("Power strip '{0}' is already in the database", devId);
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Invalid device type '{0}'", devId);
                    }
                }
                else
                {
                    Console.Error.WriteLine("Invalid device identification '{0}'", device);
                }
            }
        }

        private void OnTopicSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Topic Subscribed: MessageId: {0}", e.MessageId);
        }

        private void OnMessagePublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("Message Published: MessageId: {0}, IsPublished: {1}",
                e.MessageId, e.IsPublished
            );
        }
    }
}
