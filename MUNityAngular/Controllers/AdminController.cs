using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Models.User;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.DataHandlers.EntityFramework.Models;
using MUNityAngular.Models.Conference;
using User = MUNityAngular.Models.Core.User;

namespace MUNityAngular.Controllers
{

    /// <summary>
    /// The AdminController handles every request were Admin-Auth is requiered.
    /// This controller allows the user to change any type of conference, manage Users and documents
    /// aswell as turning the gloabal settings on or off.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {


    }
}