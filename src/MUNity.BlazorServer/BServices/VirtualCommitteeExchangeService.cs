using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.VirtualCommittees.Dtos;
using System.Collections.Concurrent;
using MUNity.Extensions.ObservableCollectionExtensions;
using static MUNity.BlazorServer.Components.VirtualCommittee.VCUsersComponent;
using MUNity.Database.Models.Session;

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
                    .Select(n => new CommitteeSessionExchange(this.scopeFactroy)
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

        public event EventHandler BannerChanged;

        public event EventHandler<VotingExchange> VoteNotified;

        public event EventHandler<List<PresentRole>> PresentsChanged;

        public CommitteeSessionExchange CurrentSessionExchange { get; set; }

        //public ObservableCollection<CommitteeSessionExchange> SessionExchanges { get; set; } = new();
        public VirtualSessionBanner Banner { get; private set; }

        public ConcurrentDictionary<int, bool> connectedRoles = new ConcurrentDictionary<int, bool>();

        public void NotifyUserConnected()
        {
            UserConnected?.Invoke(this, EventArgs.Empty);
        }

        public void NotifyUserDisconnected()
        {
            UserDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public void NotifyBannerChanged()
        {
            BannerChanged?.Invoke(this, EventArgs.Empty);
        }

        public void NotifyUsersToVote(VotingExchange exchange)
        {
            VoteNotified?.Invoke(this, exchange);
        }

        public VirtualCommitteeExchange(string committeeId)
        {
            this.CommitteeId = committeeId;
            this.Banner = new VirtualSessionBanner();
        }

        public void SetBanner(VirtualSessionBanner banner)
        {
            this.Banner = banner;
            BannerChanged?.Invoke(this, EventArgs.Empty);
        }

        public void InvokePresentsChanged(SessionPresents presents)
        {
            var list = presents.CheckedUsers.Select(n => new PresentRole()
            {
                IsPresent = n.State == PresentsState.PresentsStates.Present || n.State == PresentsState.PresentsStates.Late,
                RoleId = n.Role.RoleId,
                IsCountry = n.Role.DelegateCountry != null
            }).ToList();
            this.PresentsChanged?.Invoke(this, list);
        }
    }

    public class VirtualSessionBanner
    {
        public bool Active { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool UseTimer { get; set; }

        public TimeOnly Time { get; set; }
    }
}
