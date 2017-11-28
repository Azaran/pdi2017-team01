using System;
using System.Collections.Generic;
using MqttService.Models;

namespace MqttService.Interfaces
{
    public interface IPowerStripRepository : IDisposable
    {
        void Add(PowerStrip powerstrip);
        IEnumerable<PowerStrip> All();
        void Update(IEnumerable<PowerStrip> powerstrips);
        void Delete(PowerStrip powerstrip);
        void DeleteAll();
    }
}
