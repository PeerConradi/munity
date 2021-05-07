using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using MUNity.Models.Simulation;
using MUNity.Schema.Simulation;
using MUNity.Schema.Simulation.Voting;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Dtos.Simulations;
using MUNityCore.Extensions.CastExtensions;
using MUNityCore.Models.Simulation;
using MUNityCore.Models.Simulation.Presets;
using MUNity.Schema.Simulation.Resolution;

namespace MUNityCore.Services
{

    /// <summary>
    /// A service for virtual committees.
    /// The service should be handled as a singleton, because the virtual committee is created and destroied within
    /// the Memory.
    /// </summary>
    public class SimulationService
    {

        private readonly MunityContext _context;

        public IEnumerable<Models.Simulation.Presets.ISimulationPreset> Presets
        {
            get
            {
                yield return new Models.Simulation.Presets.GV_Preset();
                yield return new Models.Simulation.Presets.HA3_Preset();
                yield return new Models.Simulation.Presets.KFK_Preset();
                yield return new Models.Simulation.Presets.KWT_Preset();
                yield return new Models.Simulation.Presets.MRR_Preset();
                yield return new Models.Simulation.Presets.SR_Preset();
                yield return new Models.Simulation.Presets.WiSo_Preset();
                yield return new Models.Simulation.Presets.BW_TVT_EGO();
                yield return new Models.Simulation.Presets.BW_TVT_Sim_Preset();
            }
        }

        internal SimulationVoting CreateVoting(CreateSimulationVoting body)
        {
            var activeVotings = this._context.SimulationVotings.Where(n => n.Simulation.SimulationId == body.SimulationId
            && n.IsActive);
            if (activeVotings.Any())
                foreach(var v in activeVotings)
                    v.IsActive = false;


            var mdl = new SimulationVoting()
            {
                Description = "",
                IsActive = true,
                Name = body.Text,
                Simulation = this._context.Simulations.FirstOrDefault(n => n.SimulationId == body.SimulationId),
                AllowAbstention = body.AllowAbstention
            };

            if (body.Mode == EVotingMode.AllParticipants)
            {
                mdl.VoteSlots = _context.SimulationUser.Where(n => n.Simulation.SimulationId == body.SimulationId &&
                n.Role != null && n.Role.RoleType != RoleTypes.Chairman)
                    .Select(n => new SimulationVotingSlot()
                    {
                        Choice = EVoteStates.NotVoted,
                        User = n,
                        Voting = mdl
                    }).ToList();
            }
            else if (body.Mode == EVotingMode.JustDelegates)
            {
                mdl.VoteSlots = _context.SimulationUser.Where(n => n.Simulation.SimulationId == body.SimulationId &&
                n.Role != null && n.Role.RoleType == RoleTypes.Delegate)
                    .Select(n => new SimulationVotingSlot()
                    {
                        Choice = EVoteStates.NotVoted,
                        User = n,
                        Voting = mdl
                    }).ToList();
            }


            this._context.SimulationVotings.Add(mdl);
            this._context.SaveChanges();
            return mdl;
        }

        internal string GetSimulationUserCsv(int simulationId, string baseUrl)
        {
            string returnVal = "RoleType;RoleName;BenutzerId;Password;Einladungslink\n";
            var usersWithRoles = from users in _context.SimulationUser
                    join invites in _context.SimulationInvites on users.SimulationUserId equals invites.User.SimulationUserId
                    join roles in _context.SimulationRoles on users.Role.SimulationRoleId equals roles.SimulationRoleId
                    select new
                    {
                        RoleType = users.Role.RoleType.ToString(),
                        RoleName = users.Role.Name,
                        Kennung = users.PublicUserId,
                        Pass = users.Password,
                        Invite = baseUrl + invites.SimulationInviteId
                    };
            foreach(var entry in usersWithRoles)
            {
                returnVal += $"{entry.RoleType};{entry.RoleName};{entry.Kennung};{entry.Pass};{entry.Invite}\n";
            }

            var withoutRoles = from users in _context.SimulationUser
                               join invites in _context.SimulationInvites on users.SimulationUserId equals invites.User.SimulationUserId
                               where users.Role == null
                               select new
                               {
                                   Kennung = users.PublicUserId,
                                   Pass = users.Password,
                                   Invite = baseUrl + invites.SimulationInviteId
                               };

            foreach(var entry in withoutRoles)
            {
                returnVal += $"None;Keine Rolle;{entry.Kennung};{entry.Pass};{entry.Invite}\n";
            }
            return returnVal;
        }

        internal SimulationVoting CreateVotingForDelegates(int simulationId, string text, bool allowAbstentions)
        {
            var activeVotings = this._context.SimulationVotings.Where(n => n.Simulation.SimulationId == simulationId
            && n.IsActive);
            if (activeVotings.Any())
                foreach (var v in activeVotings)
                    v.IsActive = false;


            var mdl = new SimulationVoting()
            {
                Description = "",
                IsActive = true,
                Name = text,
                Simulation = this._context.Simulations.FirstOrDefault(n => n.SimulationId == simulationId),
                AllowAbstention = allowAbstentions
            };

            mdl.VoteSlots = _context.SimulationUser.Where(n => n.Simulation.SimulationId == simulationId &&
                n.Role != null && n.Role.RoleType == RoleTypes.Delegate)
                    .Select(n => new SimulationVotingSlot()
                    {
                        Choice = EVoteStates.NotVoted,
                        User = n,
                        Voting = mdl
                    }).ToList();

            this._context.SimulationVotings.Add(mdl);
            this._context.SaveChanges();
            return mdl;
        }

        internal async Task<SimulationListItemDto> GetInfo(int simulationId)
        {
            var simulation = await _context.Simulations.FirstOrDefaultAsync(n => n.SimulationId == simulationId);
            if (simulation == null)
                return null;
            var mdl = new SimulationListItemDto()
            {
                Name = simulation.Name,
                Phase = simulation.Phase,
                SimulationId = simulation.SimulationId,
                UsingPassword = false
            };
            return mdl;
        }

        internal async Task<string> GetUserToken(int userId)
        {
            var user = await _context.SimulationUser.FirstOrDefaultAsync(n => n.SimulationUserId == userId);
            return user?.Token ?? null;
        }

