namespace MqttService.Models
{
    public sealed class PowerStrip : Device
    {
        public PowerStrip(string id, bool powered = true)
        : base(id, powered)
        {
        }
    }
}
