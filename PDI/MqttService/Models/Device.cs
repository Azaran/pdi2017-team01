namespace MqttService.Models
{
    public abstract class Device
    {
        public string ID { get; set; }
        public bool Powered { get; set; }

        public Device(string id, bool powered)
        {
            this.ID      = id;
            this.Powered = powered;
        }
    }
}
