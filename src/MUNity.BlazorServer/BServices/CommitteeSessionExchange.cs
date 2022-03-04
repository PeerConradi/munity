using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Session;
using MUNity.VirtualCommittees.Dtos;
using System.Collections.ObjectModel;

namespace MUNity.BlazorServer.BServices
{
    public class CommitteeSessionExchange
    {
        public IServiceScopeFactory scopeFactory;

        public string SessionId { get; set; }

        public string SessionName { get; set; }

        public string CurrentAgendaItemName { get; set; }

        public int CurrentAgendaItemId { get; set; }

        public event EventHandler PetitionStatusChanged;

        public event EventHandler PresentsCheckedChanged;

        public event EventHandler CurrentAgendaItemChanged;

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
                PetitionTypeName = newPetition.PetitionType?.Name ?? "unkown",
                PetitionCategoryName = newPetition.PetitionType?.Category ?? "unkown",
                PetitionUserIso = newPetition.User.DelegateCountry?.Iso ?? "un",
                PetitionUserName = newPetition.User?.RoleName ?? "unkown",
                PetitionDate = newPetition.PetitionDate,
                PetitionId = newPetition.PetitionId,
                PetitionTypeId = newPetition.PetitionType?.PetitionTypeId ?? -1,
                PetitionUserId = newPetition.User?.RoleId ?? -1,
                Status = newPetition.Status,
                TargetAgendaItemId = newPetition.AgendaItem?.AgendaItemId ?? -1,
                Text = newPetition.Text,
                PetitionTypeSortOrder = newPetition.PetitionType?.SortOrder ?? 0
            };

            
            this.Petitions.Insert(newIndex, dto);
            //PetitionAdded?.Invoke(this, petition);
        }

        public void MakePetition(int patitionType, int roleId)
        {
            using var scope = scopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<MunityContext>();
            var logger = scope.ServiceProvider.GetService<ILogger<VirtualCommitteeExchange>>();
            var agendaItem = dbContext.AgendaItems.FirstOrDefault(n => n.AgendaItemId == this.CurrentAgendaItemId);
            var petitionType = dbContext.PetitionTypes.FirstOrDefault(n => n.PetitionTypeId == patitionType);
            var user = dbContext.Delegates.Include(n => n.DelegateCountry).FirstOrDefault(n => n.RoleId == roleId);
            if (agendaItem == null)
            {
                logger?.LogWarning("No Agenda Item with the id '{0}' found. Unable to create a new petition", this.CurrentAgendaItemId);
                return;
            }

            if (petitionType == null)
            {
                logger?.LogWarning($"No PetitionType with id '{patitionType}' found. Unable to create a new petition");
                return;
            }

            if (user == null)
            {
                logger?.LogWarning($"No user/role with the id {roleId} was found. Unable to create a new Petition");
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
            dbContext.Petitions.Add(petition);
            dbContext.SaveChanges();
            this.AddPetition(petition);
        }

        public void ActivatePetition(string petitionId)
        {
            using var scope = scopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<MunityContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CommitteeSessionExchange>>();
            var petition = dbContext.Petitions.FirstOrDefault(n => n.PetitionId == petitionId);
            if (petition == null)
            {
                logger?.LogWarning("No petition with the Id '{0}' found to activate.", petitionId);
            }
            else
            {
                petition.Status = Base.EPetitionStates.Active;
                dbContext.SaveChanges();

                var petitionDto = this.Petitions.FirstOrDefault(n => n.PetitionId == petitionId);
                if (petitionDto != null)
                {
                    petitionDto.Status = Base.EPetitionStates.Active;
                    this.PetitionStatusChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    logger?.LogWarning($"Unable to find a dto for petition {petitionId}|{petition.PetitionId}");
                }
                    
            }
        }


        public void RemovePetition(string petitionId)
        {
            using var scope = scopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<MunityContext>();
            var logger = scope.ServiceProvider.GetService<ILogger<CommitteeSessionExchange>>();
            var petition = dbContext.Petitions.FirstOrDefault(n => n.PetitionId == petitionId);
            if (petition == null)
            {
                logger?.LogWarning("No petition with the Id '{0}' found to remove.", petitionId);
            }
            else
            {
                if (petition.Status == Base.EPetitionStates.Active)
                    petition.Status = Base.EPetitionStates.Resolved;
                else
                    petition.Status = Base.EPetitionStates.Removed;

                dbContext.SaveChanges();

                this.Petitions.Remove(this.Petitions.FirstOrDefault(n => n.PetitionId == petitionId));
            }
        }
        public void NotifyPetitionStatusChanged()
        {
            this.PetitionStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        public void NotifyCurrentAgendaItemNameChanged()
        {
            CurrentAgendaItemChanged?.Invoke(this, EventArgs.Empty);
        }

        public CommitteeSessionExchange(IServiceScopeFactory factory)
        {
            this.scopeFactory = factory;
        }
    }
}
