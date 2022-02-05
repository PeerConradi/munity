using MUNity.Database.Context;
using MUNity.Database.Models.Session;

namespace MUNity.BlazorServer.BServices
{
    public class VirtualCommiteeParticipationService : IDisposable
    {
        private ILogger<VirtualCommiteeParticipationService> _logger;
        private VirtualCommitteeExchangeService _committeeExchangeService;
        private MunityContext _dbContext;

        private VirtualCommitteeExchange _exchange;
        // Virtual Committee ViewModel

        private string _committeeId;
        private string _secret;
        private int _roleId;

        public int RoleId => _roleId;

        public bool IsActiveForCommittee(string committeeId)
        {
            return _committeeId == committeeId;
        }

        // Role
        public void Dispose()
        {
            SignOff();
        }

        private void SignOff()
        {
            if (_exchange?.connectedRoles.TryRemove(_roleId, out var t) == true)
            {
                _exchange.NotifyUserDisconnected();
            }
        }

        public void MakePetition(int agendaItemId, int patitionType)
        {
            var agendaItem = _dbContext.AgendaItems.FirstOrDefault(n => n.AgendaItemId == agendaItemId);
            var petitionType = _dbContext.PetitionTypes.FirstOrDefault(n => n.PetitionTypeId == patitionType);
            var user = _dbContext.Delegates.FirstOrDefault(n => n.RoleId == _roleId);
            if (agendaItem == null)
            {
                _logger?.LogWarning("No Agenda Item with the id '{0}' found. Unable to create a new petition", agendaItemId);
                return;
            }

            if (petitionType == null)
            {
                _logger?.LogWarning($"No PetitionType with id '{patitionType}' found. Unable to create a new petition");
                return;
            }

            if (user == null)
            {
                _logger?.LogWarning($"No user/role with the id {_roleId} was found. Unable to create a new Petition");
                return;
            }

            var petition = new Petition()
            {
                AgendaItem = agendaItem,
                PetitionDate = DateTime.Now,
                PetitionType = petitionType,
                Status = Base.EPetitionStates.Queued,
                Text = "",
                User = user,
            };
            _dbContext.Petitions.Add(petition);
            _dbContext.SaveChanges();
            if (_exchange != null)
            {
                _exchange.NotifyPetitionAdded(petition);

            }
            else
            {
                _logger?.LogWarning($"No exchange for the committee '{_committeeId}' of RoleId '{_roleId}' was found. View might not be in sync");
            }
        }

        public void ActivatePetition(string petitionId)
        {
            var petition = _dbContext.Petitions.FirstOrDefault(n => n.PetitionId == petitionId);
            if (petition == null)
            {
                _logger?.LogWarning("No petition with the Id '{0}' found to activate.", petitionId);
            }
            else
            {
                petition.Status = Base.EPetitionStates.Active;
                this._dbContext.SaveChanges();
                this._exchange?.NotifyPetitionUpdated(petition);
            }
        }

        public void RemovePetition(string petitionId)
        {
            var petition = _dbContext.Petitions.FirstOrDefault(n => n.PetitionId == petitionId);
            if (petition == null)
            {
                _logger?.LogWarning("No petition with the Id '{0}' found to remove.", petitionId);
            }
            else
            {
                if (petition.Status == Base.EPetitionStates.Active)
                    petition.Status = Base.EPetitionStates.Resolved;
                else
                    petition.Status = Base.EPetitionStates.Removed;

                this._dbContext.SaveChanges();
                this._exchange?.NotifyPetitionRemoved(petition);
            }
        }


        public void SignIn(string committeeId, string secret, int myRoleId)
        {
            SignOff();

            this._committeeId = committeeId;
            this._secret = secret;
            this._roleId = myRoleId;
            this._exchange = _committeeExchangeService.GetExchange(committeeId);
            if (_exchange != null)
            {
                _exchange.connectedRoles.TryAdd(myRoleId, true);
                _exchange.NotifyUserConnected();
            }
        }

        public VirtualCommiteeParticipationService(ILogger<VirtualCommiteeParticipationService> logger, 
            VirtualCommitteeExchangeService exchangeService,
            MunityContext dbContext)
        {
            this._logger = logger;
            this._committeeExchangeService = exchangeService;
            this._dbContext = dbContext;
        }
    }
}
