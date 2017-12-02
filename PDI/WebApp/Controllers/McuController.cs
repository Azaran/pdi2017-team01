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
        List<Microcontroller> mcus = new List<Microcontroller>
        {
            new Microcontroller ("Tansy2", true, 45.4),
            new Microcontroller ("Tansy3", false, 21.1),
            new Microcontroller ("Tansy4", true, 44.4),
            new Microcontroller ("Tansy5", true, 41.9),
        };

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
            return Ok(200);
        }

        [HttpPost]
        [ActionName("Poweron")]
        public IHttpActionResult PostPowerOn(string id)
        {
            return Ok(200);
        }

        [HttpPost]
        [ActionName("Kill")]
        public IHttpActionResult PostKill(string id)
        {
            return Ok(200);
        }

        [HttpPost]
        [ActionName("Restart")]
        public IHttpActionResult PostRestart(string id)
        {
            return Ok(200);
        }
    }
}