        internal bool Vote(UserVoteRequest body)
        {
            var slot = this._context.VotingSlots
                .FirstOrDefault(n => n.User.Token == body.Token &&
                n.Voting.SimulationVotingId == body.VotingId);
            if (slot == null)
                return false;

            slot.Choice = body.Choice;
            this._context.SaveChanges();
            return true;
        }

        internal bool VoteByUserId(string votingId, int userId, EVoteStates choice)
        {
            var slot = this._context.VotingSlots
                .FirstOrDefault(n => n.User.SimulationUserId == userId &&
                n.Voting.SimulationVotingId == votingId);
            if (slot == null)
                return false;

            slot.Choice = choice;
            this._context.SaveChanges();
            return true;
        }

        internal bool Vote(string votingId, string token, EVoteStates choice)
        {
            var slot = this._context.VotingSlots
                .FirstOrDefault(n => n.User.Token == token &&
                n.Voting.SimulationVotingId == votingId);
            if (slot == null)
                return false;

            slot.Choice = choice;
            this._context.SaveChanges();
            return true;
        }

        internal SimulationVotingDto GetCurrentVoting(int simulationId)
        {
            var voting = _context.SimulationVotings
                .Include(n => n.VoteSlots)
                .ThenInclude(n => n.User)
                .ThenInclude(n => n.Role)
                .FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.IsActive);
            if (voting == null)
                return null;

            var mdl = new SimulationVotingDto()
            {
                Description = voting.Description,
                Name = voting.Name,
                Slots = voting.VoteSlots.Select(n => new SimulationVoteSlotDto()
                {
                    Choice = n.Choice,
                    SimulationUserId = n.User.SimulationUserId,
                    SimulationVoteSlotId = n.SimulationVotingSlotId,
                    VoteTime = n.VoteTime,
                    DisplayName = n.User.DisplayName,
                    RoleName = n.User.Role.Name
                }).ToList(),
                VotingId = voting.SimulationVotingId,
                AllowAbstention = voting.AllowAbstention
            };
            return mdl;
        }

        internal SimulationStatus SetStatus(SetSimulationStatusDto body)
        {
            return SetStatus(body.SimulationId, body.StatusText);
        }

        internal SimulationStatus SetStatus(int simulationId, string statusText)
        {
            var simulation = _context.Simulations.FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return null;
            var status = new SimulationStatus()
            {
                Simulation = simulation,
                StatusText = statusText,
                StatusTime = DateTime.Now
            };
            _context.SimulationStatuses.Add(status);
            this._context.SaveChanges();
            return status;
        }

        internal SimulationRole CreateRole(CreateRoleRequest body)
        {
            var simulation = _context.Simulations.FirstOrDefault(n => n.SimulationId == body.SimulationId);
            if (simulation == null) return null;

            var newRole = new SimulationRole(body.Iso, body.Name, body.RoleType);
            newRole.Simulation = simulation;
            _context.SimulationRoles.Add(newRole);
            _context.SaveChanges();
            return newRole;
        }

        internal SimulationRole CreateRole(int simulationId, string name, string iso, RoleTypes type)
        {
            var simulation = _context.Simulations.FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return null;

            var newRole = new SimulationRole(iso, name, type);
            newRole.Simulation = simulation;
            _context.SimulationRoles.Add(newRole);
            _context.SaveChanges();
            return newRole;
        }

        internal PetitionType CreatePetitionType(CreatePetitionTypeRequest body)
        {
            var newPetitionType = new PetitionType()
            {
                Category = body.Category,
                Description = body.Description,
                Name = body.Name,
                Reference = body.Reference,
                Ruling = body.Ruling
            };
            _context.PetitionTypes.Add(newPetitionType);
            _context.SaveChanges();
            return newPetitionType;
        }

        internal bool RemoveAgendaItem(int agendaItemId)
        {
            var petitionsToRemove = this._context.Petitions.Where(n => n.AgendaItem.AgendaItemId == agendaItemId);
            if (petitionsToRemove.Any())
                this._context.Petitions.RemoveRange(petitionsToRemove);

            var agendaItemToRemove = this._context.AgendaItems.Find(agendaItemId);
            this._context.AgendaItems.Remove(agendaItemToRemove);
            this._context.SaveChanges();
            return true;
        }

