using System.Collections.Generic;
using MqttService.Interfaces;
using MqttService.Models;
using MqttService.Persistence.Mapper;
using MqttService.Persistence.Entity;

namespace MqttService.Persistence.Sql
{
    public class MicrocontrollerSqlRepository : IMicrocontrollerRepository
    {
        private readonly MicrocontrollerMapper _microcontrollerMapper = new MicrocontrollerMapper();
        private readonly MqttServiceDbContext _mqqtSvcDbContext;

        public MicrocontrollerSqlRepository(MqttServiceDbContext mqttSvcDbContext)
        {
            this._mqqtSvcDbContext = mqttSvcDbContext;
        }

        public void Add(Microcontroller microcontroller)
        {
            if (!Contains(microcontroller.DeviceId))
            {
                this._mqqtSvcDbContext.Microcontrollers.Add(this._microcontrollerMapper.Map(microcontroller));
                this._mqqtSvcDbContext.SaveChanges();
            }
        }

        public IEnumerable<Microcontroller> All()
        {
            return this._microcontrollerMapper.Map(this._mqqtSvcDbContext.Microcontrollers);
        }

        public void DeleteAll()
        {
            this._mqqtSvcDbContext.DeleteAll<MicrocontrollerEntity>();
        }

        public void Delete(Microcontroller microcontroller)
        {
            foreach (MicrocontrollerEntity me in this._mqqtSvcDbContext.Microcontrollers)
            {
                if(me.DeviceId == microcontroller.DeviceId)
                {
                    this._mqqtSvcDbContext.Microcontrollers.Remove(me);
                    break;
                }
            }
            this._mqqtSvcDbContext.SaveChanges();
        }

        public void Dispose()
        {
            this._mqqtSvcDbContext?.Dispose();
        }

        /// <summary>
        /// Updates already saved microcontrollers and adds missing ones.
        /// </summary>
        /// <param name="microcontrollers">Collection of Microcontrollers</param>
        public void Update(IEnumerable<Microcontroller> microcontrollers)
        {
            bool found;
            foreach(Microcontroller mc in microcontrollers)
            {
                found = false;
                foreach(MicrocontrollerEntity me in this._mqqtSvcDbContext.Microcontrollers)
                {
                    if(me.DeviceId == mc.DeviceId)
                    {
                        me.Temperature = mc.Temperature;
                        me.Powered = mc.Powered;
                        found = true;
                    }
                }

                if(!found)
                {
                    Add(mc);
                }
            }
            this._mqqtSvcDbContext.SaveChanges();
        }

        public bool Contains(string deviceId)
        {
            foreach(MicrocontrollerEntity me in this._mqqtSvcDbContext.Microcontrollers)
            {
                if (me.DeviceId == deviceId)
                    return true;
            }
            return false;
        }
    }
}
