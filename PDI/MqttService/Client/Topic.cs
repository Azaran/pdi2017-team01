using System.Collections.Generic;
using System.Linq;

namespace MqttService.Client
{
    public static class Topic
    {
        // MCU related topics:
        private const string MCU_ROOT       = "/vecera.vojta@gmail.com";
        private const string MCU_ANNOUNCE   = "/conn";
        private const string MCU_STATUS     = "/status";
        private const string MCU_TEMP       = "/temp";
        private const string MCU_STATE      = "/state";
        private const string MCU_HSHUT      = "/hshut";
        private const string MCU_RESET      = "/reset";

        // Power strip related topics:
        private const string STRIP_ROOT     = "/strip/sonoff";
        private const string STRIP_CPOWER   = "/cmnd/power";
        private const string STRIP_SPOWER   = "/stat/Power";
        private const string STRIP_ANNOUNCE = "/tele/INFO1";
        private const string STRIP_ENERGY   = "/tele/ENERGY";

        private static string McuTopicFromDeviceId(string devId, string topic)
        {
            return MCU_ROOT + "/" + devId + topic;
        }

        public static string McuAnnounce()
        {
            return MCU_ROOT + MCU_ANNOUNCE;
        }

        public static string McuStatus(string devId)
        {
            return McuTopicFromDeviceId(devId, MCU_STATUS);
        }

        public static string McuTemperature(string devId)
        {
            return McuTopicFromDeviceId(devId, MCU_TEMP);
        }

        public static string McuState(string devId)
        {
            return McuTopicFromDeviceId(devId, MCU_STATE);
        }

        public static string McuHardShutdown(string devId)
        {
            return McuTopicFromDeviceId(devId, MCU_HSHUT);
        }

        public static string McuReset(string devId)
        {
            return McuTopicFromDeviceId(devId, MCU_RESET);
        }

        private static bool IsMcuMessage(string topic, string msg)
        {
            List<string> parts = topic.Split('/').ToList();
            if (parts.Count == 3 && parts[0] == MCU_ROOT && parts[2] == MCU_STATUS)
                return true;
            return false;
        }

        public static bool IsMcuStatus(string topic)
        {
            return IsMcuMessage(topic, MCU_STATUS);
        }

        public static bool IsMcuState(string topic)
        {
            return IsMcuMessage(topic, MCU_STATE);
        }

        public static bool IsMcuTemp(string topic)
        {
            return IsMcuMessage(topic, MCU_TEMP);
        }

        public static string StripAnnounce()
        {
            return STRIP_ROOT + STRIP_ANNOUNCE;
        }

        public static string StripPowerStatus()
        {
            return STRIP_ROOT + STRIP_SPOWER;
        }

        public static string StripEnergy()
        {
            return STRIP_ROOT + STRIP_ENERGY;
        }

        public static string StripPowerCommand()
        {
            return STRIP_ROOT + STRIP_CPOWER;
        }

        public static string DeviceIdFromTopic(string topic)
        {
            List<string> parts = topic.Split('/').ToList();
            if (parts.Count == 3 && parts[0] == MCU_ROOT)
                return parts[1];
            return null;
        }
    }
}
