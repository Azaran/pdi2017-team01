using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MqttService.Models;

namespace WebApp.Controllers
{
    public class StripController : ApiController
    {
        List<PowerStrip> strips = new List<PowerStrip>
        {
            new PowerStrip ("Tansy2", true),
            new PowerStrip ("Tansy4", false)
        };

        [HttpGet]
        [ActionName("Read")]
        public List<PowerStrip> GetAllStrips()
        {
            return strips;
        }

        [HttpGet]
        [ActionName("Read")]
        public IHttpActionResult GetStrip(string id)
        {
            var strip = strips.FirstOrDefault((p) => p.DeviceId == id);
            if (strip == null)
            {
                return NotFound();
            }
            return Ok(strip);
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
    }
}
