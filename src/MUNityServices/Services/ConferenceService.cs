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
using MUNity.Database.General;
using MUNity.Schema.Extensions;

namespace MUNity.Services
{
    public class ConferenceService
    {
        private readonly MunityContext context;

        private UserConferenceAuthService authService;

        public CreateConferenceResponse CreateConference(CreateConferenceRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateConferenceResponse();
            var user = context.Users.FirstOrDefault(n => n.UserName == claim.Identity.Name);
            if (user == null)
            {
                response.AddNoPermissionError("You are not allowed to create a conference.");
                return response;
            }

            var project = context.Projects.FirstOrDefault(n => n.ProjectId == request.ProjectId);
            if (project == null)
            {
                response.AddInvalidDataError("The Project was not found.", nameof(request.ProjectId));
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
            var defaultOwnerAuth = new ConferenceRoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = true,
                CanEditParticipations = true,
                CanSeeApplications = true,
                PowerLevel = 1,
                RoleAuthName = "Conference-Admin",
            };
            context.ConferenceRoleAuthorizations.Add(defaultOwnerAuth);

            var defaultTeamMemberAuth = new ConferenceRoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = false,
                CanEditParticipations = false,
                CanSeeApplications = true,
                PowerLevel = 2,
                RoleAuthName = "Team-Member"
            };
            context.ConferenceRoleAuthorizations.Add(defaultTeamMemberAuth);

            var defaultParticipantAuth = new ConferenceRoleAuth()
            {
                Conference = conference,
                CanEditConferenceSettings = false,
                CanEditParticipations = false,
                CanSeeApplications = false,
                PowerLevel = 5,
                RoleAuthName = "Participant"
            };
            context.ConferenceRoleAuthorizations.Add(defaultParticipantAuth);
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

        

        public async Task<List<ParticipatingConferenceInfo>> GetParticipatingConferencesAsync(ClaimsPrincipal claim)
        {
            var user = await authService.GetUserAsync(claim);
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
            var user = await authService.GetUserAsync(claim);
            var isAllowed = user != null && authService.IsUserAllowedToEditConference(request.ConferenceId, user.UserName);
            if (!isAllowed)
            {
                response.AddNoPermissionError();
                return response;
            }

            var conference = context.Conferences.FirstOrDefault(n => n.ConferenceId == request.ConferenceId);
            if (conference == null)
            {
                response.AddConferenceNotFoundError();
            }

            Committee parentCommittee = null;
            if (!string.IsNullOrEmpty(request.ResolutlyCommitteeId))
            {
                parentCommittee = context.Committees.FirstOrDefault(n => n.CommitteeId == request.ResolutlyCommitteeId);
                if (parentCommittee == null)
                {
                    response.AddCommitteeNotFoundError();
                }
            }

            if (response.HasError)
                return response;

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
            response.NewCommitteeId = committee.CommitteeId;
            return response;
        }

