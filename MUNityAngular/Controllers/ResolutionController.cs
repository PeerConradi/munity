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
            var json = DataHandlers.FileSystem.ResolutionHandler.GetJsonFromResolution(
                DataHandlers.FileSystem.ResolutionHandler.ExampleResolution);
            return json;
        }

        // GET: api/Resolution/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
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
