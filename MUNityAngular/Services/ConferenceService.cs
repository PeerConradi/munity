using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;
using MySql.Data.MySqlClient;
using MUNityAngular.Models.User;
using MUNityAngular.DataHandlers.EntityFramework;
using System.Text.RegularExpressions;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Services
{
    public class ConferenceService
    {
        private MunityContext _context;

        //Create without cache
        //private List<DataHandlers.EntityFramework.Models.Conference> conferences = new List<DataHandlers.EntityFramework.Models.Conference>();


        public Conference GetConference(string id)
        {
            return _context.Conferences.FirstOrDefault(n => n.ConferenceId == id);   
        }

        
        public List<string> GetDelegationIdsOfCommittee(string committeeid)
        {
            return _context.CommitteeDelegations.Where(n => n.Committee.CommitteeId == committeeid).Select(n => n.Delegation.DelegationId).ToList();
        }

        #region General Conference Settings
        public bool CreateConference(Conference model, int? userid = null)
        {
            model.CreationDate = DateTime.Now;
            // Try to get the id from the abbreviation this makes for better URLs
            string customid = model.Abbreviation.ToLower();
            customid = customid.Replace(" ", "");
            var possible = Regex.IsMatch(customid, @"^[a-z0-9]+$");
            if (possible)
            {
                if (!_context.Conferences.Any(n => n.ConferenceId == customid))
                    model.ConferenceId = customid;
            }

            

            if (userid != null)
            {
                model.CreationUser = _context.Users.FirstOrDefault(n => n.UserId == userid);
            }

            _context.Conferences.Add(model);
            _context.SaveChanges();
            return true;
        }

        public void SetUserConferenceAuths(User user, Conference conference, ConferenceUserAuth auths)
        {
            var foundAuth = _context.ConferenceUserAuths.FirstOrDefault(n => n.User == user && n.Conference == conference);
            if (foundAuth == null)
            {
                auths.User = user;
                auths.Conference = conference;
                _context.ConferenceUserAuths.Add(auths);
            }
            _context.SaveChanges();
        }


        public void RemoveConference(Conference conference)
        {
            _context.Conferences.Remove(conference);
        }

        public void UpdateConference(Conference conference)
        {
            _context.Conferences.Update(conference);
            _context.SaveChanges();

        }
        #endregion


        #region Filter Conferences
        public List<string> GetNameOfAllConferences()
        {
            return _context.Conferences.Select(n => n.Name).ToList();
        }

        public List<Conference> GetAllConferencesOfAuth(string authkey)
        {
            var foundUser = _context.AuthKey.FirstOrDefault(n => n.AuthKeyValue == authkey)?.User;
            if (foundUser == null)
                return null;
            var conferences = _context.ConferenceUserAuths.Where(n => n.User == foundUser).Select(n => n.Conference);
            return conferences.ToList();
        }

        internal List<Conference> GetAll()
        {
            return _context.Conferences.ToList();
        }
        #endregion

        public bool CanAuthEditConference(string auth, string conferenceid)
        {
            var foundUser = _context.AuthKey.FirstOrDefault(n => n.AuthKeyValue == auth)?.User;
            if (foundUser == null)
                return false;
            return _context.ConferenceUserAuths.FirstOrDefault(n => n.User == foundUser && n.Conference.ConferenceId == conferenceid)?.CanEditSettings ?? false;

        }

        public List<User> UsersWithAccessToConference(string conferenceid)
        {
            return _context.ConferenceUserAuths.Where(n => n.Conference.ConferenceId == conferenceid).Select(n => n.User).ToList();
        }

        #region Committee
        public bool AddCommittee(Conference conference, Committee committee)
        {
            if (conference == null)
                throw new ArgumentNullException("The conference cannot be null!");

            if (!conference.Committees.Contains(committee))
            {
                conference.Committees.Add(committee);
                _context.Conferences.Update(conference);
                if (!_context.Committees.Contains(committee))
                {
                    _context.Committees.Add(committee);
                }
                _context.SaveChanges();
            }
            return true;
        }

        public Committee GetCommittee(string id)
        {
            return _context.Committees.FirstOrDefault(n => n.CommitteeId == id);
        }

        public void SetCommitteeStatus(CommitteeStatus status)
        {
            _context.CommitteeStatuses.Add(status);
            _context.SaveChanges();
        }

        public CommitteeStatus GetCommitteeStatus(Committee committee)
        {
            return _context.CommitteeStatuses.Where(n => n.Committee == committee).Aggregate((a, b) => a.Timestamp > b.Timestamp ? a : b);
        }

        #endregion

        #region Delegation

        public List<Delegation> GetAllDelegations()
        {
            return _context.Delegations.ToList();
        }

        public Delegation GetDelegation(string id)
        {
            return _context.Delegations.FirstOrDefault(n => n.DelegationId == id);
        }

        public Delegation CreateDelegation(string name,string fullname, string abbreviation, string type, string iconname)
        {
            var model = new Delegation();
            model.Abbreviation = abbreviation;
            model.Name = name;
            model.Type = type;
            model.FullName = fullname;
            model.IconName = iconname;
            _context.Delegations.Add(model);
            _context.SaveChanges();
            return model;
        }

        public List<Delegation> GetDelegationsOfCommittee(Committee committee)
        {
            return GetDelegationsOfCommittee(committee.CommitteeId);
        }

        public List<Delegation> GetDelegationsOfCommittee(string committeeid)
        {
            return _context.CommitteeDelegations.Where(n => n.Committee.CommitteeId == committeeid).Select(n => n.Delegation).ToList();
        }

        public bool AddDelegationToCommittee(Committee committee, Delegation delegation, int mincount, int maxcount)
        {
            var hasLink = _context.CommitteeDelegations.FirstOrDefault(n => n.Committee == committee && n.Delegation == delegation);
            if (hasLink != null)
            {
                hasLink.MinCount = mincount;
                hasLink.MaxCount = maxcount;
                _context.SaveChanges();
                return true;
            }
            var link = new CommitteeDelegation();
            link.Committee = committee;
            link.Delegation = delegation;
            link.MinCount = mincount;
            link.MaxCount = maxcount;
            _context.CommitteeDelegations.Add(link);
            _context.SaveChanges();
            return true;
        }

        public bool RemoveDelegationFromCommittee(Committee committee, Delegation delegation)
        {
            var val = _context.CommitteeDelegations.FirstOrDefault(n => n.Committee == committee && n.Delegation == delegation);
            if (val != null)
            {
                _context.CommitteeDelegations.Remove(val);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Delegation> GetDelegationsOfConference(Conference conference)
        {
            var list = new List<Delegation>();
            conference.Committees.ForEach(n =>
            {
                list.AddRange(_context.CommitteeDelegations.Where(a => a.Committee == n).Select(x => x.Delegation));
            });
            return list;
        }

        public List<Delegation> GetDelegationsOfConference(string conferenceid)
        {
            var conference = _context.Conferences.FirstOrDefault(n => n.ConferenceId == conferenceid);
            if (conference == null)
                return null;
            return GetDelegationsOfConference(conference);
        }

        #endregion

        #region Team

        public int AddTeamRole(TeamRole role)
        {
            _context.TeamRoles.Add(role);
            _context.SaveChanges();
            return Convert.ToInt32(role.TeamRoleId);
        }

        internal List<TeamRole> GetRolesOfConference(string conferenceid)
        {
            return _context.TeamRoles.Where(n => n.Conference.ConferenceId == conferenceid).ToList();
        }

        public void AddUserToConferenceTeam(User user, TeamRole role)
        {
            var teamUser = new TeamUser();
            teamUser.Role = role;
            teamUser.User = user;
            _context.TeamUsers.Add(teamUser);
            _context.SaveChanges();
        }

        public List<User> GetTeamUsers(Conference conference)
        {
            return _context.TeamUsers.Where(n => n.Role.Conference == conference).Select(n => n.User).ToList();
        }

        public List<TeamRole> GetUserTeamRolesAtConference(User user, Conference conference)
        {
            return _context.TeamUsers.Where(n => n.Role.Conference == conference && n.User == user).Select(n => n.Role).ToList();
        }

        public List<TeamRole> GetConferenceTeam(Conference conference)
        {
            return _context.TeamRoles.Where(n => n.Conference == conference).ToList();
        }

        public ConferenceUserAuth GetConferenceUserAuth(Conference conference, User user)
        {
            return _context.ConferenceUserAuths.FirstOrDefault(n => n.Conference == conference && n.User == user);
        }

        public ConferenceUserAuth GetConferenceUserAuth(string conferenceid, int userid)
        {
            return _context.ConferenceUserAuths.FirstOrDefault(n => n.Conference.ConferenceId == conferenceid && n.User.UserId == userid);
        }

        public int GetConferenceCount()
        {
            return _context.Conferences.Count();
        }

        #endregion

        public Conference GetConferenceOfCommittee(Committee committee)
        {
            return _context.Conferences.FirstOrDefault(n => n.Committees.Contains(committee));
        }

        public TeamRole GetTeamRole(int id)
        {
            return _context.TeamRoles.FirstOrDefault(n => n.TeamRoleId == id);
        }

        public ConferenceService(MunityContext context)
        {
            this._context = context;
            Console.WriteLine("Conference-Service Started!");
        }
    }
}
