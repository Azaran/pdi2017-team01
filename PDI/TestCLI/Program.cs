using System;
using MqttService.Models;

namespace TestCLI
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
        }
    }
}
