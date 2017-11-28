using System;
using System.Linq;
using System.Collections.Generic;

using MqttService.Models;
using MqttService.Persistence;
using MqttService.Persistence.Sql;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new MqttService.MqttService();
            var mc = new Microcontroller("aaaa", false, 64.2);
            var ps = new PowerStrip("bbbb");

            Console.WriteLine("MC: {0}, {1}, {2}", mc.DeviceId, mc.Powered, mc.Temperature);
            Console.WriteLine("PS: {0}, {1}", ps.DeviceId, ps.Powered);

            var db = new MqttServiceDbContext("Data Source=./TestDB.sqlite");
            var mcr = new MicrocontrollerSqlRepository(db);
            var psr = new PowerStripSqlRepository(db);

            Console.WriteLine("Trying to load data");
            List<Microcontroller> mcrs = mcr.AllMicrocontrollers().ToList();
            List<PowerStrip> pwss = psr.AllPowerStrips().ToList();

            Console.WriteLine("Microcontrollers: {}", mcrs.Count());
            Console.WriteLine("Power Strips: {}", pwss.Count());
        }
    }
}
