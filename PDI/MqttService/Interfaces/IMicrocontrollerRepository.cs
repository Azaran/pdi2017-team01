using System;
using System.Collections.Generic;
using MqttService.Models;

namespace MqttService.Interfaces
{
    public interface IMicrocontrollerRepository : IDisposable
    {
        void Add(Microcontroller microcontroller);
        IEnumerable<Microcontroller> All();
        void Update(IEnumerable<Microcontroller> microcontrollers);
        void Delete(Microcontroller microcontroller);
        void DeleteAll();
        bool Contains(string deviceId);
    }
}
