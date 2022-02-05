using MUNity.Database.Models.Session;
using System.Collections.Concurrent;

namespace MUNity.BlazorServer.BServices
{
    public class VirtualCommitteeExchangeService
    {
        private ILogger<VirtualCommiteeParticipationService> _logger;

        private List<VirtualCommitteeExchange> _exchanges;

        public void NotifyPetitionAdded(Petition petition)
        {

        }

        public VirtualCommitteeExchange GetExchange(string committeeId)
        {
            var exchange = _exchanges.FirstOrDefault(n => n.CommitteeId == committeeId);
            if (exchange == null)
            {
                _logger.LogInformation($"New exchange for committee: {committeeId} created");
                exchange = new VirtualCommitteeExchange(committeeId);
                this._exchanges.Add(exchange);
            }
            return exchange;
        }

        public VirtualCommitteeExchangeService(ILogger<VirtualCommiteeParticipationService> logger)
        {
            this._logger = logger;
            this._exchanges = new List<VirtualCommitteeExchange>();
            
        }
    }

    public class VirtualCommitteeExchange
    {
        public string CommitteeId { get; private set; }

        public event EventHandler UserConnected;

        public event EventHandler UserDisconnected;

        public event EventHandler<Petition> PetitionCreated;

        public ConcurrentDictionary<int, bool> connectedRoles = new ConcurrentDictionary<int, bool>();

        public void NotifyUserConnected()
        {
            UserConnected?.Invoke(this, EventArgs.Empty);
        }

        public void NotifyUserDisconnected()
        {
            UserDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public VirtualCommitteeExchange(string committeeId)
        {
            this.CommitteeId = committeeId;
        }
    }
}
