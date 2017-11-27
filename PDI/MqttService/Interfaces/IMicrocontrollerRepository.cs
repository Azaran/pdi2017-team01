using System;
using System.Collections.Generic;
using MqttService.Models;

namespace MqttService.Interfaces
{
    public interface IMicrocontrollerRepository : IDisposable
    {
        void AddMicrocontroller(Microcontroller microcontroller);
        IEnumerable<Microcontroller> AllMicrocontrollers();
        void Update(IEnumerable<Microcontroller> microcontrollers);
        void DeleteMicrocontroller(Microcontroller microcontroller);
        void DeleteAll();
    }
}
