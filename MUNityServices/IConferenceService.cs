using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using MUNity.Schema.Conference;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.User;

namespace MUNity.Services
{
    public interface IConferenceService
    {
        Project CreateProject(string name, string abbreviation, Organization organization);

        Task<Project> GetProject(string id);

        Task<Project> GetFullProject(string id);

        Conference CreateConference(string name, string fullname, string abbreviation, Project project);

        Task<Conference> GetConference(string id);

        Committee CreateCommittee(Conference conference, string name, string fullname, string abbreviation);

        Task<Committee> GetCommittee(string id);

        public ConferenceTeamRole CreateLeaderRole(Conference conference);

        public ConferenceTeamRole CreateTeamRole(Conference conference, string roleName, ConferenceTeamRole parentRole = null,
            RoleAuth auth = null);

        public Participation Participate(MunityUser user, AbstractConferenceRole role);

        public IQueryable<AbstractConferenceRole> GetUserRolesOnConference(string username, string conferenceid);

        Task SaveDatabaseChanges();

        Task<bool> SetConferenceName(string conferenceid, string newname);

        Task<bool> IsConferenceNameTaken(string name);

        Task<bool> IsConferenceFullNameTaken(string fullname);

        Task<bool> SetConferenceFullName(string conferenceid, string newfullname);

        Task<bool> SetConferenceAbbreviation(string conferenceid, string abbreviation);

        Task<bool> SetConferenceDate(string conferenceid, DateTime startDate, DateTime endDate);

        IEnumerable<Conference> FindPublicConferencesByName(string name);
        IEnumerable<Project> FindProjectsByName(string name);

        Task<ConferenceInformation> GetConferenceInformation(string conferenceId);
    }
}
