using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.DataHandlers.Database;
using MUNityAngular.Models;
using Newtonsoft.Json;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            string guide = "";
            guide += "To Create a new conference call: Create?auth=[AUTH_CODE]";
            return guide;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<string> GetNameOfAllConferences()
        {
            return ConferenceHandler.GetNameOfAllConferences();
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<ConferenceModel> GetConferences()
        {
            return ConferenceHandler.GetAllConferences();
        }

        [HttpGet]
        [Route("[action]")]
        public string GetConferencesFormatted()
        {
            var text = JsonConvert.SerializeObject(ConferenceHandler.GetAllConferences(), Newtonsoft.Json.Formatting.Indented);
            return text;
        }



        [Route("[action]")]
        [HttpGet]
        public string Create(string auth)
        {
            var model = new ConferenceModel();
            return JsonConvert.SerializeObject(model);
        }
    }
}