using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Conference.Roles;
using MUNityAngular.Models.Organisation;

namespace MUNityAngular.Services
{
    public interface IConferenceService
    {
        Project CreateProject(string name, string abbreviation, Organisation organisation);

        Task<Project> GetProject(string id);

        Conference CreateConference(string name, string fullname, string abbreviation, Project project);

        Task<Conference> GetConference(string id);

        Committee CreateCommittee(Conference conference, string name, string fullname, string abbreviation);

        Task<Committee> GetCommittee(string id);

        public TeamRole CreateLeaderRole(Conference conference);

        public TeamRole CreateTeamRole(Conference conference, string roleName, TeamRole parentRole = null,
            RoleAuth auth = null);

        public Participation Participate(Models.Core.User user, AbstractRole role);


    }
}
