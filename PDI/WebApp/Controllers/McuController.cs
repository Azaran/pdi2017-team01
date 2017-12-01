using MqttService.Persistence.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class McuController : ApiController
    {
        List<MicrocontrollerEntity> mcus = new List<MicrocontrollerEntity>
        {
            new MicrocontrollerEntity { DeviceId = "Tansy2", Powered = true, Temperature = 45.4},
            new MicrocontrollerEntity { DeviceId = "Tansy3", Powered = true, Temperature = 25.4},
            new MicrocontrollerEntity { DeviceId = "MegaTron", Powered = false, Temperature = 45.4},
            new MicrocontrollerEntity { DeviceId = "Tansy4", Powered = true, Temperature = 45.4},
            new MicrocontrollerEntity { DeviceId = "Tansy5", Powered = true, Temperature = 25.4},
            new MicrocontrollerEntity { DeviceId = "MegaTron 2", Powered = false, Temperature = 45.4},
            new MicrocontrollerEntity { DeviceId = "Tansy6", Powered = true, Temperature = 45.4},
            new MicrocontrollerEntity { DeviceId = "Tansy7", Powered = true, Temperature = 25.4},
            new MicrocontrollerEntity { DeviceId = "MegaTron 3", Powered = false, Temperature = 45.4}
        };

        [HttpGet]
        [ActionName("Read")]
        public List<MicrocontrollerEntity> GetAllMcus()
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
