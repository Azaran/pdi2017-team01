using System;
using System.Collections.Generic;
using MqttService.Models;

namespace MqttService.Interfaces
{
    public interface IPowerStripRepository : IDisposable
    {
        void AddPowerStrip(PowerStrip powerstrip);
        IEnumerable<PowerStrip> AllPowerStrips();
        void Update(IEnumerable<PowerStrip> powerstrips);
        void DeletePowerStrip(PowerStrip powerstrip);
        void DeleteAll();
    }
}
