using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MqttService.Models;

namespace WebApp.Controllers
{
    public class McuController : ApiController
    {
        static List<Microcontroller> mcus = new List<Microcontroller>();
        static MqttService.MqttService srv = new MqttService.MqttService();

        static McuController()
        {
            mcus = srv.GetMicrocontrollers().ToList();
        }
        
        [HttpGet]
        [ActionName("Read")]
        public List<Microcontroller> GetAllMcus()
        {
            return mcus;
        }

        [HttpGet]
        [ActionName("Read")]
        public IHttpActionResult GetMcu(string id)
        {
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