        public async Task<CommitteeSeatsInfo> GetCommitteeSeatsInfo(string committeeId, ClaimsPrincipal claim)
        {
            var conference = context.Committees
                .Include(n => n.Conference)
                .FirstOrDefault(n => n.CommitteeId == committeeId).Conference;
            if (conference == null)
                return null;
            var isAllowed = await authService.IsUserAllowedToEditConference(conference.ConferenceId, claim);
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

            info.Delegations = context.Delegations
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
                    CountryId = n.DelegateCountry.CountryId,
                    CountryName = n.DelegateCountry.Name,
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

        public async Task<CreateSeatResponse> CreateCommitteeSeat(CreateCommitteeSeatRequest request, ClaimsPrincipal claim)
        {
            var response = new CreateSeatResponse();
            var committee = context.Committees
                .Include(n => n.Conference)
                .FirstOrDefault(n => n.CommitteeId == request.CommitteeId);
            var isAllowed = await authService.IsUserAllowedToEditConference(committee.Conference.ConferenceId, claim);
            if (!isAllowed)
            {
                response.AddNoPermissionError();
                return response;
            }

            Country country = null;
            if (request.CountryId != -1)
            {
                country = context.Countries.FirstOrDefault(n => n.CountryId == request.CountryId);
                if (country == null)
                {
                    response.AddNotFoundError(nameof(request.CountryId));
                }
            }

            Delegation delegation = null;
            if (!string.IsNullOrEmpty(request.DelegationId))
            {
                delegation = context.Delegations.FirstOrDefault(n => n.DelegationId == request.DelegationId);
                if (delegation == null)
                {
                    response.AddNotFoundError(nameof(request.DelegationId));
                }
            }

            if (response.HasError)
                return response;

            var role = new ConferenceDelegateRole()
            {
                Committee = committee,
                Conference = committee.Conference,
                DelegateCountry = country,
                DelegateType = request.Subtype,
                Delegation = delegation,
                RoleName = request.RoleName,
                RoleFullName = request.RoleName,
                Title = request.RoleName
            };

            context.Delegates.Add(role);
            await context.SaveChangesAsync();
            response.CreatedRoleId = role.RoleId;
            return response;
        }

        public async Task<CreateSeatResponse> CreateFreeSeat(CreateFreeSeatRequest request,
            ClaimsPrincipal claim)
        {
            var response = new CreateSeatResponse();
            var isAllowed = await authService.IsUserAllowedToEditConference(request.ConferenceId, claim);
            if (!isAllowed)
            {
                response.AddNoPermissionError();
                return response;
            }

            var conference = context.Conferences.FirstOrDefault(n => n.ConferenceId == request.ConferenceId);
            if (conference == null)
            {
                response.AddNotFoundError(nameof(request.ConferenceId));
            }

            Country country = null;
            if (request.CountryId != -1)
            {
                country = context.Countries.FirstOrDefault(n => n.CountryId == request.CountryId);
                if (country == null)
                {
                    response.AddNotFoundError(nameof(request.CountryId));
                }
            }

            Delegation delegation = null;
            if (!string.IsNullOrEmpty(request.DelegationId))
            {
                delegation = context.Delegations.FirstOrDefault(n => n.DelegationId == request.DelegationId);
                if (delegation == null)
                {
                    response.AddNotFoundError(nameof(request.DelegationId));
                }
            }

            if (response.HasError)
                return response;

            var role = new ConferenceDelegateRole()
            {
                Committee = null,
                Conference = conference,
                DelegateCountry = country,
                DelegateType = request.Subtype,
                Delegation = delegation,
                RoleName = request.RoleName,
                RoleFullName = request.RoleName,
                Title = request.RoleName
            };

            context.Delegates.Add(role);
            await context.SaveChangesAsync();
            response.CreatedRoleId = role.RoleId;
            return response;
        }

        public async Task<ConferenceRolesInfo> GetRolesInfo(string conferenceId, ClaimsPrincipal claim)
        {
            var isAllowed = await authService.IsUserAllowedToEditConference(conferenceId, claim);
            if (!isAllowed)
                return null;

            var mdl = context.Conferences
                .Include(n => n.ConferenceProject)
                .ThenInclude(n => n.ProjectOrganization)
                .Include(n => n.Roles)
                .Include(n => n.Delegations)
                .AsSingleQuery()
                .AsNoTracking()
                .Select(conf => new ConferenceRolesInfo()
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
                    Roles = conf.Roles.OfType<ConferenceDelegateRole>()
                        .Select(role => new ManageDelegationRoleInfo()
                        {
                            ApplicationState = role.ApplicationState,
                            HasParicipant = role.Participations.Any(),
                            RoleCommitteeId = role.Committee.CommitteeId,
                            RoleCommitteeName = role.Committee.Name,
                            RoleId = role.RoleId,
                            RoleName = role.RoleName,
                            Subtype = role.DelegateType,
                            DelegationId = role.Delegation.DelegationId,
                            DelegationName = role.Delegation.Name
                        }).ToList(),
                    Delegations = conf.Delegations.Select(del => new DelegationInfo()
                    {
                        DelegationId = del.DelegationId,
                        DelegationName = del.Name
                    }).ToList()
                }).FirstOrDefault(n => n.ConferenceId == conferenceId);

            mdl.Countries = context.Countries.AsNoTracking().Select(n => new CountryInfo()
            {
                Name = n.Name,
                CountryId = n.CountryId
            }).ToList();

            return mdl;
        }

        public ConferenceService(MunityContext context, UserConferenceAuthService authService)
        {
            this.context = context;
            this.authService = authService;
        }
    }
}
