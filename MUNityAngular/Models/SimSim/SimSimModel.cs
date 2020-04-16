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

        private Dictionary<string, ISimSimUserFacade> _userTokens { get; set; }

        public IReadOnlyCollection<ISimSimUserFacade> Leaders
        {
            get
            {
                return Users.Where(n => _leaderIds.Contains(n.UserToken)).ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// A list of User Tokens that are Leaders of the Lobby. The public Token
        /// not the Hidden Token
        /// </summary>
        private List<string> _leaderIds { get; set; }

        public List<AllChatMessage> AllChat { get; set; }

        public Queue<SimSimRequestModel> Requests { get; set; }

        internal void RemoveUser(ISimSimUserFacade user)
        {
            _leaderIds.Remove(user.UserToken);
            Users.Remove(user);
            var signalRkey = SignalRConnections.FirstOrDefault(n => n.Value == user).Key ?? null;
            if (signalRkey != null)
                SignalRConnections.Remove(signalRkey);

            var tokenKey = _userTokens.FirstOrDefault(n => n.Value == user).Key;
            if (tokenKey != null)
                _userTokens.Remove(tokenKey);
        }

        public bool AllowAnyDelegation { get; set; }

        public List<Delegation> AllowedDelegations { get; set; }

        public string Password { get; set; }
        public bool UsingPassword { get => !string.IsNullOrEmpty(Password); }
        public bool CanJoin { get; set; } = true;

        public Dictionary<string, ISimSimUserFacade> SignalRConnections = new Dictionary<string, ISimSimUserFacade>();

        public void AddUserToLeaders(SimSimUser user)
        {
            if (!_leaderIds.Any(n => n == user.UserToken))
                _leaderIds.Add(user.UserToken);
        }

        public ISimSimUserFacade GetUserByToken(string token)
        {
            if (!_userTokens.ContainsKey(token))
                return null;
            return _userTokens[token];
        }

        public void AddUserWithToken(SimSimUser user)
        {
            Users.Add(user);
            _userTokens.Add(user.HiddenToken, user);
        }

        public bool HiddenTokenValid(string token)
        {
            return _userTokens.ContainsKey(token);
        }

        public ISimSimUserFacade GetUserByHiddenToken(string token)
        {
            if (_userTokens.ContainsKey(token))
                return _userTokens[token];
            return null;
        }

        public SimSimModel()
        {
            Users = new List<ISimSimUserFacade>();
            _userTokens = new Dictionary<string, ISimSimUserFacade>();
            _leaderIds = new List<string>();
            Requests = new Queue<SimSimRequestModel>();
            AllChat = new List<AllChatMessage>();
            AllowedDelegations = new List<Delegation>();
        }
    }
}
