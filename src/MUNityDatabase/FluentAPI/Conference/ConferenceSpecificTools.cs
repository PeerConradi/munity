using Microsoft.EntityFrameworkCore;
using MUNity.Base;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI
{
    public class ConferenceSpecificTools
    {
        private MunityContext _dbContext;

        private string _conferenceId;

        /// <summary>
        /// Creates a new Committee inside the given Conference.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Committee AddCommittee(Action<CommitteeOptionsBuilder> options)
        {
            var conference = _dbContext.Conferences.Find(_conferenceId);

            if (conference == null)
                throw new ConferenceNotFoundException($"No conference found for {_conferenceId}");

            var builder = new CommitteeOptionsBuilder();
            options(builder);
            builder.Committee.Conference = conference;

            var committeeEasy = Util.IdGenerator.AsPrimaryKey(builder.Committee.CommitteeShort);
            builder.Committee.CommitteeId = conference.ConferenceId + "-" + committeeEasy;

            if (builder.Committee.ChildCommittees.Count > 0)
            {
                foreach (var committeeChildCommittee in builder.Committee.ChildCommittees)
                {
                    committeeChildCommittee.Conference = conference;
                }
            }
            conference.Committees.Add(builder.Committee);
            this._dbContext.SaveChanges();
            return builder.Committee;
        }

        public TeamRoleGroup AddTeamRoleGroup(Action<ITeamRoleBuilder> options)
        {
            var conference = _dbContext.Conferences.Find(_conferenceId);

            if (conference == null)
                throw new ConferenceNotFoundException($"No conference found for {_conferenceId}");

            var builder = new TeamRoleGroupBuilder();
            options(builder);
            conference.TeamRoleGroups.Add(builder.Group);
            if (builder.Group.TeamRoles.Count > 0)
            {
                foreach (var conferenceTeamRole in builder.Group.TeamRoles)
                {
                    conferenceTeamRole.Conference = conference;
                }
            }
            _dbContext.SaveChanges();
            return builder.Group;
        }

        public decimal GetCostOfRole(int roleId)
        {

            // Check if the role has a Cost exception
            // if yes return it
            var roleCosts = _dbContext.ConferenceParticipationCostRules.FirstOrDefault(n =>
            n.Role.RoleId == roleId);

            if (roleCosts != null && roleCosts.Costs.HasValue)
                return roleCosts.Costs.Value;

            // Check of the delegation has a cost exception
            // if yes return it
            var delegationId = _dbContext.Delegates.Where(n => n.RoleId == roleId)
                .Select(n => n.Delegation.DelegationId)
                .FirstOrDefault();

            if (delegationId != null)
            {
                var delegationCosts = _dbContext.ConferenceParticipationCostRules.FirstOrDefault(n =>
                n.Delegation.DelegationId == delegationId);
                if (delegationCosts?.Costs != null)
                    return delegationCosts.Costs.Value;
            }

            // Check of the committee has a cost exception
            // if yes return it
            var committeeId = _dbContext.Delegates.Where(n => n.RoleId == roleId)
                .Select(n => n.Committee.CommitteeId)
                .FirstOrDefault();
            if (committeeId != null)
            {
                var committeeCosts = _dbContext.ConferenceParticipationCostRules.FirstOrDefault(n =>
                n.Committee.CommitteeId == committeeId);
                if (committeeCosts?.Costs != null)
                    return committeeCosts.Costs.Value;
            }

            // Check for the cost of the conference
            // and return it.
            var conferenceId = _dbContext.Delegates
                .AsNoTracking()
                .Where(n => n.RoleId == roleId)
                .Select(n => n.Conference.ConferenceId).FirstOrDefault();

            if (conferenceId == null)
                throw new ConferenceNotFoundException($"No Conference for the role {roleId} found");

            var conferenceCost = _dbContext.Conferences.Find(conferenceId).GeneralParticipationCost;
            return conferenceCost;
        }

        public decimal GetTeamRoleCost(int teamRoleId)
        {
            // Check if the role has a Cost exception
            // if yes return it
            var roleCosts = _dbContext.ConferenceParticipationCostRules.FirstOrDefault(n =>
            n.Role.RoleId == teamRoleId);

            if (roleCosts != null && roleCosts.Costs.HasValue)
                return roleCosts.Costs.Value;

            var conferenceId = _dbContext.ConferenceTeamRoles
                .AsNoTracking()
                .Where(n => n.RoleId == teamRoleId)
                .Select(n => n.Conference.ConferenceId).FirstOrDefault();

            if (conferenceId == null)
                throw new ConferenceNotFoundException($"No Conference for the role {teamRoleId} found");

            var conferenceCost = _dbContext.Conferences.Find(conferenceId).GeneralParticipationCost;
            return conferenceCost;
        }

        public Participation MakeUserParticipateInTeamRole(string username, string roleName)
        {
            var user = _dbContext.Users.FirstOrDefault(n => n.NormalizedUserName == username.ToUpper());

            if (user == null)
                throw new UserNotFoundException($"No user found for {username}");

            var roles = _dbContext.ConferenceTeamRoles.Where(n => n.Conference.ConferenceId == _conferenceId &&
            n.RoleName == roleName).ToList();
            if (roles.Count == 0)
                throw new ConferenceRoleNotFoundException($"No role with the name {roleName} was found fpr the Conference {_conferenceId}");
            if (roles.Count > 1)
                throw new NotUnambiguousRoleNameException($"The role name '{roleName}' is not unambiguous for the conference {_conferenceId} make sure it is or use the RoleId overload of this method.");
            var role = roles.First(); 

            var participation = new Participation()
            {
                Cost = GetTeamRoleCost(role.RoleId),
                IsMainRole = true,
                Paid = 0,
                ParticipationSecret = "TODO",
                Role = role,
                User = user
            };
            _dbContext.Participations.Add(participation);
            _dbContext.SaveChanges();
            return participation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="delegationName">if left to null the countryname will be used</param>
        /// <param name="delegationFullName">if left null the wird Delegation + delegationName will be used.</param>
        /// <param name="delegationShortName">if left to null the country ISO will be used.</param>
        /// <returns></returns>
        public Delegation GroupRolesOfCountryIntoADelegation(string countryName,
            string delegationName = null, string delegationFullName = null, string delegationShortName = null)
        {
            var conference = _dbContext.Conferences.FirstOrDefault(n => n.ConferenceId == _conferenceId);
            if (conference == null)
                throw new NullReferenceException($"The conference with id '{_conferenceId}' was not found!");


            if (delegationFullName == null)
                delegationFullName = "Delegation " + delegationName;

            var country = _dbContext.Countries.FirstOrDefault(n => n.Name == countryName || n.FullName == countryName);
            if (country == null)
                throw new CountryNotFoundException($"Country {countryName} not found...");

            if (delegationName == null)
                delegationName = country.Name;

            if (_dbContext.Delegations.Any(n => n.Name == delegationName && n.Conference.ConferenceId == _conferenceId))
                throw new NameTakenException($"The Delegationname {delegationName} is already taken!");


                if (delegationFullName == null)
                delegationFullName = $"Delegation {delegationName}";

            if (delegationShortName == null)
                delegationShortName = country.Iso;

            var delegation = new Delegation()
            {
                Conference = conference,
                Name = delegationName,
                Roles = _dbContext.Delegates
                    .Where(n => n.DelegateCountry.CountryId == country.CountryId && 
                    n.Conference.ConferenceId == _conferenceId).ToList(),
                FullName = delegationFullName,
                DelegationShort = delegationShortName
            };
            _dbContext.Delegations.Add(delegation);
            _dbContext.SaveChanges();
            return delegation;
        }

        public bool HasDelegationByName(string name)
        {
            return _dbContext.Delegations.Any(n => n.Conference.ConferenceId == _conferenceId &&
            n.Name == name || n.FullName == name);
        }

        public int DelegationSizeByName(string name)
        {
            var size = _dbContext.Delegates.Count(n => (n.Delegation.Name == name || n.Delegation.FullName == name &&
                n.Conference.ConferenceId == _conferenceId));

            if (size == 0)
                size = _dbContext.Delegations.Any(n => n.Name == name || n.FullName == name) ? 0 : -1;
            return size;
        }

        public Delegation GroupRolesOfCountryIntoADelegationByCommitteeIds(string countryName, params string[] committees)
        {
            var country = _dbContext.Countries.AsNoTracking().FirstOrDefault(n => n.Name == countryName || n.FullName == countryName);

            if (country == null)
                throw new CountryNotFoundException($"Country with the name {countryName} was not found.");

            var delegation = new Delegation()
            {
                Conference = _dbContext.Conferences.Find(_conferenceId),
                DelegationShort = country.Iso,
                FullName = "Delegation " + country.Name,
                Name = countryName,
                Roles = new List<ConferenceDelegateRole>()
            };

            foreach(var committee in committees)
            {
                var role = _dbContext.Delegates.FirstOrDefault(n => n.Conference.ConferenceId == _conferenceId &&
                n.Committee.CommitteeId == committee && n.DelegateCountry.CountryId == country.CountryId);

                if (role != null)
                    delegation.Roles.Add(role);
                else
                    throw new ConferenceRoleNotFoundException($"{country.Name} ({country.CountryId}) seems to not be represented inside a committee with the id {committee}");
            }

            _dbContext.Delegations.Add(delegation);
            _dbContext.SaveChanges();
            return delegation;
        }

        public Delegation AddDelegation(Action<DelegationBuilder> options)
        {
            DelegationBuilder builder = new DelegationBuilder(_dbContext, _conferenceId);
            options(builder);
            var delegation = builder.Delegation;
            return delegation;
        }

        public List<ConferenceParticipationCostRule> AddCostRuleForRolesOfSubType(string subTypeName, decimal costs, string name)
        {
            var roles = this._dbContext.Delegates.Where(n => n.DelegateType == subTypeName &&
            n.Conference.ConferenceId == _conferenceId);

            var list = new List<ConferenceParticipationCostRule>();

            foreach (var role in roles)
            {
                var costRule = new ConferenceParticipationCostRule()
                {
                    Committee = null,
                    Conference = null,
                    Role = role,
                    Delegation = null,
                    AddPercentage = null,
                    CostRuleTitle = name,
                    Costs = costs,
                    CutPercentage = null,
                    UserMaxAge = null,
                    UserMinAge = null
                };

                _dbContext.ConferenceParticipationCostRules.Add(costRule);
                list.Add(costRule);
            }

            _dbContext.SaveChanges();
            return list;
        }

        public DelegationApplicationBuilder CreateDelegationApplication()
        {
            var options = _dbContext.ConferenceApplicationOptions.FirstOrDefault(n => n.Conference.ConferenceId == _conferenceId);

            if (options == null || options.AllowDelegationApplication == false)
                throw new ApplicationTypeNotAllowedException($"The Application type DelegationApplication is not allowed make sure you have 'AllowDelegationApplication' set to true.");
            
            var builder = new DelegationApplicationBuilder(_dbContext, _conferenceId);
            return builder;
        }

        public DelegationCostResult CostsOfDelegationByName(string delegationName)
        {
            var result = new DelegationCostResult();

            var delegationId = _dbContext.Delegations.Where(n =>
            n.Conference.ConferenceId == _conferenceId &&
            n.Name == delegationName)
                .Select(n => n.DelegationId).FirstOrDefault();

            if (delegationId == null)
                throw new DelegationNotFoundException($"No Delegation with the Name {delegationName} found for conference {_conferenceId}");

            var delegationWithRoles = _dbContext.Delegations
                .Include(n => n.Roles)
                .Select(n => new
                {
                    DelegationId = n.DelegationId,
                    ConferenceId = n.Conference.ConferenceId,
                    Roles = n.Roles.Select(a => new
                    {
                        RoleId = a.RoleId,
                        RoleName = a.RoleName,
                        CommitteeId = a.Committee.CommitteeId,
                    })
                })
                .FirstOrDefault(n => n.DelegationId == delegationId);


            if (delegationWithRoles == null)
                throw new NullReferenceException($"The Delegation with Id {delegationName} was not found!");

            decimal priceForConference =
                _dbContext.Conferences.Where(n => n.ConferenceId == delegationWithRoles.ConferenceId)
                    .Select(a => a.GeneralParticipationCost)
                    .FirstOrDefault();

            decimal? priceForDelegation =
                _dbContext.ConferenceParticipationCostRules.Where(n => n.Delegation.DelegationId == delegationId)
                    .Select(n => n.Costs)
                    .FirstOrDefault();

            foreach (var role in delegationWithRoles.Roles)
            {
                // Check for role price
                decimal? rolePrice = _dbContext.ConferenceParticipationCostRules
                    .Where(n => n.Role.RoleId == role.RoleId)
                    .Select(n => n.Costs)
                    .FirstOrDefault();



                // Check for Committee Price
                if (rolePrice == null && role.CommitteeId != null)
                {

                    if (priceForDelegation != null)
                    {
                        // Prio 2: Price for the delegation
                        result.Costs.Add(new DelegationCostPoint()
                        {
                            CommitteeName = _dbContext.Delegates.Where(n => n.RoleId == role.RoleId).Select(n => n.Committee.Name).FirstOrDefault(),
                            Cost = priceForDelegation.Value,
                            RoleId = role.RoleId,
                            RoleName = role.RoleName
                        });
                    }
                    else
                    {
                        // Prio 3: Price for the committee
                        decimal? committeePrice = _dbContext.ConferenceParticipationCostRules
                            .Where(n => n.Committee.CommitteeId == role.CommitteeId)
                            .Select(n => n.Costs)
                            .FirstOrDefault();

                        if (committeePrice != null)
                        {
                            result.Costs.Add(new DelegationCostPoint()
                            {
                                CommitteeName = _dbContext.Delegates.Where(n => n.RoleId == role.RoleId).Select(n => n.Committee.Name).FirstOrDefault(),
                                Cost = committeePrice.Value,
                                RoleId = role.RoleId,
                                RoleName = role.RoleName
                            });
                        }
                        else
                        {
                            result.Costs.Add(new DelegationCostPoint()
                            {
                                CommitteeName = _dbContext.Delegates.Where(n => n.RoleId == role.RoleId).Select(n => n.Committee.Name).FirstOrDefault(),
                                Cost = priceForConference,
                                RoleId = role.RoleId,
                                RoleName = role.RoleName
                            });
                        }
                    }
                }
                else if (rolePrice != null)
                {
                    // Prio 1: Role Price (Highest)
                    result.Costs.Add(new DelegationCostPoint()
                    {
                        CommitteeName = _dbContext.Delegates.Where(n => n.RoleId == role.RoleId).Select(n => n.Committee.Name).FirstOrDefault(),
                        Cost = rolePrice.Value,
                        RoleId = role.RoleId,
                        RoleName = role.RoleName
                    });
                }

            }

            return result;
        }

        public int AddBasicAuthorizations()
        {
            var conference = this._dbContext.Conferences.FirstOrDefault(n => n.ConferenceId == _conferenceId);
            var ownerAuth = new ConferenceRoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = true,
                CanEditParticipations = true,
                CanSeeApplications = true,
                PowerLevel = 1,
                RoleAuthName = "Project-Owner"
            };
            _dbContext.ConferenceRoleAuthorizations.Add(ownerAuth);

            var participantControllingTeamAuth = new ConferenceRoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = false,
                CanEditParticipations = true,
                CanSeeApplications = true,
                PowerLevel = 2,
                RoleAuthName = "Team (Participant Management)"
            };
            _dbContext.ConferenceRoleAuthorizations.Add(participantControllingTeamAuth);

            var teamAuth = new ConferenceRoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = false,
                CanEditParticipations = false,
                CanSeeApplications = true,
                RoleAuthName = "Team (Basic)",
                PowerLevel = 3,
            };
            _dbContext.ConferenceRoleAuthorizations.Add(teamAuth);

            var participantAuth = new ConferenceRoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = false,
                RoleAuthName = "Participant",
                PowerLevel = 4,
                CanSeeApplications = false,
                CanEditParticipations = false
            };
            _dbContext.ConferenceRoleAuthorizations.Add(participantAuth);

            return _dbContext.SaveChanges();
        }

        public IQueryable<DelegationApplication> ApplicationsWithFreeSlots()
        {
            return _dbContext.DelegationApplications.Where(n => n.Conference.ConferenceId == _conferenceId &&
                n.OpenToPublic &&
                (n.Users.Count(n => n.Status == DelegationApplicationUserEntryStatuses.Joined || n.Status == DelegationApplicationUserEntryStatuses.Invited) < n.DelegationWishes.Max(a => a.Delegation.Roles.Count)));
        }

        public IQueryable<Delegation> DelegationsWithOnlyAtLocationSlots(int minRolesCount = 0)
        {
            return _dbContext.Delegations.Where(n => n.Conference.ConferenceId == _conferenceId &&
               n.Roles.Count >= minRolesCount &&
               n.Roles.All(a => a.Committee.CommitteeType == CommitteeTypes.AtLocation));
        }

        public IQueryable<Delegation> DelegationsWithOnlyAtLocationAndRoleCount(int roleCount)
        {
            return _dbContext.Delegations.Where(n => n.Conference.ConferenceId == _conferenceId &&
               n.Roles.Count == roleCount &&
               n.Roles.All(a => a.Committee.CommitteeType == CommitteeTypes.AtLocation));
        }

        public IQueryable<Delegation> DelegationsWithOnlyOnlineRoles(int minRolesCount = 0)
        {
            return _dbContext.Delegations.Where(n => n.Conference.ConferenceId == _conferenceId &&
               n.Roles.Count >= minRolesCount &&
               n.Roles.All(a => a.Committee.CommitteeType == CommitteeTypes.Online));
        }

        public ConferenceSpecificTools(MunityContext context, [NotNull]string conferenceId)
        {
            this._dbContext = context;
            this._conferenceId = conferenceId;
        }
    }
}
