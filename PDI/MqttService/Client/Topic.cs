namespace MqttService.Client
{
    public static class Topic
    {
        private const string ROOT_TOPIC     = "/vecera.vojta@gmail.com";
        private const string ANNOUNCE_TOPIC = "/conn";
        private const string STATUS_TOPIC   = "/status";
        private const string TEMP_TOPIC     = "/temp";
        private const string STATE_TOPIC    = "/state";

        private static string TopicFromId(string devId, string topic)
        {
            return ROOT_TOPIC + "/" + devId + topic;
        }

        public static string DeviceAnnounce()
        {
            return ROOT_TOPIC + ANNOUNCE_TOPIC;
        }

        public static string DeviceStatus(string devId)
        {
            return TopicFromId(devId, STATUS_TOPIC);
        }

        public static string DeviceTemperature(string devId)
        {
            return TopicFromId(devId, TEMP_TOPIC);
        }

        public static string DeviceState(string devId)
        {
            return TopicFromId(devId, STATE_TOPIC);
        }
    }
}
