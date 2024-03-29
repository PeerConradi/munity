﻿using MUNity.Database.Context;

namespace MUNity.BlazorServer.BServices
{
    public class VotingExchangeService
    {
        private readonly IList<VotingExchange> _exchanges = new List<VotingExchange>();

        private readonly IServiceScopeFactory _scopeFactory;

        public VotingExchange GetExchange(string votingId, string title, string text)
        {
            var exchange = _exchanges.FirstOrDefault(n => n.VotingId == votingId);
            if (exchange == null)
            {
                //var scope = _scopeFactory.CreateScope();
                //var dbContext = scope.ServiceProvider.GetRequiredService<MunityContext>();

                exchange = new VotingExchange()
                {
                    VotingId = votingId,
                    Title = title,
                    Text = text
                };
                _exchanges.Add(exchange);
            }
            else
            {
                exchange.Title = title;
                exchange.Text = text;
            }
            return exchange;
        }

        public VotingExchangeService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
    }

    public class VotingExchange
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string VotingId { get; set; }

        public event EventHandler<UserVotedEventArgs> UserVoted;

        public void NotifyUserVoted(int roleId, MUNity.Base.EVoteStates choice)
        {
            UserVoted?.Invoke(this, new UserVotedEventArgs(roleId, choice));
        }

    }

    public class UserVotedEventArgs : EventArgs
    {
        public int RoleId { get; private set; }

        public Base.EVoteStates Choice { get; private set; }

        public UserVotedEventArgs(int roleId, Base.EVoteStates choice)
        {
            this.RoleId = roleId;
            this.Choice = choice;
        }
    }
}
