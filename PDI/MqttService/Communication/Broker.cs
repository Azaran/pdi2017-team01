extern alias Gnat;

using Gnat::uPLibrary.Networking.M2Mqtt;

namespace MqttService.Communication
{
    public class Broker
    {
        private MqttBroker _Broker { get; set; }
        public bool Running { get; private set; }

        public Broker()
        {
            this._Broker = new MqttBroker();
            this.Running = false;
        }

        public void Start()
        {
            if (!this.Running)
            {
                this._Broker.Start();
                this.Running = true;
            }
        }

        public void Stop()
        {
            if(this.Running)
            {
                this._Broker.Stop();
                this.Running = false;
            }
        }
    }
}
