using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Schema.Conference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class ConferenceService
    {
        private readonly MunityContext context;

        public CreateConferenceResponse CreateConference(CreateConferenceRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateConferenceResponse();
            var user = context.Users.FirstOrDefault(n => n.UserName == claim.Identity.Name);
            if (user == null)
            {
                response.Status = CreateConferenceResponse.CreateConferecenStatuses.NoPermission;
                return response;
            }

            var project = context.Projects.FirstOrDefault(n => n.ProjectId == request.ProjectId);
            if (project == null)
            {
                response.Status = CreateConferenceResponse.CreateConferecenStatuses.ProjectNotFound;
                return response;
            }

            // Create the conference
            var conference = new Conference()
            {
                Name = request.Name,
                FullName = request.FullName,
                ConferenceShort = request.ConferenceShort,
                ConferenceProject = project,
                CreationDate = DateTime.UtcNow,
                CreationUser = user
            };

            var easyId = Util.IdGenerator.AsPrimaryKey(request.ConferenceShort);
            if (context.Conferences.All(n => n.ConferenceId != easyId))
                conference.ConferenceId = easyId;

            if (DateTime.TryParse(request.StartDate, out DateTime start))
                conference.StartDate = start;

            if (DateTime.TryParse(request.EndDate, out DateTime end))
                conference.EndDate = end;

            context.Conferences.Add(conference);

            //// Create the Creator/Admin Role
            //var roleGroup = new TeamRoleGroup()
            //{
            //    FullName = "Conference Owner",
            //    Name = "Owner",
            //    GroupLevel = 1,
            //    TeamRoleGroupShort = "Ow",
            //    Conference = conference
            //};
            //context.TeamRoleGroups.Add(roleGroup);

            //var roleAuth = new RoleAuth()
            //{
            //    Conference = conference,
            //    CanEditConferenceSettings = true,
            //    CanEditParticipations = true,
            //    CanSeeApplications = true,
            //    PowerLevel = 1,
            //    RoleAuthName = "Owner-Permissions",
            //};

            //context.RoleAuths.Add(roleAuth);

            //var ownerRole = new ConferenceTeamRole()
            //{
            //    Conference = conference,
            //    IconName = "ow",
            //    RoleFullName = "Conference-Owner",
            //    RoleAuth = roleAuth,
            //    RoleName = "Owner",
            //    RoleShort = "OW",
            //    TeamRoleGroup = roleGroup,
            //    TeamRoleLevel = 1
            //};

            //context.TeamRoles.Add(ownerRole);

            context.SaveChanges();

            response.ConferenceId = conference.ConferenceId;

            return response;
        }

        public ConferenceService(MunityContext context)
        {
            this.context = context;
        }
    }
}
