using System.Collections.Generic;
using MqttService.Interfaces;
using MqttService.Models;
using MqttService.Persistence.Mapper;
using MqttService.Persistence.Entity;

namespace MqttService.Persistence.Sql
{
    public class PowerStripSqlRepository : IPowerStripRepository
    {
        private readonly PowerStripMapper _powerstripMapper = new PowerStripMapper();
        private readonly MqttServiceDbContext _mqqtSvcDbContext;

        public PowerStripSqlRepository(MqttServiceDbContext mqttSvcDbContext)
        {
            this._mqqtSvcDbContext = mqttSvcDbContext;
        }

        public void Add(PowerStrip powerstrip)
        {
            if (!Contains(powerstrip.DeviceId))
            {
                this._mqqtSvcDbContext.PowerStrips.Add(this._powerstripMapper.Map(powerstrip));
                this._mqqtSvcDbContext.SaveChanges();
            }
        }

        public IEnumerable<PowerStrip> All()
        {
            return this._powerstripMapper.Map(this._mqqtSvcDbContext.PowerStrips);
        }

        public void DeleteAll()
        {
            this._mqqtSvcDbContext.DeleteAll<PowerStripEntity>();
        }

        public void Delete(PowerStrip powerstrip)
        {
            foreach (var pe in this._mqqtSvcDbContext.PowerStrips)
            {
                if (pe.DeviceId == powerstrip.DeviceId)
                {
                    this._mqqtSvcDbContext.PowerStrips.Remove(pe);
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
        /// Updates already saved power strip and adds missing ones.
        /// </summary>
        /// <param name="powerstrips">Collection of PowerStrips</param>
        public void Update(IEnumerable<PowerStrip> powerstrips)
        {
            bool found;
            foreach (var ps in powerstrips)
            {
                found = false;
                foreach (var pe in this._mqqtSvcDbContext.PowerStrips)
                {
                    if (pe.DeviceId == ps.DeviceId)
                    {
                        pe.Powered = ps.Powered;
                        found = true;
                    }
                }

                if (!found)
                {
                    Add(ps);
                }
            }
            this._mqqtSvcDbContext.SaveChanges();
        }

        public bool Contains(string deviceId)
        {
            foreach (var pe in this._mqqtSvcDbContext.PowerStrips)
            {
                if (pe.DeviceId == deviceId)
                    return true;
            }
            return false;
        }

        public int Count()
        {
            int i = 0;
            foreach (var pe in this._mqqtSvcDbContext.PowerStrips)
                ++i;
            return i;
        }
    }
}
