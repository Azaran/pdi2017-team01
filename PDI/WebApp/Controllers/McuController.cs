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
            new MicrocontrollerEntity { DeviceId = "Tansy 2", Powered = true, Temperature = 45.4},
            new MicrocontrollerEntity { DeviceId = "Tansy 3", Powered = true, Temperature = 25.4},
            new MicrocontrollerEntity { DeviceId = "MegaTron", Powered = false, Temperature = 45.4}
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
            mcus.Add(new MicrocontrollerEntity { DeviceId = id, Powered = false, Temperature = -273.4 });
            return Ok(200);
        }
    }
}
