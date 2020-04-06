using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUNityAngular.Models.SimSim;
using MUNityAngular.Schema.Request.Simulation;
using MUNityAngular.Services;
using MUNityAngular.Util.Extenstions;

namespace MUNityAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimSimController : ControllerBase
    {
        [Route("[action]")]
        [HttpGet]
        public ActionResult<ISimSimFacade> CreateSimSim([FromServices]SimSimService service)
        {
            return StatusCode(StatusCodes.Status200OK, service.CreateSimSim() as ISimSimFacade);
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<ISimSimFacade> GetSimSim([FromHeader]string id,
            [FromServices]SimSimService service)
        {
            var simulation = service.GetSimSim(id.ToIntOrDefault(-1));
            if (simulation == null)
                return StatusCode(StatusCodes.Status404NotFound, "Simulation not found");
            return StatusCode(StatusCodes.Status200OK, simulation as ISimSimFacade);
        }

        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        public ActionResult<SimSimUser> TryJoin([FromHeader]string id, [FromHeader]string name, [FromServices]SimSimService service)
        {
            var simulation = service.GetSimSim(id.ToIntOrDefault(-1));
            if (simulation == null)
                return new NotFoundResult();

            if (simulation.CanJoin == false)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to join, the lobby is closed!");

            if (string.IsNullOrWhiteSpace(name))
            {
                return new BadRequestResult();
            }

            var user = service.JoinSimulation(simulation, name, simulation.Users.Count == 0);
            return user;
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult AddChatMessage([FromBody]ISimulationChatMessageRequest body, [FromServices]SimSimService service)
        {
            var simulation = service.GetSimSim(body.SimulationId.ToIntOrDefault(-1));
            if (simulation == null)
                return StatusCode(StatusCodes.Status404NotFound, "Simulation not found!");
            var actualUser = simulation.GetUserByToken(body.UserToken);
            if (actualUser == null)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not a valid User in this simulation!");

            var chatMessage = new AllChatMessage(actualUser, body.Text);
            service.AddChatMessage(simulation, chatMessage);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("[action]")]
        [HttpGet]
        public ActionResult<List<AllChatMessage>> GetAllChat([FromHeader]string simulationid, [FromHeader]string token, [FromServices]SimSimService service)
        {
            var simulation = service.GetSimSim(simulationid.ToIntOrDefault());
            if (simulation == null)
                return StatusCode(StatusCodes.Status404NotFound, "Simulation not found!");

            var user = simulation.GetUserByToken(token);
            if (user == null)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not allowed to do that!");

            return simulation.AllChat;
        }
    }
}