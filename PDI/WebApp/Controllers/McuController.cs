using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MqttService.Models;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class McuController : ApiController
    {
        private static MqttService.MqttService srv =  new MqttService.MqttService();

        private async Task<MqttService.MqttService> GetMqttService()
        {
            if (!srv.GetClientConnected())
                await srv.ConnectClient();
            
            return srv;
        }

        [HttpGet]
        [ActionName("Read")]
        public async Task<List<Microcontroller>> GetAllMcus()
        {
            MqttService.MqttService lService = await GetMqttService();
            //return new List<Microcontroller> { new Microcontroller("Tansy2") };
            return lService.GetMicrocontrollers().ToList();   // DB problem?
        }

        [HttpGet]
        [ActionName("Read")]
        public IHttpActionResult GetMcu(string id)
        {
            var mcus = srv.GetMicrocontrollers().ToList();
            var mcu = mcus.FirstOrDefault((p) => p.DeviceId == id);
            if (mcu == null)
            {
                return NotFound();
            }
            return Ok(mcu);
        }

        [HttpPost]
        [ActionName("Poweroff")]
        public IHttpActionResult PostPowerOff(string id)
        {
            srv.CommandMcuPower(id, 0);
            return Ok(200);
        }

        [HttpPost]
        [ActionName("Poweron")]
        public IHttpActionResult PostPowerOn(string id)
        {
            srv.CommandMcuPower(id, 1);
            return Ok(200);
        }

        [HttpPost]
        [ActionName("Kill")]
        public IHttpActionResult PostKill(string id)
        {
            srv.CommandMcuHardShutdown(id);
            return Ok(200);
        }

        [HttpPost]
        [ActionName("Restart")]
        public IHttpActionResult PostRestart(string id)
        {
            srv.CommandMcuReset(id);
            return Ok(200);
        }
    }
}