        internal bool SetUserRole(SetUserSimulationRole body)
        {
            var role = _context.SimulationRoles.FirstOrDefault(n => n.SimulationRoleId == body.RoleId);
            if (role == null) return false;
            var user = _context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == body.UserId);
            if (user == null) return false;
            user.Role = role;
            _context.SaveChanges();
            return true;
        }

        internal bool SetUserRole(int userId, int roleId)
        {
            var role = _context.SimulationRoles.FirstOrDefault(n => n.SimulationRoleId == roleId);
            var user = _context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == userId);
            if (user == null) return false;
            user.Role = role;
            _context.SaveChanges();
            return true;
        }

        public Simulation CreateSimulation(string name, string password)
        {
            var sim = new Simulation()
            {
                Name = name,
                Password = password
            };

            sim.ListOfSpeakers = new MUNity.Models.ListOfSpeakers.ListOfSpeakers();

            this._context.Simulations.Add(sim);
            this._context.ListOfSpeakers.Add(sim.ListOfSpeakers);
            this._context.SaveChanges();
            return sim;
        }

        public string GetSpeakerlistIdOfSimulation(int simulationId)
        {
            return this._context.Simulations.Include(n => n.ListOfSpeakers).FirstOrDefault(n => n.SimulationId == simulationId)?.ListOfSpeakers?.ListOfSpeakersId;
        }

        public SimulationUser CreateModerator(Simulation simulation, string displayName)
        {
            var ownerUser = new SimulationUser()
            {
                CanCreateRole = true,
                CanEditListOfSpeakers = true,
                CanEditResolution = true,
                CanSelectRole = true,
                DisplayName = displayName,
                Role = null,
                Simulation = simulation,
            };
            _context.SimulationUser.Add(ownerUser);
            _context.SaveChanges();
            return ownerUser;
        }

        internal SimulationDto GetSimulationResponse(int id)
        {
            var simulation = GetSimulationWithHubsUsersAndRoles(id);
            return simulation.ToSimulationDto();
        }

        public SimulationUser CreateUser(int simulationId, string displayName)
        {
            // I have no idea why this is necessary but this will fix
            // that the users are suddenly empty...
            var hasSimulation = _context.Simulations
                .Include(n => n.Users)
                .FirstOrDefault(n => n.SimulationId == simulationId);

            if (hasSimulation == null)
            {
                Console.WriteLine("Unknown Simulation: " + simulationId.ToString() + " when creating a new user");
                return null;
            }

            var baseUser = new SimulationUser()
            {
                CanCreateRole = false,
                CanEditListOfSpeakers = false,
                CanEditResolution = false,
                CanSelectRole = false,
                DisplayName = displayName,
                Role = null,
                Simulation = hasSimulation
            };
            hasSimulation.Users.Add(baseUser);
            _context.SimulationUser.Add(baseUser);
            _context.SaveChanges();
            return baseUser;
        }

        internal bool RemovePetition(PetitionInteractRequest body)
        {
            var petition = _context.Petitions.FirstOrDefault(n => n.PetitionId == body.PetitionId);
            if (petition == null) return false;
            _context.Petitions.Remove(petition);
            int changes = _context.SaveChanges();
            return changes == 1;
        }

        /// <summary>
        /// Removes a Petition with the given id and will return the AgendaItemId of this petition.
        /// </summary>
        /// <param name="petitionId"></param>
        /// <returns></returns>
        internal int RemovePetition(string petitionId)
        {
            var petition = _context.Petitions.Include(n => n.AgendaItem).FirstOrDefault(n => n.PetitionId == petitionId);
            if (petition == null)
                return -1;
            _context.Petitions.Remove(petition);
            _context.SaveChanges();
            return petition.AgendaItem.AgendaItemId;
        }

        internal void ActivatePetition(string petitionId)
        {
            var petition = _context.Petitions.Include(n => n.AgendaItem).FirstOrDefault(n => n.PetitionId == petitionId);
            var agendaItemId = petition.AgendaItem.AgendaItemId;
            var currentActivePetition = _context.Petitions.FirstOrDefault(n => n.AgendaItem.AgendaItemId == agendaItemId &&
                n.Status == MUNity.Models.Simulation.EPetitionStates.Active);
            if (currentActivePetition != null)
            {
                _context.Petitions.Remove(currentActivePetition);
            }
            if (petition != null)
            {
                petition.Status = MUNity.Models.Simulation.EPetitionStates.Active;
            }
            _context.SaveChanges();
        }

        internal List<PetitionInfoDto> GetPetitionsOfAgendaItem(int agendaItemId)
        {
            return _context.Petitions.Where(n => n.AgendaItem.AgendaItemId == agendaItemId)
                .Select(n => new PetitionInfoDto()
                {
                    OrderIndex = n.AgendaItem.Simulation.PetitionTypes.FirstOrDefault(a =>
                    a.PetitionType.PetitionTypeId == n.PetitionType.PetitionTypeId).OrderIndex,
                    PetitionId = n.PetitionId,
                    SubmitterDisplayName = n.SimulationUser.DisplayName,
                    SubmitterRoleName = n.SimulationUser.Role.Name,
                    SubmitTime = n.PetitionDate,
                    TypeName = n.PetitionType.Name,
                    Status = n.Status,
                    CategoryName = n.PetitionType.Category,
                    RoleIso = n.SimulationUser.Role.Iso
                }).OrderBy(n => n.OrderIndex).ThenBy(n => n.SubmitTime).ToList();
        }

        internal PetitionInfoDto GetPetitionInfo(string petitionId)
        {
            var element = _context.Petitions
                .Include(n => n.AgendaItem)
                .ThenInclude(n => n.Simulation)
                .ThenInclude(n => n.PetitionTypes)
                .ThenInclude(n => n.PetitionType)
                .Include(n => n.SimulationUser)
                .ThenInclude(n => n.Role)
                .Include(n => n.PetitionType)
                .AsNoTracking()
                .FirstOrDefault(n => n.PetitionId == petitionId);

            if (element == null)
                return null;

            var dto = new PetitionInfoDto()
            {
                OrderIndex = element.AgendaItem.Simulation.PetitionTypes.FirstOrDefault(a =>
                    a.PetitionType.PetitionTypeId == element.PetitionType.PetitionTypeId).OrderIndex,
                PetitionId = element.PetitionId,
                SubmitterDisplayName = element.SimulationUser.DisplayName,
                SubmitterRoleName = element.SimulationUser.Role.Name,
                SubmitTime = element.PetitionDate,
                TypeName = element.PetitionType.Name,
                Status = element.Status,
                CategoryName = element.PetitionType.Category,
                RoleIso = element.SimulationUser.Role.Iso,
                AgendaItemId = element.AgendaItem.AgendaItemId
            };

            return dto;
        }

        internal async Task<bool> IsPetitionInteractionAllowed(PetitionInteractRequest body)
        {
            if (body == null || string.IsNullOrEmpty(body.PetitionId)) return false;
            if (!string.IsNullOrEmpty(Program.MasterToken) && body.Token == Program.MasterToken) return true;

            var isAdminOrChair = await IsTokenValidAndUserChairOrOwner(body);
            if (isAdminOrChair) return true;
            var petition = this._context.Petitions.Include(n => n.SimulationUser).FirstOrDefault(n => n.PetitionId == body.PetitionId);

            if (petition == null || petition.SimulationUser == null)
                return false;

            var isUsersPetition = petition.SimulationUser.Token == body.Token;
            if (isUsersPetition) return true;
            return await IsTokenValidAndUserChair(body);
        }

        public Task<Simulation> GetSimulationAsync(int id)
        {
            return this._context.Simulations.FirstOrDefaultAsync(n => n.SimulationId == id);
        }

        public Simulation GetSimulation(int id)
        {
            return this._context.Simulations.FirstOrDefault(n => n.SimulationId == id);
        }


        public Simulation GetSimulationWithHubsUsersAndRoles(int id)
        {
            var simulation = this._context.Simulations
                .Include(n => n.Roles).FirstOrDefault(n => n.SimulationId == id);
            if (simulation == null) return null;
            var users = this._context.SimulationUser
                .Where(a => a.Simulation.SimulationId == id).AsEnumerable();
            simulation.Users = users.ToList();
            return simulation;
        }

        public IEnumerable<Simulation> GetSimulations()
        {
            return this._context.Simulations.AsEnumerable();
        }

        public List<int> GetIdsOfAllSimulations()
        {
            return this._context.Simulations.Select(n => n.SimulationId).ToList();
        }


        //public IQueryable<SimulationRole> GetSimulationsRoles(int simulationId)
        //{
        //    return this._context.SimulationRoles.Where(n => n.Simulation.SimulationId == simulationId);
        //}

        public IQueryable<SimulationUser> GetSimulationUsers(int simulationId)
        {
            return this._context.SimulationUser.AsNoTracking().Where(n => n.Simulation.SimulationId == simulationId);
        }

        public List<SimulationUserInfoDto> GetSimulationUserInfos(int simulationId)
        {
            var list = this._context.SimulationUser.Where(n => n.Simulation.SimulationId == simulationId)
                .AsNoTracking()
                .Select(n => new SimulationUserInfoDto()
                {
                    SimulationUserId = n.SimulationUserId,
                    DisplayName = n.DisplayName,
                    RoleName = (n.Role != null) ? n.Role.Name : "",
                    RoleType = (n.Role != null) ? n.Role.RoleType : RoleTypes.None,
                    RoleIso = (n.Role != null) ? n.Role.Iso : "un"
                }).ToList();
            foreach(var user in list)
            {
                user.IsOnline = MUNityCore.Hubs.ConnectionUsers.ConnectionIds.AsEnumerable().Any(a => a.Value.SimulationId == simulationId &&
                        a.Value.SimulationUserId == user.SimulationUserId);
            }

            return list;
        }

        internal SimulationUserInfoDto GetSimulationUserInfo(int userId)
        {
            return this._context.SimulationUser.Where(n => n.SimulationUserId == userId)
                .AsNoTracking()
                .Select(n => new SimulationUserInfoDto()
                {
                    SimulationUserId = n.SimulationUserId,
                    DisplayName = n.DisplayName,
                    RoleName = (n.Role != null) ? n.Role.Name : "",
                    RoleType = (n.Role != null) ? n.Role.RoleType : RoleTypes.None,
                    RoleIso = (n.Role != null) ? n.Role.Iso : "un"
                }).FirstOrDefault();
        }

        public async Task<List<SimulationUserInfoDto>> GetSimulationUserInfosAsync(int simulationId)
        {
            return await this._context.SimulationUser.Where(n => n.Simulation.SimulationId == simulationId)
                .Select(n => new SimulationUserInfoDto()
                {
                    SimulationUserId = n.SimulationUserId,
                    DisplayName = n.DisplayName,
                    RoleName = (n.Role != null) ? n.Role.Name : "",
                    RoleType = (n.Role != null) ? n.Role.RoleType : RoleTypes.None

                }).ToListAsync();
        }

        public SimulationUser GetSimulationUserByPublicId(int simulationId, string publicId)
        {
            return this._context.SimulationUser.FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.PublicUserId == publicId);
        }

        public bool UserOnline(int simulationId, int userId)
        {
            return this._context.SimulationUser.Any(n => n.Simulation.SimulationId == simulationId && n.SimulationUserId == userId);
        }

        public SimulationUser GetSimulationUser(int simulationId, string token)
        {
            return this._context.SimulationUser.FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token);
        }

        public int? GetSimulationUserId(int simulationId, string token)
        {
            return this._context.SimulationUser.FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token)?.SimulationUserId;
        }
        

        public MUNity.Models.ListOfSpeakers.ListOfSpeakers InitListOfSpeakers(int simulationId)
        {
            var simulation = this._context.Simulations.Include(n => n.ListOfSpeakers).FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return null;
            simulation.ListOfSpeakers = new MUNity.Models.ListOfSpeakers.ListOfSpeakers();
            this._context.ListOfSpeakers.Add(simulation.ListOfSpeakers);
            this._context.SaveChanges();
            return simulation.ListOfSpeakers;
        }

        public void SaveDbChanges()
        {
            this._context.SaveChanges();
        }

        

        internal void ApplyPreset(Simulation simulation, ISimulationPreset preset, bool removeExistingRoles = true)
        {
            if (removeExistingRoles) simulation.Roles.Clear();
            simulation.Roles.AddRange(preset.Roles);
            this._context.SaveChanges();
        }



        #region roles

        public SimulationUser GetSimulationUserWithRole(int simulationId, string token)
        {
            return this._context.SimulationUser.Include(n => n.Role).FirstOrDefault(n => n.Simulation.SimulationId == simulationId && n.Token == token);
        }

        public SimulationRole AddChairmanRole(int simulationid, string name)
        {
            var simulation = this._context.Simulations
                .Include(n => n.Roles)
                .FirstOrDefault(n => n.SimulationId == simulationid);

            var currentChairmanRole = simulation.Roles.FirstOrDefault(n =>
                n.RoleType == RoleTypes.Chairman);
            if (currentChairmanRole == null)
            {
                currentChairmanRole = new SimulationRole()
                {
                    RoleType = RoleTypes.Chairman,
                    Iso = "UN"
                };
                simulation.Roles.Add(currentChairmanRole);
            }

            currentChairmanRole.Name = name;
            this._context.SaveChanges();
            return currentChairmanRole;
        }

        public Petition SubmitPetition(CreatePetitionRequest dto)
        {
            var agendaItem = this._context.AgendaItems.Include(n => n.Petitions).FirstOrDefault(n => n.AgendaItemId == dto.TargetAgendaItemId);
            if (agendaItem == null) throw new Exception("Agenda Item Not found");

            var petitionType = this._context.PetitionTypes.FirstOrDefault(n => n.PetitionTypeId == dto.PetitionTypeId);
            if (petitionType == null) throw new Exception("Petition not found");

            var user = this._context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == dto.PetitionUserId);
            if (user == null) throw new Exception("User not found!");

            var newItem = new Petition()
            {
                AgendaItem = agendaItem,
                PetitionDate = DateTime.Now,
                PetitionType = petitionType,
                SimulationUser = user,
                Status = dto.Status,
                Text = dto.Text
            };
            agendaItem.Petitions.Add(newItem);
            _context.SaveChanges();
            return newItem;
        }

        public Petition SubmitPetition(int agendaItemId, int petitionTypeId, int userId)
        {
            var agendaItem = this._context.AgendaItems.Include(n => n.Petitions).FirstOrDefault(n => n.AgendaItemId == agendaItemId);
            if (agendaItem == null) throw new Exception("Agenda Item Not found");

            var petitionType = this._context.PetitionTypes.FirstOrDefault(n => n.PetitionTypeId == petitionTypeId);
            if (petitionType == null) throw new Exception("Petition not found");

            var user = this._context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == userId);
            if (user == null) throw new Exception("User not found!");

            var newItem = new Petition()
            {
                AgendaItem = agendaItem,
                PetitionDate = DateTime.Now,
                PetitionType = petitionType,
                SimulationUser = user,
                Status = MUNity.Models.Simulation.EPetitionStates.Unkown,
                Text = ""
            };
            agendaItem.Petitions.Add(newItem);
            _context.SaveChanges();
            return newItem;
        }

        internal SimulationTokenResponse JoinSimulation(int simulationId, string userId, string userPass, string displayName = "")
        {
            var simulation = this._context.Simulations.FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null)
                return null;
            var user = this.GetSimulationUserByPublicId(simulationId, userId);
            if (user.Password != userPass)
                return null;

            if (!string.IsNullOrEmpty(displayName) && displayName != user.DisplayName)
            {
                user.DisplayName = displayName;
                this._context.SaveChanges();
            }
            if (string.IsNullOrEmpty(user.Token))
            {
                user.Token = Util.Tools.IdGenerator.RandomString(20);
                this.SaveDbChanges();
            }



            var response = user.ToTokenResponse();
            Console.WriteLine("User joined the simulation and got token: " + response.Token);
            return response;
        }

        internal bool UnlinkResolution(string resolutionId)
        {
            var auth = this._context.ResolutionAuths.Include(n => n.Simulation).FirstOrDefault(n => n.ResolutionId == resolutionId);
            if (auth == null) return false;
            auth.Simulation = null;
            this._context.SaveChanges();
            return true;
        }

        public async Task<bool> SetPhase(int simulationId, GamePhases phase)
        {
            var simulation = await this._context.Simulations.FirstOrDefaultAsync(n => n.SimulationId == simulationId);
            if (simulation == null) return false;
            simulation.Phase = phase;
            await this._context.SaveChangesAsync();
            return true;
        }

        public SimulationRole AddDelegateRole(int simulationId, string name, string iso)
        {
            var simulation = this._context.Simulations.FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return null;
            var role = new SimulationRole()
            {
                Iso = iso,
                Name = name,
                RoleType = RoleTypes.Delegate,
                Simulation = simulation,
            };
            _context.SimulationRoles.Add(role);
            this._context.SaveChanges();
            return role;
        }

        internal Task<List<ResolutionSmallInfo>> GetResolutions(int simulationId)
        {
            var resolutions = from auth in _context.ResolutionAuths
                              where auth.Simulation.SimulationId == simulationId
                              join resa in _context.Resolutions on auth.ResolutionId equals resa.ResaElementId
                              select new ResolutionSmallInfo()
                              {
                                  AllowAmendments = auth.AllowOnlineAmendments,
                                  AllowPublicEdit = auth.AllowPublicEdit,
                                  LastChangedTime = resa.CreatedDate,
                                  Name = resa.Topic,
                                  ResolutionId = resa.ResaElementId
                              };
            return resolutions.ToListAsync();
        }

        internal SimulationUser GetSimulationUser(int simulationUserId)
        {
            return _context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == simulationUserId);
        }

        public bool BecomeRole(Simulation simulation, SimulationUser user, SimulationRole role)
        {
            var users = this._context.SimulationUser
                .Include(n => n.Role)
                .Where(n => n.Simulation.SimulationId == simulation.SimulationId);

            // Cannot take this role because it is already taken!
            // if (users.Any(n => n.Role.SimulationRoleId == role.SimulationRoleId)) return false;

            user.Role = role;
            this._context.SaveChanges();
            return true;
        }

        public Task<List<SimulationRole>> GetSimulationRolesAsync(int simulationId)
        {
            return this._context.SimulationRoles.Where(n => n.Simulation.SimulationId == simulationId).ToListAsync();
        }

        public List<SimulationRole> GetSimulationRoles(int simulationId)
        {
            return this._context.SimulationRoles.Where(n => n.Simulation.SimulationId == simulationId).ToList();
        }

        #endregion

        public SimulationService(MunityContext context)
        {
            this._context = context;
        }

        public Task<List<Models.Simulation.PetitionType>> GetPetitionTypes()
        {
            return this._context.PetitionTypes.ToListAsync();
        }

        #region Validation

        public async Task<bool> IsTokenValidAndUserAdmin(SimulationRequest requestSchema)
        {
            return await _context.SimulationUser.AnyAsync(n => 
            n.Simulation.SimulationId == requestSchema.SimulationId && 
            n.Token == requestSchema.Token &&
            n.CanCreateRole);
        }

        public async Task<bool> IsTokenValidAndUserAdmin(int simulationId, string token)
        {
            return await _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token &&
            n.CanCreateRole);
        }

        internal async Task<bool> IsTokenValid(SimulationRequest requestSchema)
        {
            return await _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token);
        }

        public async Task<bool> IsTokenValidAsync(int simulationId, string token)
        {
            return await _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token);
        }

        public bool IsTokenValid(int simulationId, string token)
        {
            return _context.SimulationUser.Any(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token);
        }

        public async Task<bool> IsTokenValidAndUserDelegate(SimulationRequest requestSchema)
        {
            if (!string.IsNullOrEmpty(Program.MasterToken) && requestSchema.Token == Program.MasterToken) return true;
            return await _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token &&
            n.Role != null &&
            n.Role.RoleType == RoleTypes.Delegate);
        }

        public async Task<bool> IsTokenValidAndUserChair(SimulationRequest requestSchema)
        {
            if (!string.IsNullOrEmpty(Program.MasterToken) && requestSchema.Token == Program.MasterToken)
                return true;
            return await _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == requestSchema.SimulationId &&
            n.Token == requestSchema.Token &&
            n.Role != null &&
            n.Role.RoleType == RoleTypes.Chairman);
        }

        public bool IsUserChair(int userId)
        {
            var userWithRole = _context.SimulationUser.Include(n => n.Role)
                .FirstOrDefault(n => n.SimulationUserId == userId);
            if (userWithRole.Role == null) return false;
            return userWithRole.Role.RoleType == RoleTypes.Chairman;
        }

        public async Task<bool> IsTokenValidAndUserChair(int simulationId, string token)
        {
            if (!string.IsNullOrEmpty(Program.MasterToken) && token == Program.MasterToken)
                return true;
            return await _context.SimulationUser.AnyAsync(n =>
            n.Simulation.SimulationId == simulationId &&
            n.Token == token &&
            n.Role != null &&
            n.Role.RoleType == RoleTypes.Chairman);
        }

        public async Task<bool> IsTokenValidAndUserChairOrOwnerAsync(int simulationId, string token)
        {
            return await _context.SimulationUser.AsNoTracking().AnyAsync(n =>
                n.Simulation.SimulationId == simulationId &&
                n.Token == token &&
                (n.CanCreateRole ||
                n.Role != null &&
                n.Role.RoleType == RoleTypes.Chairman));
        }

        public bool IsTokenValidAndUserChairOrOwner(int simulationId, string token)
        {
            return _context.SimulationUser.AsNoTracking().Any(n =>
                n.Simulation.SimulationId == simulationId &&
                n.Token == token &&
                (n.CanCreateRole ||
                n.Role != null &&
                n.Role.RoleType == RoleTypes.Chairman));
        }

        public async Task<bool> IsTokenValidAndUserChairOrOwner(SimulationRequest request)
        {
            if (!string.IsNullOrEmpty(Program.MasterToken) && request.Token == Program.MasterToken)
                return true;
            return await IsTokenValidAndUserChairOrOwnerAsync(request.SimulationId, request.Token);
        }

        #endregion

        #region Petitions

        public async Task<bool> AddPetitionTypeToSimulation(MUNity.Schema.Simulation.Managment.AddPetitionTypeRequestBody request)
        {
            var simulation = await _context.Simulations.Include(n => n.PetitionTypes)
                .ThenInclude(n => n.PetitionType)
                .FirstOrDefaultAsync(n => n.SimulationId == request.SimulationId);
            if (simulation == null) return false;
            var petitionTypeToAdd = await _context.PetitionTypes.FirstOrDefaultAsync(n => n.PetitionTypeId == request.PetitionTypeId);
            if (petitionTypeToAdd == null) return false;
            if (simulation.PetitionTypes.Any(n => n.PetitionType.PetitionTypeId == request.PetitionTypeId)) return true;   //already inside
            var newEntry = new PetitionTypeSimulation()
            {
                AllowChairs = request.AllowChairs,
                AllowDelegates = request.AllowDelegates,
                AllowNgo = request.AllowNgo,
                AllowSpectator = request.AllowSpectator,
                OrderIndex = (request.OrderIndex != -1) ? request.OrderIndex : simulation.PetitionTypes.Count,
                PetitionType = petitionTypeToAdd,
                Simulation = simulation
            };
            simulation.PetitionTypes.Add(newEntry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AgendaItem> CreateAgendaItem(CreateAgendaItemDto agendaItem)
        {
            
            var simulation = this._context.Simulations.Include(n => n.AgendaItems).FirstOrDefault(n => n.SimulationId == agendaItem.SimulationId);
            if (simulation == null) return null;
            var hasAnyElements = simulation.AgendaItems.Any();
            if (hasAnyElements)
            {
                var hasMatchingAgendaItem = simulation.AgendaItems.Any(n =>
                (n.Name == agendaItem.Name));
                if (hasMatchingAgendaItem) return null;
            }
            
            var item = new AgendaItem(agendaItem);

            if (hasAnyElements)
            {
                item.OrderIndex = simulation.AgendaItems.Max(n => n.OrderIndex) + 1;
            }
            else
            {
                item.OrderIndex = 1;
            }

            simulation.AgendaItems.Add(item);
            _context.AgendaItems.Add(item);
            await this._context.SaveChangesAsync();
            return item;
        }

        public AgendaItem CreateAgendaItem(int simulationId, string name, string description)
        {
            var simulation = this._context.Simulations.Include(n => n.AgendaItems).FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return null;
            var hasAnyElements = simulation.AgendaItems.Any();
            if (hasAnyElements)
            {
                var hasMatchingAgendaItem = simulation.AgendaItems.Any(n =>
                (n.Name == name));
                if (hasMatchingAgendaItem) return null;
            }

            var item = new AgendaItem()
            {
                Name = name,
                Description = description
            };

            if (hasAnyElements)
            {
                item.OrderIndex = simulation.AgendaItems.Max(n => n.OrderIndex) + 1;
            }
            else
            {
                item.OrderIndex = 1;
            }

            simulation.AgendaItems.Add(item);
            _context.AgendaItems.Add(item);
            this._context.SaveChanges();
            return item;
        }

        public Task<List<Models.Simulation.PetitionType>> GetSimulationPetitionTypes(int simulationId)
        {
            return _context.SimulationPetitionTypes.Where(n => n.Simulation.SimulationId == simulationId)
                .Include(n => n.PetitionType).Select(n => n.PetitionType).ToListAsync();
        }

        internal List<SimulationSlotDto> GetSlots(int simulationId)
        {
            var t = from user in _context.SimulationUser
                    where user.Simulation.SimulationId == simulationId
                    select new SimulationSlotDto()
                    {
                        CanCreateRole = user.CanCreateRole,
                        CanEditListOfSpeakers = user.CanEditListOfSpeakers,
                        CanEditResolution = user.CanEditResolution,
                        CanSelectRole = user.CanSelectRole,
                        DisplayName = user.DisplayName,
                        IsOnline = false,
                        RoleId = (user.Role != null) ? user.Role.SimulationRoleId : -2,
                        RoleName = (user.Role != null) ? user.Role.Name : "",
                        RoleType = (user.Role != null) ? user.Role.RoleType : RoleTypes.None,
                        RoleIso = (user.Role != null) ? user.Role.Iso : "",
                        SimulationUserId = user.SimulationUserId
                    };
            return t.ToList();
        }

        #endregion

        #region User Managment

        public Task<Simulation> GetSimulationWithUsersAndRoles(int id)
        {
            return this._context.Simulations
                .Include(n => n.Roles)
                .Include(n => n.Users)
                .FirstOrDefaultAsync(n => n.SimulationId == id);
        }

        public async Task<List<AgendaItemDto>> GetAgendaItemsAndPetitionsDto(int simulationId)
        {
            var test = _context.AgendaItems
                .Where(n => n.Simulation.SimulationId == simulationId)
                .Select(n => new AgendaItemDto()
                {
                    AgendaItemId = n.AgendaItemId,
                    Name = n.Name,
                    Description = n.Description,
                    Status = n.Status,
                    Petitions = n.Petitions.Select(a => new PetitionDto()
                    {
                        PetitionDate = a.PetitionDate,
                        PetitionId = a.PetitionId,
                        PetitionTypeId = a.PetitionType.PetitionTypeId,
                        PetitionUserId = a.SimulationUser.SimulationUserId,
                        Status = a.Status,
                        TargetAgendaItemId = a.AgendaItem.AgendaItemId,
                        Text = a.Text
                    }).ToList()
                });
            if (test.Any())
                return await test.ToListAsync();
            return new List<AgendaItemDto>();
        }

        public async Task<List<AgendaItem>> GetAgendaItems(int simulationId)
        {
            var query = _context.AgendaItems.Where(n => n.Simulation.SimulationId == simulationId);
            var hasElements = await query.AnyAsync();
            if (hasElements) return await query.ToListAsync();
            return new List<AgendaItem>();
        }
        


        internal async Task RemoveUser(int simulationId, int userId)
        {
            
            var user = await this._context.SimulationUser.SingleOrDefaultAsync(n => n.SimulationUserId == userId);
            if (user != null)
            {
                this._context.SimulationUser.Remove(user);
                await this._context.SaveChangesAsync();
            }
        }

        internal void RemoveUser(int userId)
        {
            var user = this._context.SimulationUser.SingleOrDefault(n => n.SimulationUserId == userId);
            if (user != null)
            {
                this._context.SimulationUser.Remove(user);
                this._context.SaveChanges();
            }
        }

        internal void RemoveRole(int roleId)
        {
            var usersWithRole = this._context.SimulationUser.Where(n => n.Role.SimulationRoleId == roleId).ToList();
            if (usersWithRole.Any())
            {
                foreach(var user in usersWithRole)
                {
                    user.Role = null;
                }
            }
            var role = this._context.SimulationRoles.FirstOrDefault(n => n.SimulationRoleId == roleId);
            this._context.SimulationRoles.Remove(role);
            this._context.SaveChanges();
        }

        public SimulationUser JoinSimulation(Simulation simulation, string displayName)
        {
            if (simulation == null)
                return null;

            if (simulation.LobbyMode == MUNity.Schema.Simulation.LobbyModes.Closed)
                return null;

            var user = new SimulationUser()
            {
                DisplayName = displayName,
                CanSelectRole = true
            };

            simulation.Users.Add(user);
            this._context.SaveChanges();
            return user;
        }

        public SimulationUser JoinSimulation(int simulationId, string displayName)
        {
            var simulation = this._context.Simulations.Include(n => n.Users).FirstOrDefault();
            return JoinSimulation(simulation, displayName);
        }

        #endregion
        /// <summary>
        /// Refactor methods that use this implementation
        /// only to get things running
        /// </summary>
        /// <returns></returns>
        [Obsolete("Every call to the database should be behind a service shield. Use this method only to get things running fast and then refactor the code!")]
        internal MunityContext GetDatabaseInstance()
        {
            return this._context;
        }

        public SimulationPetitionTemplate LoadSimulationPetitionTemplate(string path, string name)
        {
            var mdl = new SimulationPetitionTemplate();
            mdl.TemplateName = name;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = true
            };
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                mdl.Entries = csv.GetRecords<PetitionTemplateEntry>().ToList();
            }
            return mdl;
        }

        public void ApplyPetitionTemplateToSimulation(string name, int simulationId)
        {
            var path = AppContext.BaseDirectory + "assets/templates/petitions/" + name + ".csv";
            if (!System.IO.File.Exists(path)) return;

            var template = this.LoadSimulationPetitionTemplate(path, name);
            if (template == null || !template.Entries.Any()) return;

            this.ApplyPetitionTemplateToSimulation(template, simulationId);
        }

        public void ApplyPetitionTemplateToSimulation(SimulationPetitionTemplate template, int simulationId)
        {
            var simulation = _context.Simulations.Include(n => n.PetitionTypes).FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null) return;

            simulation.PetitionTypes.Clear();
            int index = 0;
            // PetitionTypes erstellen
            foreach (var petitionTypeTemplate in template.Entries)
            {     
                // For now we only match the name!
                var mdlInDb = _context.PetitionTypes.FirstOrDefault(n => n.Name == petitionTypeTemplate.Name);
                if (mdlInDb == null)
                {
                    mdlInDb = new PetitionType()
                    {
                        Name = petitionTypeTemplate.Name,
                        Category = petitionTypeTemplate.Category,
                        Description = petitionTypeTemplate.Description,
                        Reference = petitionTypeTemplate.Reference,
                        Ruling = petitionTypeTemplate.Ruling
                    };
                    _context.PetitionTypes.Add(mdlInDb);
                }
                var newLink = new PetitionTypeSimulation()
                {
                    AllowChairs = petitionTypeTemplate.AllowChairs,
                    AllowDelegates = petitionTypeTemplate.AllowDelegates,
                    AllowNgo = petitionTypeTemplate.AllowNgo,
                    AllowSpectator = petitionTypeTemplate.AllowSpectator,
                    OrderIndex = index,
                    PetitionType = mdlInDb,
                    Simulation = simulation
                };
                simulation.PetitionTypes.Add(newLink);

                index++;
            }
            _context.SaveChanges();

        }
    
        public List<PetitionTypeSimulation> GetPetitionTypesOfSimulation(int simulationId)
        {
            return _context.SimulationPetitionTypes.Include(n => n.PetitionType)
                .Where(n => n.Simulation.SimulationId == simulationId).ToList();
        }

        public List<AgendaItemInfo> GetAgendaItemInfosForSim(int simulationId)
        {
            return _context.AgendaItems.Where(n => n.Simulation.SimulationId == simulationId)
                .Select(n => new AgendaItemInfo()
                {
                    AgendaItemId = n.AgendaItemId,
                    Description = n.Description,
                    Name = n.Name,
                    PetitionCount = n.Petitions.Count
                }).ToList();
        }

        internal SimulationStatus GetCurrentStatus(int simulationId)
        {
            return _context.SimulationStatuses
                .OrderByDescending(n => n.StatusTime)
                .FirstOrDefault(n => n.Simulation.SimulationId == simulationId);
        }
        
        /// <summary>
        /// Gibt Informationen über die Simulation zurück
        /// </summary>
        /// <returns></returns>
        // Versuche nicht diese Methoda Async zu machen, dass funktioniert nicht!
        internal List<MUNityCore.Dtos.Simulations.HomeScreenInfo> GetHomeScreenInfos() 
        {
            return _context.Simulations.AsNoTracking().Select(n => new MUNityCore.Dtos.Simulations.HomeScreenInfo() {
                Id = n.SimulationId,
                Name = n.Name,
                RoleNames = string.Join(", ", n.Roles.Select(a => a.Name)),
                SlotCount = n.Users.Count
            }).ToList();
        }

        internal List<string> GetPetitionPresetNames()
        {
            var list = new List<string>();
            string path = AppContext.BaseDirectory + "assets/templates/petitions/";
            if (!System.IO.Directory.Exists(path))
                return new List<string>();

            var dir = new System.IO.DirectoryInfo(path);
            var files = dir.GetFiles("*.csv");
            return files.Select(n => n.Name.Substring(0, n.Name.Length - 4)).ToList();
        }

        internal (string name, string iso) GetUserRoleOrName(int simulationId, string token)
        {
            var user = _context.SimulationUser.Include(n => n.Role).AsNoTracking().FirstOrDefault(n => n.Simulation.SimulationId == simulationId &&
            n.Token == token);
            if (user.Role != null) return (user.Role.Name, user.Role.Iso);
            return (user.DisplayName, "un");
        }

        internal string GetInviteLink(int userId)
        {
            var existingInviteLink = _context.SimulationInvites.FirstOrDefault(n => n.User.SimulationUserId == userId);
            if (existingInviteLink != null)
                return existingInviteLink.SimulationInviteId;

            var user = _context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == userId);
            if (user == null)
                return null;

            var inviteLink = new SimulationInvite()
            {
                SimulationInviteId = Util.Tools.IdGenerator.RandomString(30),
                ExpireDate = DateTime.Now.AddMonths(1),
                User = user
            };
            _context.SimulationInvites.Add(inviteLink);
            _context.SaveChanges();
            return inviteLink.SimulationInviteId;
        }

        internal InvitationResponse ValidateInviteLink(string inviteLink)
        {
            var result = _context.SimulationInvites
                .Include(n => n.User)
                .ThenInclude(n => n.Role)
                .Include(n => n.User)
                .ThenInclude(n => n.Simulation)
                .FirstOrDefault(n => n.SimulationInviteId == inviteLink);

            if (result == null)
                return null;

            var mdl = new InvitationResponse()
            {
                DisplayName = result.User.DisplayName,
                RoleIso = result.User.Role?.Iso ?? "",
                RoleName = result.User.Role?.Name ?? "",
                SimulationId = result.User.Simulation?.SimulationId ?? -1,
                SimulationName = result.User.Simulation?.Name ?? "",
                Token = result.User.Token,
                SimulationUserId = result.User.SimulationUserId
            };
            return mdl;
        }

        internal void SetUserDisplayName(int simulationUserId, string newName)
        {
            var user = _context.SimulationUser.FirstOrDefault(n => n.SimulationUserId == simulationUserId);
            if (user == null)
                return;
            user.DisplayName = newName;
            _context.SaveChanges();
        }

        internal void RemoveSimulation(int simulationId)
        {
            var simulation = _context.Simulations.FirstOrDefault(n => n.SimulationId == simulationId);
            if (simulation == null)
                return;
            Console.WriteLine("Request the deletion of Simulation: " + simulation.Name);
            RemoveHubConnectionsOfSimulation(simulationId);
            // Remove Petitions
            _context.Petitions.RemoveRange(_context.Petitions.Where(n => n.AgendaItem.Simulation.SimulationId == simulationId || n.SimulationUser.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed all Petitions");
            // Remove AgendaItems
            _context.AgendaItems.RemoveRange(_context.AgendaItems.Where(n => n.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed all AgendaItems");
            // Remove SimulationPetitionTypes
            _context.SimulationPetitionTypes.RemoveRange(_context.SimulationPetitionTypes.Where(n => n.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed all PetitionTypes");
            //Remove given votes
            _context.VotingSlots.RemoveRange(_context.VotingSlots.Where(n => n.Voting.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed all Votes");
            // votings
            _context.SimulationVotings.RemoveRange(_context.SimulationVotings.Where(n => n.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed all Votings");
            // Remove Invites
            _context.SimulationInvites.RemoveRange(_context.SimulationInvites.Where(n => n.User.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed all Invites");
            // Remove roles:
            _context.SimulationRoles.RemoveRange(_context.SimulationRoles.Where(n => n.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed all Roles");
            // Remove Users
            _context.SimulationUser.RemoveRange(_context.SimulationUser.Where(n => n.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed all Users");
            // Remove Log
            _context.SimulationLog.RemoveRange(_context.SimulationLog.Where(n => n.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed the log");
            // Remove Statuses
            _context.SimulationStatuses.RemoveRange(_context.SimulationStatuses.Where(n => n.Simulation.SimulationId == simulationId));
            _context.SaveChanges();
            Console.WriteLine("Removed the statuses");
            // Unlink resolutions
            var resolutions = _context.ResolutionAuths.Where(n => n.Simulation.SimulationId == simulationId);
            if (resolutions.Any())
            {
                foreach(var resolution in resolutions)
                {
                    resolution.Simulation = null;
                }
            }
            Console.WriteLine("Unlinked the Resolutions");

            _context.Simulations.Remove(simulation);
            _context.SaveChanges();
        }

        internal void RemoveHubConnectionsOfSimulation(int simulationId)
        {
            var connections = MUNityCore.Hubs.ConnectionUsers.ConnectionIds.Where(n => n.Value.SimulationId == simulationId).ToList();
            foreach(var connection in connections)
            {
                Hubs.ConnectionUsers.ConnectionIds.TryRemove(connection);
            }
        }

    }
}
