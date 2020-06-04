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
using MUNityAngular.Util.Extenstions;
using MUNityAngular.Schema.Request;
using MUNityAngular.DataHandlers.EntityFramework.Models;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Controllers
{

    /// <summary>
    /// The Conference Controller needs a complete rework!
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : ControllerBase
    {
        
    }
}