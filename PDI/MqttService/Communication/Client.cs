extern alias Mqtt;

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Mqtt::uPLibrary.Networking.M2Mqtt;
using Mqtt::uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttService.Communication
{
    public class Client
    {
        private MqttClient _Client { get; set; }
        public List<string> SubscribedTopics { get; private set; }
        public bool IsConnected { get; private set; }

        public Client(string brokerUrl)
        {
            this.SubscribedTopics = new List<string>();
            this._Client = new MqttClient(brokerUrl);
            this._Client.MqttMsgPublished += OnMessagePublished;
            this._Client.MqttMsgSubscribed += OnTopicSubscribed;
            this._Client.MqttMsgPublishReceived += OnMessageReceived;
            this._Client.MqttMsgUnsubscribed += OnTopicUnsubscribed;
            this._Client.Connect(Guid.NewGuid().ToString());
            this.IsConnected = this._Client.IsConnected;
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

        private void OnTopicUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Debug.WriteLine("Topic Unsubscribed: MessageId: {0}", e.MessageId);
        }

        private void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.WriteLine("Message Received: Received: '{0}', Topic: '{1}'",
                Encoding.UTF8.GetString(e.Message), e.Topic
            );
        }

        private void OnTopicSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Debug.WriteLine("Topic Subscribed: MessageId: {0}", e.MessageId);
        }

        private void OnMessagePublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Debug.WriteLine("Message Published: MessageId: {0}, IsPublished: {1}",
                e.MessageId, e.IsPublished
            );
        }
    }
}
