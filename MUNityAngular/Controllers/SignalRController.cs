using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        IHubContext<Hubs.TestHub, Hubs.ITypedTestHub> _hub;
        public SignalRController(IHubContext<Hubs.TestHub, Hubs.ITypedTestHub> hub)
        {
            _hub = hub;
            
        }

        [HttpGet]
        [Route("[action]")]
        public StatusCodeResult PushMessage()
        {
            _ = _hub.Clients.All.sendToAll("Anon", "Inhalt");
            return new StatusCodeResult(200);
        }

        [HttpGet]
        [Route("[action]")]
        public StatusCodeResult PushGroup()
        {
            _ = _hub.Clients.Group("test").sendToAll("Gruppe", "Nachricht");
            return new StatusCodeResult(200);
        }
    }
}