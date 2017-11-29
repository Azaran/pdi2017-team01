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
        MicrocontrollerEntity[] mcus = new MicrocontrollerEntity[]
        {
            new MicrocontrollerEntity { DeviceId = "Tansy 2", Powered = true, Temperature = 45.4},
            new MicrocontrollerEntity { DeviceId = "Tansy 3", Powered = true, Temperature = 25.4},
            new MicrocontrollerEntity { DeviceId = "MegaTron", Powered = false, Temperature = 45.4}
        };

        public IEnumerable<MicrocontrollerEntity> GetAllMcus()
        {
            return mcus;
        }

        public IHttpActionResult GetMcu(string id)
        {
            var mcu = mcus.FirstOrDefault((p) => p.DeviceId == id);
            if (mcu == null)
            {
                return NotFound();
            }
            return Ok(mcu);
        }
    }
}
