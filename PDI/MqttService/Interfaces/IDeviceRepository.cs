using System;

namespace MqttService.Interfaces
{
    public interface IDeviceRepository : IDisposable
    {
        void UpdatePowered(bool value);
    }
}
