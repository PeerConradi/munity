using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Models.SimSim
{
    public class SimSimModel : ISimSimFacade
    {
        public int SimSimId { get; set; }

        public string Name { get; set; }

        public List<ISimSimUserFacade> Users { get; set; }

        private Dictionary<string, ISimSimUserFacade> UserTokens { get; set; }

        public IReadOnlyCollection<ISimSimUserFacade> Leaders
        {
            get
            {
                return Users.Where(n => _leaderIds.Contains(n.UserToken)).ToList().AsReadOnly();
            }
        }

        private List<string> _leaderIds { get; set; }

        public List<AllChatMessage> AllChat { get; set; }

        public Queue<SimSimRequestModel> Requests { get; set; }

        public bool AllowAnyDelegation { get; set; }

        public bool AllowJoin { get; set; }

        public List<Delegation> AllowedDelegations { get; set; }

        public string Password { get; set; }
        public bool UsingPassword { get => string.IsNullOrEmpty(Password); }
        public bool CanJoin { get; set; }

        public void AddUserToLeaders(SimSimUser user)
        {
            if (!_leaderIds.Any(n => n == user.UserToken))
                _leaderIds.Add(user.UserToken);
        }

        public ISimSimUserFacade GetUserByToken(string token)
        {
            if (!UserTokens.ContainsKey(token))
                return null;
            return UserTokens[token];
        }

        public void AddUserWithToken(SimSimUser user)
        {
            Users.Add(user);
            UserTokens.Add(user.HiddenToken, user);
        }
    }
}
