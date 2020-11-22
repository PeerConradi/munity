using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MUNityCore.Util.Extensions;
using MUNityCore.DataHandlers.EntityFramework;
using MUNityCore.Exceptions.ConferenceExceptions;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Conference.Roles;
using MUNityCore.Models.Organization;
using MUNityCore.Schema.Response.Conference;

namespace MUNityCore.Services
{
    public class ConferenceService : IConferenceService
    {
        private readonly MunityContext _context;

        public Project CreateProject(string name, string abbreviation, Organization organization)
        {
            var project = new Project
            {
                ProjectName = name,
                ProjectShort = abbreviation,
                ProjectOrganization = organization
            };

            var idFromShort = Util.Tools.IdGenerator.AsPrimaryKey(abbreviation);

            if (!_context.Projects.Any(n => n.ProjectId == idFromShort))
                project.ProjectId = idFromShort;

            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        public Task<Project> GetProject(string id)
        {
            return _context.Projects.FirstOrDefaultAsync(n => n.ProjectId == id);
        }

        public Task<Project> GetFullProject(string id)
        {
            return _context.Projects
                .Include(n => n.Conferences)
                .Include(n => n.ProjectOrganization)
                .FirstOrDefaultAsync(n => n.ProjectId == id);
        }

        /// <summary>
        /// Creates an empty conference. This conference does not contain any roles
        /// participations etc.
        /// </summary>
        /// <param name="name">The name of the conference like MUN Schleswig-Holstein 2021</param>
        /// <param name="fullname">The full name of the conference for example Model United Nations Schleswig-Holstein 2021</param>
        /// <param name="abbreviation">A short name of the conference like: MUN-SH 2021</param>
        /// <param name="project">The project that this conference belongs to. Like the Model United Nations Schleswig-Holstein projects.</param>
        /// <exception cref="ArgumentException">every conference has to be created inside a parent project.</exception>
        /// <returns></returns>
        public Conference CreateConference(string name, string fullname, string abbreviation, Project project)
        {
            if (_context.Conferences.Any(n => n.Name == name || n.FullName == fullname))
                throw new NameAlreadyTakenException("The conference name or Fullname is already taken by another conference.");

            if (project == null)
                throw new ArgumentException("The project cannot be null!");

            var conference = new Conference
            {
                Name = name,
                FullName = fullname,
                ConferenceShort = abbreviation,
                CreationDate = DateTime.Now,
                StartDate = new DateTime(1980, 1, 1),
                EndDate = new DateTime(1980, 1, 4)
            };
            if (!_context.Conferences.Any(n => n.ConferenceId == abbreviation))
                conference.ConferenceId = abbreviation;
            conference.ConferenceProject = project;

            _context.Conferences.Add(conference);
            _context.SaveChanges();

            return conference;
        }

        /// <summary>
        /// Gets a conference with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Conference> GetConference(string id)
        {
            return _context.Conferences.Include(n => n.Committees).FirstOrDefaultAsync(n => n.ConferenceId == id);
        }

        /// <summary>
        /// Creates a Committee for a conference.
        /// </summary>
        /// <param name="conference"></param>
        /// <param name="name"></param>
        /// <param name="fullname"></param>
        /// <param name="abbreviation"></param>
        /// <returns></returns>
        public Committee CreateCommittee(Conference conference, string name, string fullname, string abbreviation)
        {
            if (conference == null)
                throw new ArgumentException("The conference can not be null, make sure you give a valid conference when creating a committee.");

            var committee = new Committee
            {
                Name = name,
                FullName = fullname,
                CommitteeShort = abbreviation
            };



            string customid = conference.ConferenceId + "-" + abbreviation.ToUrlValid();
            if (!_context.Committees.Any(n => n.CommitteeId == customid))
                committee.CommitteeId = customid;


            committee.Conference = conference;
            _context.Committees.Add(committee);

            _context.SaveChanges();
            return committee;
        }

        public Task<Committee> GetCommittee(string id)
        {
            return _context.Committees.FirstOrDefaultAsync(n => n.CommitteeId == id);
        }

        public TeamRole CreateLeaderRole(Conference conference)
        {
            var roleAuth = new RoleAuth
            {
                RoleAuthName = "Leader",
                Conference = conference,
                CanEditConferenceSettings = true,
                CanSeeApplications = true,
                CanEditParticipations = true,
                PowerLevel = 1
            };
            _context.RoleAuths.Add(roleAuth);
            var role = new TeamRole
            {
                Conference = conference,
                RoleAuth = roleAuth,
                RoleName = "Leader",
                ApplicationState = EApplicationStates.Closed,
                ApplicationValue = "",
                IconName = "default",
                ParentTeamRole = null
            };
            _context.TeamRoles.Add(role);
            _context.SaveChanges();
            return role;
        }

        public Task SaveDatabaseChanges()
        {
            return this._context.SaveChangesAsync();
        }

        public TeamRoleGroup CreateTeamRoleGroup(Conference conference, string groupName, string groupFullName, string groupShort, int level)
        {
            var group = new TeamRoleGroup()
            {
                Name = groupName,
                FullName = groupFullName,
                TeamRoleGroupShort = groupShort,
                GroupLevel = level
            };

            this._context.TeamRoleGroups.Add(group);
            this._context.SaveChanges();
            return group;
        }

        public TeamRole CreateTeamRole(Conference conference, string roleName, TeamRole parentRole = null, RoleAuth auth = null)
        {
            var role = new TeamRole {Conference = conference, RoleName = roleName, RoleAuth = auth};

            if (parentRole != null)
                role.ParentTeamRole = parentRole;

            _context.TeamRoles.Add(role);
            _context.SaveChanges();
            return role;
        }

        public bool AddRoleToGroup(TeamRole role, TeamRoleGroup group)
        {
            if (group != null && role != null)
            {
                group.TeamRoles.Add(role);
                this._context.SaveChanges();
                return true;
            }

            return false;
        }

        public SecretaryGeneralRole CreateSecretaryGeneralRole(Conference conference, string roleName, string title, RoleAuth auth = null)
        {
            var role = new SecretaryGeneralRole {Title = title, RoleName = roleName, Conference = conference};

            _context.SecretaryGenerals.Add(role);
            _context.SaveChanges();
            return role;
        }

        public IQueryable<TeamRole> GetTeamRoles(string conferenceId)
        {
            return _context.TeamRoles.Where(n => n.Conference.ConferenceId == conferenceId);
        }

        public Delegation CreateDelegation(Conference conference, string name)
        {
            var delegation = new Delegation {Conference = conference, Name = name};

            _context.Delegation.Add(delegation);
            _context.SaveChanges();

            return delegation;
        }

        public IQueryable<Delegation> GetDelegations(string conferenceId)
        {
            return _context.Delegation.Where(c => c.Conference.ConferenceId == conferenceId);
        }

        public DelegateRole CreateDelegateRole(Committee committee, Delegation delegation, string name, bool isLeader = false)
        {
            var role = new DelegateRole
            {
                RoleName = name,
                Committee = committee,
                Conference = committee.Conference,
                Delegation = delegation,
                IsDelegationLeader = isLeader,
                Title = name
            };

            _context.Delegates.Add(role);
            _context.SaveChanges();
            return role;
        }

        public IQueryable<DelegateRole> GetDelegateRolesOfConference(string conferenceId)
        {
            return _context.Delegates.Where(c => c.Conference.ConferenceId == conferenceId);
        }

        public IQueryable<DelegateRole> GetDelegateRolesOfCommittee(string committeeId)
        {
            return _context.Delegates.Where(n => n.Committee.CommitteeId == committeeId);
        }

        public IQueryable<DelegateRole> GetDelegateRolesOfDelegation(string delegationId)
        {

            return _context.Delegates.Where(n => n.Delegation.DelegationId == delegationId);
        }

        public NgoRole CreateNgoRole(Conference conference,string roleName, string ngoName)
        {
            var role = new NgoRole {RoleName = roleName, NgoName = ngoName, Conference = conference};

            _context.NgoRoles.Add(role);
            _context.SaveChanges();
            return role;

        }

        public IQueryable<NgoRole> GetNgoRoles(string conferenceId)
        {
            return _context.NgoRoles.Where(n => n.Conference.ConferenceId == conferenceId);
        }

        public PressRole CreatePressRole(Conference conference, PressRole.EPressCategories category, string roleName)
        {
            var role = new PressRole {RoleName = roleName, Conference = conference, PressCategory = category};

            _context.PressRoles.Add(role);
            _context.SaveChanges();

            return role;
        }

        public IQueryable<PressRole> GetPressRoles(string conferenceId)
        {
            return _context.PressRoles.Where(n => n.Conference.ConferenceId == conferenceId);
        }

        public Participation Participate(MunityUser user, AbstractRole role)
        {
            var participation = new Participation {User = user, Role = role};

            _context.Participations.Add(participation);
            _context.SaveChanges();

            return participation;
        }

        public IQueryable<Participation> GetUserParticipations(MunityUser user)
        {
            return _context.Participations.Where(n => n.User == user);
        }

        public IQueryable<Participation> GetConferenceParticipations(string conferenceId)
        {
            var list = _context.Participations.Where(n => n.Role.Conference.ConferenceId == conferenceId);
            return list;
        }

        public IQueryable<AbstractRole> GetUserRolesOnConference(string username, string conferenceid)
        {
            return this._context.Participations.Where(n =>
                n.Role.Conference.ConferenceId == conferenceid && n.User.Username == username).Select(n => n.Role);
        }

        public async Task<bool> SetConferenceName(string conferenceid, string newname)
        {
            var conference = await this._context.Conferences.FirstOrDefaultAsync();
            if (conference == null) return false;
            conference.Name = newname;
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetConferenceFullName(string conferenceid, string newfullname)
        {
            var conference = await this._context.Conferences.FirstOrDefaultAsync();
            if (conference == null) return false;
            conference.FullName = newfullname;
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetConferenceAbbreviation(string conferenceid, string newabbreviation)
        {
            var conference = await this._context.Conferences.FirstOrDefaultAsync();
            if (conference == null) return false;
            conference.ConferenceShort = newabbreviation;
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetConferenceDate(string conferenceid, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) return false;
            var conference = await this._context.Conferences.FirstOrDefaultAsync();
            if (conference == null) return false;
            conference.StartDate = startDate;
            conference.EndDate = endDate;
            await this._context.SaveChangesAsync();
            return true;
        }

        public Task<bool> IsConferenceNameTaken(string name)
        {
            return this._context.Conferences.AnyAsync(n => n.Name.ToLower() == name.ToLower());
        }

        public Task<bool> IsConferenceFullNameTaken(string fullname)
        {
            return this._context.Conferences.AnyAsync(n => n.FullName.ToLower() == fullname.ToLower());
        }

        public IEnumerable<Conference> FindPublicConferencesByName(string name)
        {
            name = name.ToLower();
            return this._context.Conferences.Include(n => n.ConferenceProject)
                .Include(n => n.Committees)
                .Where(n => n.Visibility == Conference.EConferenceVisibilityMode.Public &&
                                                        (n.Name.ToLower().Contains(name) ||
                                                         n.FullName.ToLower().Contains(name) ||
                                                         n.ConferenceShort.ToLower().Contains(name)));
        }

        public IEnumerable<Project> FindProjectsByName(string name)
        {
            name = name.ToLower();
            return this._context.Projects.
                Include(n => n.ProjectOrganization)
                .Include(n => n.Conferences)
                .Where(n => n.ProjectName.ToLower().Contains(name));
        }

        public async Task<ConferenceInformation> GetConferenceInformation(string conferenceId)
        {
            var conference = await this._context.Conferences.Include(n => n.Committees).Include(n => n.ConferenceProject)
                .FirstOrDefaultAsync(a => a.ConferenceId == conferenceId);
            if (conference == null) return null;
            return conference;
        }

        public bool AddRoleApplication(RoleApplication application)
        {
            this._context.RoleApplications.Add(application);
            this._context.SaveChanges();
            return true;
        }

        public ConferenceService(MunityContext context)
        {
            this._context = context;
            Console.WriteLine("Conference-Service Started!");
        }
    }
}
