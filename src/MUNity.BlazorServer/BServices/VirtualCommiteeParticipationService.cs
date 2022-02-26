using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Session;
using MUNity.VirtualCommittees.Dtos;

namespace MUNity.BlazorServer.BServices
{

    /// <summary>
    /// This service handles the current state of the user that is inside a virtual committee.
    /// Every user can only be inside one virtual committee.
    /// </summary>
    public class VirtualCommiteeParticipationService : IDisposable
    {
        private ILogger<VirtualCommiteeParticipationService> _logger;
        private VirtualCommitteeExchangeService _committeeExchangeService;
        private MunityContext _dbContext;

        private VirtualCommitteeExchange _exchange;
        // Virtual Committee ViewModel

        private string _committeeId;
        private string _secret;
        private int? _roleId;

        public int? RoleId => _roleId;

        private string _roleName;
        public string RoleName => _roleName;

        private string _roleIso;
        public string RoleIso => _roleIso;

        /// <summary>
        /// Callback event when the user is done with SignIn into the conference
        /// </summary>
        public event EventHandler Registered;

        public event EventHandler<string> EditingPreambleParagraphChanged;
        public event EventHandler<string> EditingOperativeParagraphChanged;

        private string lastEditedPreambleParagraphId = null;
        public string LastEditedPreambleParagraphId 
        {
            get => lastEditedPreambleParagraphId;
            set
            {
                if (lastEditedPreambleParagraphId != value)
                {
                    lastEditedPreambleParagraphId = value;
                    EditingPreambleParagraphChanged?.Invoke(this, value);
                }
            }
        }

        private string lastEditedOperativeParagraphId = null;
        public string LastEditedOperativeParagraphId
        {
            get => lastEditedOperativeParagraphId;
            set
            {
                if (lastEditedOperativeParagraphId != value)
                {
                    lastEditedOperativeParagraphId = value;
                    EditingOperativeParagraphChanged?.Invoke(this, value);
                }
            }
        }

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
            if (_exchange == null)
                return;

            if (_roleId != null && _exchange.connectedRoles.TryRemove(_roleId.Value, out var t) == true)
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
                var ex = _exchange.SessionExchanges.FirstOrDefault(n => n.CurrentAgendaItemId == agendaItemId);
                ex.AddPetition(petition);
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
                if (_exchange != null)
                {
                    var petitionDto = _exchange.SessionExchanges.SelectMany(n => n.Petitions).FirstOrDefault(n => n.PetitionId == petitionId);
                    if (petitionDto != null)
                    {
                        petitionDto.Status = Base.EPetitionStates.Active;
                    }
                    else
                    {
                        _logger?.LogWarning($"Unable to find a dto for petition {petitionId}|{petition.PetitionId}");
                    }
                    var ex = _exchange.SessionExchanges.FirstOrDefault(n => n.Petitions.Any(a => a.PetitionId == petitionId));
                    ex.NotifyPetitionStatusChanged();
                }
                else
                {
                    _logger?.LogWarning($"No exchange for the committee '{_committeeId}' of RoleId '{_roleId}' was found. View might not be in sync");
                }
            }
        }

        public void RemovePetition(string petitionId)
        {
            var petition = _dbContext.Petitions.Include(n => n.AgendaItem).FirstOrDefault(n => n.PetitionId == petitionId);
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
                var ex = _exchange.SessionExchanges.FirstOrDefault(n => n.CurrentAgendaItemId == petition.AgendaItem.AgendaItemId);
                ex?.Petitions.Remove(ex.Petitions.FirstOrDefault(n => n.PetitionId == petitionId));
            }
        }


        public bool SignIn(string committeeId, string secret)
        {
            SignOff();
            var roleInfo = this._dbContext.Delegates
                .Where(n => n.RoleSecret == secret)
                .AsNoTracking()
                .Select(n => new
                {
                    RoleId = n.RoleId,
                    RoleName = n.RoleName,
                    Iso = n.DelegateCountry.Iso
                })
                .FirstOrDefault();

            if (roleInfo == null)
                return false;

            this._committeeId = committeeId;
            this._secret = secret;
            this._roleId = roleInfo?.RoleId;
            this._roleName = roleInfo?.RoleName;
            this._roleIso = roleInfo?.Iso ?? "un";
            this._exchange = _committeeExchangeService.GetExchange(committeeId);
            if (_exchange != null && this._roleId != null)
            {
                _exchange.connectedRoles.TryAdd(this._roleId.Value, true);
                _exchange.NotifyUserConnected();
            }
            this.Registered?.Invoke(this, EventArgs.Empty);
            return true;
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
