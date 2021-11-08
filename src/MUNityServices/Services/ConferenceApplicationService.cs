using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Schema.Conference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class ConferenceApplicationService
    {
        private MunityContext _dbContext;

        private ILogger<ConferenceApplicationService> _logger;

        public enum AvailableDelegationsTypes
        {
            All,
            OnlineOnly,
            PresenceOnly
        }

        public List<ApplicationAvailableDelegation> AvailableDelegations(string conferenceId, int minRolesCount = 1, AvailableDelegationsTypes type = AvailableDelegationsTypes.All)
        {
            IQueryable<Delegation> delegationsQuery;

            switch (type)
            {
                case AvailableDelegationsTypes.All:
                    throw new NotImplementedException();
                case AvailableDelegationsTypes.OnlineOnly:
                    delegationsQuery = _dbContext.Fluent.ForConference(conferenceId)
                .DelegationsWithOnlyOnlineRoles();
                    break;
                case AvailableDelegationsTypes.PresenceOnly:
                    delegationsQuery = _dbContext.Fluent.ForConference(conferenceId)
                .DelegationsWithOnlyAtLocationSlots();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return delegationsQuery.Select(n => new ApplicationAvailableDelegation()
                {
                    DelegationId = n.DelegationId,
                    Name = n.Name,
                    Roles = n.Roles.Select(role => new ApplicationDelegationRoleSlot()
                    {
                        CommitteeName = role.Committee.Name,
                        Costs = _dbContext.Fluent.ForConference(conferenceId).GetCostOfRole(role.RoleId),
                        RoleName = role.RoleName,
                        CountryName = role.DelegateCountry.Name,
                        CountryIso = role.DelegateCountry.Iso
                    }).ToList()
                }).ToList();
        }

        public FindUserForApplicationResult FindUserToAddToDelegationApplication(string mailOrUsername, string conferenceId)
        {
            var normalized = mailOrUsername.ToUpper();
            var user = _dbContext.Users.FirstOrDefault(n => n.NormalizedUserName == normalized || n.NormalizedEmail == normalized);
            if (user != null)
            {
                // Check if user is already participating
                var alreadyParticipating = _dbContext.Participations.Any(n => n.User.Id == user.Id && n.Role.Conference.ConferenceId == conferenceId);
                if (alreadyParticipating)
                    return new FindUserForApplicationResult() { Status = FindUserForApplicationResult.ResultStatuses.AlreadyParticipating };

                // Check user already in an application
                var alreadyApplying = _dbContext.DelegationApplicationUserEntries
                    .Any(n => n.User.Id == user.Id && n.Application.DelegationWishes
                        .Any(a => a.Delegation.Conference.ConferenceId == conferenceId));
                if (alreadyApplying)
                    return new FindUserForApplicationResult() { Status = FindUserForApplicationResult.ResultStatuses.AlreadyApplying };

                // Return the user and ok
                return new FindUserForApplicationResult()
                {
                    Status = FindUserForApplicationResult.ResultStatuses.CanBeAdded,
                    ForeName = user.Forename,
                    LastName = user.Lastname,
                    UserName = user.UserName
                };
            }
            else
            {
                // Return user doesnt exists result
                return new FindUserForApplicationResult() { Status = FindUserForApplicationResult.ResultStatuses.NoUserFound };
            }
        }

        public List<UserApplicationInfo> GetApplicationsOfUser(ClaimsPrincipal claim)
        {
            var list = new List<UserApplicationInfo>();

            var delegationApplications = _dbContext.DelegationApplicationUserEntries
                .Where(n => n.User.NormalizedUserName == claim.Identity.Name.ToUpper())
                .Include(n => n.Application)
                .AsNoTracking()
                .Select(n => new UserApplicationInfo()
                {
                    ApplicationId = n.Application.DelegationApplicationId,
                    ConferenceFullName = n.Application.Conference.FullName,
                    ConferenceId = n.Application.Conference.ConferenceId,
                    ConferenceName = n.Application.Conference.Name,
                    ConferenceShort = n.Application.Conference.ConferenceShort
                });

            
            return delegationApplications.ToList();
        }

        public ConferenceApplicationService(MunityContext context, ILogger<ConferenceApplicationService> logger)
        {
            this._dbContext = context;
            this._logger = logger;
        }
    }
}
