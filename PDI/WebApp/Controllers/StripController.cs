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
    public class StripController : ApiController
    {
        private static MqttService.MqttService srv;

        private async Task<MqttService.MqttService> GetMqttService()
        {
            if (!srv.GetClientConnected())
                await srv.ConnectClient();

            return srv;
        }

        [HttpGet]
        [ActionName("Read")]
        public async Task<List<PowerStrip>> GetAllStrips()
        {
            MqttService.MqttService lService = await GetMqttService();
            return lService.GetPowerStrips().ToList();
        }

        [HttpGet]
        [ActionName("Read")]
        public IHttpActionResult GetStrip(string id)
        {
            var strips = srv.GetPowerStrips().ToList();
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
