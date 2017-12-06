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
        static List<PowerStrip> strips = new List<PowerStrip>();
        static MqttService.MqttService srv = new MqttService.MqttService();

        static StripController()
        {
            strips = srv.GetPowerStrips().ToList();
        }

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
            srv.CommandStripPower(0);
            return Ok(200);
        }

        [HttpPost]
        [ActionName("Poweron")]
        public IHttpActionResult PostPowerOn(string id)
        {
            srv.CommandStripPower(1);
            return Ok(200);
        }
    }
}
