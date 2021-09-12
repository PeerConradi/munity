using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.User;
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

        private UserManager<MunityUser> userManager;

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
                CreationUser = user,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            var easyId = Util.IdGenerator.AsPrimaryKey(request.ConferenceShort);
            if (context.Conferences.All(n => n.ConferenceId != easyId))
                conference.ConferenceId = easyId;

            //if (DateTime.TryParse(request.StartDate, out DateTime start))
            //    conference.StartDate = start;

            //if (DateTime.TryParse(request.EndDate, out DateTime end))
            //    conference.EndDate = end;

            context.Conferences.Add(conference);


            CreateDefaultConferenceAuths(conference);

            context.SaveChanges();

            response.ConferenceId = conference.ConferenceId;

            return response;
        }

        private void CreateDefaultConferenceAuths(Conference conference)
        {
            var defaultOwnerAuth = new RoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = true,
                CanEditParticipations = true,
                CanSeeApplications = true,
                PowerLevel = 1,
                RoleAuthName = "Conference-Admin",
            };
            context.RoleAuths.Add(defaultOwnerAuth);

            var defaultTeamMemberAuth = new RoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = false,
                CanEditParticipations = false,
                CanSeeApplications = true,
                PowerLevel = 2,
                RoleAuthName = "Team-Member"
            };
            context.RoleAuths.Add(defaultTeamMemberAuth);

            var defaultParticipantAuth = new RoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = false,
                CanEditParticipations = false,
                CanSeeApplications = false,
                PowerLevel = 5,
                RoleAuthName = "Participant"
            };
            context.RoleAuths.Add(defaultParticipantAuth);
        }

        public async Task<CreateTeamRoleGroupResponse> CreateRoleGroupAsync(CreateTeamRoleGroupRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateTeamRoleGroupResponse();
            var user = await userManager.GetUserAsync(claim);
            var isAllowed = user != null && IsUserAllowedToEditTeam(request.ConferenceId, user.UserName);
            if (!isAllowed)
            {
                response.Status = CreateTeamRoleGroupResponse.ResponseStatuses.NoPermission;
                return response;
            }

            var conference = context.Conferences.FirstOrDefault(n => n.ConferenceId == request.ConferenceId);
            if (conference == null)
            {
                response.Status = CreateTeamRoleGroupResponse.ResponseStatuses.ConferenceNotFound;
                return response;
            }

            if (context.TeamRoleGroups.Any(n => n.Conference.ConferenceId == request.ConferenceId && n.Name == request.GroupName))
            {
                response.Status = CreateTeamRoleGroupResponse.ResponseStatuses.NameTaken;
                return response;
            }

            if (context.TeamRoleGroups.Any(n => n.Conference.ConferenceId == request.ConferenceId && n.FullName == request.GroupFullName))
            {
                response.Status = CreateTeamRoleGroupResponse.ResponseStatuses.FullNameTaken;
                return response;
            }

            if (context.TeamRoleGroups.Any(n => n.Conference.ConferenceId == request.ConferenceId && n.TeamRoleGroupShort == request.GroupShort))
            {
                response.Status = CreateTeamRoleGroupResponse.ResponseStatuses.ShortTaken;
                return response;
            }

            var group = new TeamRoleGroup()
            {
                Conference = conference,
                FullName = request.GroupFullName,
                GroupLevel = request.GroupLevel,
                Name = request.GroupName,
                TeamRoleGroupShort = request.GroupShort
            };

            context.TeamRoleGroups.Add(group);
            context.SaveChanges();
            response.Status = CreateTeamRoleGroupResponse.ResponseStatuses.Success;
            response.CreatedGroupId = group.TeamRoleGroupId;
            return response;
        }

        public async Task<CreateTeamRoleResponse> CreateTeamRoleAsync(CreateTeamRoleRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateTeamRoleResponse();
            var group = context.TeamRoleGroups.Include(n => n.Conference)
                .FirstOrDefault(n => n.TeamRoleGroupId == request.RoleGroupId);

            if (group == null)
            {
                response.Status = CreateTeamRoleResponse.StatusCodes.GroupNotFound;
                return response;
            }
            var user = await userManager.GetUserAsync(claim);
            var isAllowed = user != null && IsUserAllowedToEditTeam(group.Conference.ConferenceId, user.UserName);
            if (!isAllowed)
            {
                response.Status = CreateTeamRoleResponse.StatusCodes.NoPermission;
                return response;
            }

            ConferenceTeamRole parentRole = null;
            if (request.ParentRoleId != -1)
            {
                // We are also checking for the ConferenceId to make sure that the given Parent role is in
                // the same conference!
                parentRole = context.TeamRoles.FirstOrDefault(n =>
                n.RoleId == request.ParentRoleId &&
                n.Conference.ConferenceId == group.Conference.ConferenceId);
            }


            var role = new ConferenceTeamRole()
            {
                Conference = group.Conference,
                ParentTeamRole = parentRole,
                RoleName = request.RoleName,
                RoleShort = request.RoleShort,
                RoleFullName = request.RoleFullName,
                TeamRoleGroup = group,
                TeamRoleLevel = 0
            };
            context.TeamRoles.Add(role);
            context.SaveChanges();
            response.Status = CreateTeamRoleResponse.StatusCodes.Success;
            response.RoleId = role.RoleId;

            return response;
        }

        public ManageTeamInfo GetTeamDashboard(string conferenceId)
        {
            var dashboardInfo = context.Conferences.Select(n => new ManageTeamInfo()
            {
                OrganizationName = n.ConferenceProject.ProjectOrganization.OrganizationName,
                OrganizationShort = n.ConferenceProject.ProjectOrganization.OrganizationShort,
                OrganizationId = n.ConferenceProject.ProjectOrganization.OrganizationId,
                ProjectName = n.ConferenceProject.ProjectName,
                ProjectShort = n.ConferenceProject.ProjectShort,
                ProjectId = n.ConferenceProject.ProjectId,
                ConferenceName = n.Name,
                ConferenceShort = n.ConferenceShort,
                ConferenceId = n.ConferenceId
            }).FirstOrDefault(n => n.ConferenceId == conferenceId);

            if (dashboardInfo == null)
                return null;

            dashboardInfo.RoleGroups = context.TeamRoleGroups
                .Include(n => n.TeamRoles)
                .ThenInclude(n => n.Participations)
                .AsSingleQuery()
                .Where(n => n.Conference.ConferenceId == conferenceId)
                .Select(n => new TeamRoleGroupInfo()
                {
                    FullName = n.FullName,
                    Name = n.Name,
                    Short = n.TeamRoleGroupShort,
                    TeamRoleGroupId = n.TeamRoleGroupId,
                    Roles = n.TeamRoles.Select(a => new TeamRoleInfo()
                    {
                        FullName = a.RoleFullName,
                        Name = a.RoleName,
                        ParentId = a.ParentTeamRole.RoleId,
                        ParentName = a.ParentTeamRole.RoleName,
                        Short = a.RoleShort,
                        TeamRoleId = a.RoleId,
                        Participants = a.Participations.Select(p => new RoleParticipant()
                        {
                            ParticipantDisplayName = p.User.Forename + " " + p.User.Lastname,
                            ParticipantUsername = p.User.UserName
                        }).ToList()
                    }).ToList()
                }).ToList();

            return dashboardInfo;
        }

        public bool IsUserAllowedToEditTeam(string conferenceId, string username)
        {
            var isCreator = context.Conferences.Any(n => n.ConferenceId == conferenceId && n.CreationUser.UserName == username);
            if (isCreator)
                return true;

            var isAllowedTeamMember = context.Participations.Any(n => n.User.UserName == username && n.Role.Conference.ConferenceId == conferenceId && n.Role.RoleAuth.CanEditConferenceSettings == true);
            return isAllowedTeamMember;
        }

        public bool IsUserAllowedToEditConference(string conferenceId, string username)
        {
            var isCreator = context.Conferences.Any(n => n.ConferenceId == conferenceId && n.CreationUser.UserName == username);
            if (isCreator)
                return true;

            var isAllowedTeamMember = context.Participations.Any(n => n.User.UserName == username && n.Role.Conference.ConferenceId == conferenceId && n.Role.RoleAuth.CanEditConferenceSettings == true);
            return isAllowedTeamMember;
        }

        public async Task<bool> IsUserAllowedToEditConference(string conferenceId, ClaimsPrincipal claim)
        {
            var user = await userManager.GetUserAsync(claim);
            if (user == null)
                return false;

            return IsUserAllowedToEditConference(conferenceId, user.UserName);
        }

        public async Task<List<ParticipatingConferenceInfo>> GetParticipatingConferencesAsync(ClaimsPrincipal claim)
        {
            var user = await userManager.GetUserAsync(claim);
            if (user == null)
                return null;

            var list = new List<ParticipatingConferenceInfo>();
            // Load Conferences this user owns
            var owningConferences = context.Conferences.Where(n => n.CreationUser.UserName == user.UserName)
                .Select(n => new ParticipatingConferenceInfo()
                {
                    ConferenceFullName = n.FullName,
                    ConferenceShort = n.ConferenceShort,
                    ConferenceId = n.ConferenceId,
                    IsTeamMember = true
                }).ToList();

            list.AddRange(owningConferences);
            return list;
        }

        public ManageCommitteesInfo GetManageInfo(string conferenceId)
        {
            return context.Conferences
                .Select(conf => new ManageCommitteesInfo()
                {
                    ConferenceId = conf.ConferenceId,
                    ConferenceName = conf.Name,
                    ConferenceShort = conf.ConferenceShort,
                    OrganizationId = conf.ConferenceProject.ProjectOrganization.OrganizationId,
                    OrganizationName = conf.ConferenceProject.ProjectOrganization.OrganizationName,
                    OrganizationShort = conf.ConferenceProject.ProjectOrganization.OrganizationShort,
                    ProjectId = conf.ConferenceProject.ProjectId,
                    ProjectName = conf.ConferenceProject.ProjectName,
                    ProjectShort = conf.ConferenceProject.ProjectShort,
                    Committees = conf.Committees.Select(comm => new ManageCommitteeInfo()
                    {
                        CommitteeId = comm.CommitteeId,
                        CommitteeName = comm.Name,
                        CommitteeShort = comm.CommitteeShort,
                        ParentCommitteeId = comm.ResolutlyCommittee.CommitteeId,
                        ParentCommitteeName = comm.ResolutlyCommittee.Name,
                        SeatCount = context.Delegates.Count(n => n.Committee.CommitteeId == comm.CommitteeId)
                    }).ToList()
                }).FirstOrDefault(n => n.ConferenceId == conferenceId);
        }

        public async Task<CreateCommitteeResponse> CreateCommitteeAsync(CreateCommitteeRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateCommitteeResponse();
            var user = await userManager.GetUserAsync(claim);
            var isAllowed = user != null && IsUserAllowedToEditConference(request.ConferenceId, user.UserName);
            if (!isAllowed)
            {
                response.Status = CreateCommitteeResponse.StatusCodes.NoPermission;
                return response;
            }

            var conference = context.Conferences.FirstOrDefault(n => n.ConferenceId == request.ConferenceId);
            if (conference == null)
            {
                response.Status = CreateCommitteeResponse.StatusCodes.ConferenceNotFound;
                return response;
            }

            Committee parentCommittee = null;
            if (!string.IsNullOrEmpty(request.ResolutlyCommitteeId))
            {
                parentCommittee = context.Committees.FirstOrDefault(n => n.CommitteeId == request.ResolutlyCommitteeId);
                if (parentCommittee == null)
                {
                    response.Status = CreateCommitteeResponse.StatusCodes.ResolutlyCommitteeNotFound;
                    return response;
                }
            }

            var committee = new Committee()
            {
                Article = request.Article,
                CommitteeShort = request.Short,
                Conference = conference,
                FullName = request.FullName,
                Name = request.Name,
                ResolutlyCommittee = parentCommittee
            };

            var shortAsId = Util.IdGenerator.AsPrimaryKey(request.Short);
            var easyId = conference.ConferenceId + "-" + shortAsId;
            if (context.Committees.All(n => n.CommitteeId != easyId))
            {
                committee.CommitteeId = easyId;
            }
            context.Committees.Add(committee);
            context.SaveChanges();
            response.Status = CreateCommitteeResponse.StatusCodes.Success;
            response.NewCommitteeId = committee.CommitteeId;
            return response;
        }

        public async Task<CommitteeSeatsInfo> GetCommitteeSeatsInfo(string committeeId, ClaimsPrincipal claim)
        {
            var conference = context.Committees
                .Include(n => n.Conference)
                .FirstOrDefault(n => n.CommitteeId == committeeId).Conference;
            var isAllowed = await IsUserAllowedToEditConference(conference.ConferenceId, claim);
            if (!isAllowed)
                return null;

            var info = context.Committees
                .Where(n => n.CommitteeId == committeeId)
                .Select(n => new CommitteeSeatsInfo()
                {
                    CommitteeArticle = n.Article,
                    CommitteeId = n.CommitteeId,
                    CommitteeName = n.Name,
                    CommitteeShort = n.CommitteeShort
                }).FirstOrDefault();

            if (info == null)
                return null;

            info.Countries = context.Countries
                .Select(a => new CountryInfo()
                {
                    CountryId = a.CountryId,
                    Name = a.Name
                }).ToList();

            info.Delegations = context.Delegation
                .Where(n => n.Conference.ConferenceId == conference.ConferenceId)
                .Select(n => new DelegationInfo()
                {
                    DelegationId = n.DelegationId,
                    DelegationName = n.Name
                }).ToList();

            info.Seats = context.Delegates
                .Where(n => n.Committee.CommitteeId == committeeId)
                .Select(n => new CommitteeSeatInfo()
                {
                    CountryId = n.DelegateState.CountryId,
                    CountryName = n.DelegateState.Name,
                    DelegationId = n.Delegation.DelegationId,
                    DelegationName = n.Delegation.Name,
                    RoleId = n.RoleId,
                    RoleName = n.RoleName,
                    Subtypes = n.DelegateType,
                    Participants = n.Participations.Select(a => new CommitteeParticipation()
                    {
                        DisplayName = a.User.Forename + " " + a.User.Lastname,
                        ParticipationId = a.ParticipationId,
                        Username = a.User.UserName
                    }).ToList()
                }).ToList();

            return info;
        }

        public ConferenceService(MunityContext context, UserManager<MunityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
    }
}
