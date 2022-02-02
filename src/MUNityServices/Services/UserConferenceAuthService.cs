using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class UserConferenceAuthService
    {
        private MunityContext context;

        private UserManager<MunityUser> userManager;

        public bool IsUserAllowedToEditTeam(string conferenceId, string username)
        {
            var isCreator = context.Conferences.Any(n => n.ConferenceId == conferenceId && n.CreationUser.UserName == username);
            if (isCreator)
                return true;

            var isAllowedTeamMember = context.Participations.Any(n => n.User.UserName == username && n.Role.Conference.ConferenceId == conferenceId && n.Role.ConferenceRoleAuth.CanEditConferenceSettings == true);
            return isAllowedTeamMember;
        }

        public async Task<bool> IsUserAllowedToEditTeam(string conferenceId, ClaimsPrincipal claim)
        {
            var user = await userManager.GetUserAsync(claim);
            if (user == null)
                return false;
            return IsUserAllowedToEditTeam(conferenceId, user.UserName);
        }

        public bool IsUserAllowedToEditConference(string conferenceId, string username)
        {
            var isCreator = context.Conferences.Any(n => n.ConferenceId == conferenceId && n.CreationUser.UserName == username);
            if (isCreator)
                return true;

            var isAllowedTeamMember = context.Participations.Any(n => n.User.UserName == username && n.Role.Conference.ConferenceId == conferenceId && n.Role.ConferenceRoleAuth.CanEditConferenceSettings == true);
            return isAllowedTeamMember;
        }

        public async Task<bool> IsUserAllowedToEditConference(string conferenceId, ClaimsPrincipal claim)
        {
            var user = await userManager.GetUserAsync(claim);
            if (user == null)
                return false;

            return IsUserAllowedToEditConference(conferenceId, user.UserName);
        }

        public async Task<bool> IsUserAllowedToEditConference(string conferenceId, Task<AuthenticationState> authStateTask)
        {
            return await IsUserAllowedToEditConference(conferenceId, (await authStateTask)?.User);
        }

        public async Task<bool> IsUserTeamMember(string conferenceId, ClaimsPrincipal claim)
        {
            var user = await userManager.GetUserAsync(claim);
            if (user == null)
                return false;

            return context.Participations.Any(n => n.Role is ConferenceTeamRole && n.User == user && n.Role.Conference.ConferenceId == conferenceId);
        }

        public async Task<bool> IsUserTeamMemberForCommitteeOrHigher(string committeeId, ClaimsPrincipal claim)
        {
            var user = await userManager.GetUserAsync(claim);
            if (user == null)
                return false;

            var conferenceId = context.Committees
                .AsNoTracking()
                .Where(n => n.CommitteeId == committeeId).Select(n => n.Conference.ConferenceId).FirstOrDefault();


            var isTeamMember = context.Participations.Any(n => n.Role is ConferenceTeamRole && n.User == user && n.Role.Conference.ConferenceId == conferenceId);
            if (isTeamMember)
                return true;

            var isOwner = context.Conferences.Any(n => n.ConferenceId == conferenceId && n.CreationUser == user);
            return isOwner;
        }

        public Task<MunityUser> GetUserAsync(ClaimsPrincipal claim)
        {
            return userManager.GetUserAsync(claim);
        }

        public UserConferenceAuthService(MunityContext context, UserManager<MunityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
    }
}
