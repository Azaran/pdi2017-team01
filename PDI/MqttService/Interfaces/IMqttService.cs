using System;
using System.ServiceModel;
using System.Collections.Generic;

namespace MqttService.Interfaces
{
    [ServiceContract]
    public interface IMqttService : IDisposable
    {
        [OperationContract]
        void PublishTopic(string topic);

        [OperationContract]
        void AddSubscribedTopic(string topic);

        [OperationContract]
        IEnumerable<string> SubscribedTopics();
    }
}
