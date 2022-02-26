using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Session;
using MUNity.VirtualCommittees.Dtos;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using MUNity.Extensions.ObservableCollectionExtensions;

namespace MUNity.BlazorServer.BServices
{
    public class VirtualCommitteeExchangeService
    {
        private ILogger<VirtualCommiteeParticipationService> _logger;

        private IServiceScopeFactory scopeFactroy;

        private List<VirtualCommitteeExchange> _exchanges;

        public VirtualCommitteeExchange GetExchange(string committeeId)
        {
            var exchange = _exchanges.FirstOrDefault(n => n.CommitteeId == committeeId);
            if (exchange == null)
            {

                exchange = new VirtualCommitteeExchange(committeeId);

                ReloadSessionExchangesForExchange(exchange);
                this._exchanges.Add(exchange);

                _logger.LogInformation($"New exchange for committee: {committeeId} created.");

            }
            return exchange;
        }

        public void ReloadSessionExchangesForExchange(VirtualCommitteeExchange exchange)
        {
            using var scope = scopeFactroy.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<MunityContext>();
            var session = context.CommitteeSessions.AsNoTracking().Where(n => n.Committee.CommitteeId == exchange.CommitteeId)
                    .Select(n => new CommitteeSessionExchange()
                    {
                        SessionId = n.CommitteeSessionId,
                        CurrentAgendaItemName = n.CurrentAgendaItem.Name,
                        CurrentAgendaItemId = n.CurrentAgendaItem.AgendaItemId,
                        SessionName = n.Name,
                        Petitions = n.CurrentAgendaItem.Petitions
                        .Where(n => n.Status != Base.EPetitionStates.Resolved && n.Status != Base.EPetitionStates.Done && n.Status != Base.EPetitionStates.Removed)
                        .Select(p =>
                            new PetitionDto()
                            {
                                PetitionDate = p.PetitionDate,
                                PetitionTypeName = p.PetitionType.Name,
                                PetitionCategoryName = p.PetitionType.Category,
                                PetitionUserIso = p.User.DelegateCountry.Iso,
                                PetitionUserName = p.User.RoleName,
                                PetitionId = p.PetitionId,
                                PetitionTypeId = p.PetitionType.PetitionTypeId,
                                PetitionTypeSortOrder = p.PetitionType.SortOrder,
                                PetitionUserId = p.User.RoleId,
                                Status = p.Status,
                                TargetAgendaItemId = p.AgendaItem.AgendaItemId,
                                Text = p.Text
                            }
                        ).OrderBy(n => n.PetitionTypeSortOrder).ThenBy(n => n.PetitionDate).ToObservableCollection()
                    }).FirstOrDefault();
            exchange.CurrentSessionExchange = session;
            this._logger.LogInformation($"Loaded Virutal committee exchange and added a session.");
        }

        public VirtualCommitteeExchangeService(ILogger<VirtualCommiteeParticipationService> logger, IServiceScopeFactory scopeFactory)
        {
            this._logger = logger;
            this._exchanges = new List<VirtualCommitteeExchange>();
            this.scopeFactroy = scopeFactory;
        }
    }

    public class VirtualCommitteeExchange
    {
        public string CommitteeId { get; private set; }

        public event EventHandler UserConnected;

        public event EventHandler UserDisconnected;

        public event EventHandler CurrentSessionChanged;

        public CommitteeSessionExchange CurrentSessionExchange { get; set; }

        //public ObservableCollection<CommitteeSessionExchange> SessionExchanges { get; set; } = new();


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

    public class CommitteeSessionExchange
    {
        public string SessionId { get; set; }

        public string SessionName { get; set; }

        public string CurrentAgendaItemName { get; set; }

        public int CurrentAgendaItemId { get; set; }

        public event EventHandler PetitionStatusChanged;

        public event EventHandler PresentsCheckedChanged;

        public ObservableCollection<PetitionDto> Petitions { get; set; } = new();

        public void AddPetition(Petition newPetition)
        {
            var newIndex = 0;
            foreach (var existingPetition in Petitions)
            {
                if (newPetition.PetitionType.SortOrder < existingPetition.PetitionTypeSortOrder)
                    break;

                if (newPetition.PetitionType.SortOrder == existingPetition.PetitionTypeSortOrder && newPetition.PetitionDate > existingPetition.PetitionDate)
                {
                    break;
                }
                newIndex++;
            }

            var dto = new PetitionDto()
            {
                PetitionTypeName = newPetition.PetitionType.Name,
                PetitionCategoryName = newPetition.PetitionType.Category,
                PetitionUserIso = newPetition.User.DelegateCountry.Iso,
                PetitionUserName = newPetition.User.RoleName,
                PetitionDate = newPetition.PetitionDate,
                PetitionId = newPetition.PetitionId,
                PetitionTypeId = newPetition.PetitionType.PetitionTypeId,
                PetitionUserId = newPetition.User.RoleId,
                Status = newPetition.Status,
                TargetAgendaItemId = newPetition.AgendaItem.AgendaItemId,
                Text = newPetition.Text,
                PetitionTypeSortOrder = newPetition.PetitionType.SortOrder
            };

            
            this.Petitions.Insert(newIndex, dto);
            //PetitionAdded?.Invoke(this, petition);
        }
    
        public void NotifyPetitionStatusChanged()
        {
            this.PetitionStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
