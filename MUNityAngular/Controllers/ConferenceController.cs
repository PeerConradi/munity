using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.DataHandlers.Database;
using MUNityAngular.Models;
using MUNityAngular.Services;
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
        public IEnumerable<string> GetNameOfAllConferences([FromServices]ConferenceService service)
        {
            return service.GetNameOfAllConferences();
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<ConferenceModel> GetConferences([FromServices]ConferenceService service)
        {
            return service.GetAllConferences();
        }

        [HttpGet]
        [Route("[action]")]
        public string GetConferencesFormatted([FromServices]ConferenceService service)
        {
            var text = JsonConvert.SerializeObject(service.GetAllConferences(), Newtonsoft.Json.Formatting.Indented);
            return text;
        }



        [Route("[action]")]
        [HttpGet]
        public string Create(string auth, [FromHeader]string Name, [FromHeader]string FullName, 
            [FromHeader]string Abbreviation, [FromHeader]string password, [FromHeader]DateTime StartDate, [FromHeader]DateTime EndDate, [FromServices]ConferenceService service)
        {
            var model = new ConferenceModel();
            model.CreationDate = DateTime.Now;
            model.Name = Name;
            model.FullName = FullName;
            model.Abbreviation = Abbreviation;
            model.StartDate = StartDate;
            model.EndDate = EndDate;
            service.CreateConference(model, password);
            return JsonConvert.SerializeObject(model);
        }
    }
}