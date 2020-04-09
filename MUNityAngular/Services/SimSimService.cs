using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.SimSim;

namespace MUNityAngular.Services
{

    /// <summary>
    /// A Service to create Simulations of a single committee with some Delegations inside
    /// </summary>
    public class SimSimService
    {
        private List<SimSimModel> _sims;

        public SimSimModel CreateSimSim()
        {
            var sim = new SimSimModel();
            sim.SimSimId = new Random().Next(100000, 999999);
            _sims.Add(sim);
            return sim;
        }

        public SimSimModel GetSimSim(int id)
        {
            return _sims.FirstOrDefault(n => n.SimSimId == id);
        }

        public void RemoveSimSim(SimSimModel sim)
        {
            _sims.Remove(sim);
        }

        public SimSimUser JoinSimulation(SimSimModel simulation, string name, bool leader = false)
        {
            var user = new SimSimUser();
            user.DisplayName = name;
            simulation.AddUserWithToken(user);
            if (leader)
                simulation.AddUserToLeaders(user);

            return user;
        }

        public bool AddUser(SimSimModel simulation, SimSimUser user)
        {
            if (simulation.CanJoin && !simulation.Users.Contains(user))
            {
                simulation.Users.Add(user);
                return true;
            }
            return false;
        }

        public bool RemoveUser(SimSimModel simulation, ISimSimUserFacade user) {
            if (simulation.Users.Contains(user))
            {
                simulation.Users.Remove(user);
                return true;
            }
            else
            {
                user = simulation.Users.FirstOrDefault(n => n.UserToken == user.UserToken);
                if (user != null)
                {
                    simulation.Users.Remove(user);
                    return true;
                }
            }
            return false;
        }

        public void AddChatMessage(SimSimModel simulation, AllChatMessage message)
        {
            simulation.AllChat.Add(message);
        }

        public ISimSimUserFacade GetUser(SimSimModel simulation, string userToken)
        {
            return simulation.Users.FirstOrDefault(n => n.UserToken == userToken);
        }

        public SimSimService()
        {
            _sims = new List<SimSimModel>();
        }

    }
}
