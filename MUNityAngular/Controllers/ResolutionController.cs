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
        /// <summary>
        /// Gives an overview of thats possible inside the resolution Controller
        /// </summary>
        /// <returns>a Text that gives a small overview of all posibilities with the Resolution Controller</returns>
        [HttpGet]
        public string Get()
        {
            string description = "";
            description += "To create a new Resolution Call: /Create?auth=[AUTH_CODE]\n";
            description += "To get the Current Version of a Resolution call Get?auth=[AUTH_CODE]&id=[RESOLUTION_ID]\n";
            description += "The default AUTH_CODE is 'default'\n";
            description += "If you want an example call /Example\n";
            description += "Want to add a preamble paragraph: AddPreambleParagraph?auth=[AUTH_CODE]&resolutionid=[RESOLUTION_ID]\n";
            description += "If you want to add an Operative Paragraph use: AddOperativeParagraph?auth=[AUTH_CODE]&resolutionid=[RESOLUTION_ID]";
            description += "Change Operative Paragraph";
            description += "ChangeOperativeParagraph?auth=[AUTH_ID]&resolutionid=[]&paragraphid=[]&newtext=[]\n";
            description += "\n";
            return description;
        }

        /// <summary>
        /// Creates a new Resolution
        /// </summary>
        /// <param name="auth">The authenticator-code </param>
        /// <returns>The new created Resolution in a json format.</returns>
        [Route("[action]")]
        [HttpGet]
        public string Create(string auth)
        {
            return DataHandlers.FileSystem.ResolutionHandler.GetJsonFromResolution(new Models.ResolutionModel());
        }

        /// <summary>
        /// Returns an example of a finished Resolution Document.
        /// </summary>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public string Example()
        {
            return DataHandlers.FileSystem.ResolutionHandler.GetJsonFromResolution(DataHandlers.FileSystem.ResolutionHandler.ExampleResolution);
        }

        [Route("[action]")]
        [HttpGet]
        public string AddPreambleParagraph(string auth, string resolutionid)
        {
            var resolution = DataHandlers.FileSystem.ResolutionHandler.GetResolution(resolutionid);
            if (resolution != null)
            {
                var newPP = resolution.Preamble.AddParagraph();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                return json;
            }
            else
            {
                return "error: Resolution Not Found!";
            }
        }

        /// <summary>
        /// Adds an Premable Paragraph to the Resolution.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="resolutionid"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public string AddOperativeParagraph(string auth, string resolutionid)
        {
            var resolution = DataHandlers.FileSystem.ResolutionHandler.GetResolution(resolutionid);
            if (resolution != null)
            {
                var newPP = resolution.AddOperativeParagraph();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                return json;
            }
            else
            {
                return "error: Resolution Not Found!";
            }
        }

        /// <summary>
        /// Adds an Operative Paragraph to the Resolution!
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="resolutionid"></param>
        /// <param name="paragraphid"></param>
        /// <param name="newtext"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet]
        public string ChangeOperativeParagraph(string auth, string resolutionid, string paragraphid, string newtext)
        {
            var resolution = DataHandlers.FileSystem.ResolutionHandler.GetResolution(resolutionid);
            if (resolution != null)
            {
                var newPP = resolution.OperativeSections.FirstOrDefault(n => n.ID == paragraphid);
                if (newPP != null)
                {
                    newPP.Text = newtext;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(newPP);
                    return json;
                }
                else
                {
                    return "error: Parahraph not found";
                }
            }
            else
            {
                return "error: Resolution Not Found!";
            }
        }

        
        [Route("[action]")]
        [HttpGet]
        public string Get(string auth, string id)
        {
            if (auth.ToLower() == "default")
                if (id == "test")
                    return DataHandlers.FileSystem.ResolutionHandler.GetJsonFromResolution(DataHandlers.FileSystem.ResolutionHandler.ExampleResolution);
                else
                    return "Not implemented yet";
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
