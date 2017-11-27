namespace MqttService.Interfaces
{
    interface IMicrocontrollerRepository : IDeviceRepository
    {
        void UpdateTemperature(double value);
    }
}
