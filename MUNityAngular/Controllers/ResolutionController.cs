using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResolutionController : ControllerBase
    {
        // GET: api/Resolution
        [HttpGet]
        public string Get()
        {
            string description = "";
            description += "To create a new Resolution Call: /Create?auth=[AUTH_CODE]\n";
            description += "To get the Current Version of a Resolution call Get?auth=[AUTH_CODE]&id=[RESOLUTION_ID]\n";
            description += "The default AUTH_CODE is 'default'\n";
            description += "If you want an example call /Example\n";
            description += "\n";
            return description;
        }

        [Route("[action]")]
        [HttpGet]
        public string Create(string auth)
        {
            return DataHandlers.FileSystem.ResolutionHandler.GetJsonFromResolution(new Models.ResolutionModel());
        }

        [Route("[action]")]
        [HttpGet]
        public string Example()
        {
            return DataHandlers.FileSystem.ResolutionHandler.GetJsonFromResolution(DataHandlers.FileSystem.ResolutionHandler.ExampleResolution);
        }


        // GET: api/Resolution/5
        [Route("[action]")]
        [HttpGet]
        public string Get(string auth, string id)
        {
            if (auth.ToLower() == "default")
                return "access granted to " + id;
            else
                return "access denied";
        }

        // POST: api/Resolution
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Resolution/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
