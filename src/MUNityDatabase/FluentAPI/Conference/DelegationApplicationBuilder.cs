using MUNity.Base;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI
{

    public class DelegationApplicationBuilder
    {
        private DelegationApplication application = new DelegationApplication();

        private MunityContext _context;

        private string conferenceId;

        public DelegationApplicationBuilder WithAuthor(string username)
        {
            var user = _context.Users.FirstOrDefault(n => n.UserName == username);
            if (user == null)
                throw new ArgumentException($"No user with the username {username} was found.");

            var alreadyInsideUser = application.Users.FirstOrDefault(a => a.User.UserName == username);
            if (alreadyInsideUser != null)
            {
                alreadyInsideUser.Status = DelegationApplicationUserEntryStatuses.Joined;
                alreadyInsideUser.CanWrite = true;
            }
            else
            {
                application.Users.Add(new DelegationApplicationUserEntry()
                {
                    User = user,
                    Application = application,
                    Status = DelegationApplicationUserEntryStatuses.Joined,
                    CanWrite = true
                });
            }
            return this;
        }

        public DelegationApplicationBuilder WithMember(string username)
        {
            var user = _context.Users.FirstOrDefault(n => n.UserName == username);
            if (user == null)
                throw new ArgumentException($"No user with the username {username} was found.");

            var alreadyInsideUser = application.Users.FirstOrDefault(a => a.User.UserName == username);
            if (alreadyInsideUser == null)
            {
                application.Users.Add(new DelegationApplicationUserEntry()
                {
                    User = user,
                    Application = application,
                    Status = DelegationApplicationUserEntryStatuses.Invited,
                    CanWrite = true
                });
            }
            return this;
        }

        public DelegationApplicationBuilder WithPreferedDelegation(string delegationId)
        {
            throw new NotImplementedException($"WithPreferedDelegation is not implemented yet.");
        }

        public DelegationApplicationBuilder WithPreferedDelegationByName(string name)
        {
            var preferedDelegation =
                _context.Delegations.FirstOrDefault(
                    n => n.Conference.ConferenceId == conferenceId && n.Name == name);

            if (preferedDelegation == null)
                throw new ArgumentException($"Unable to find a delegation with the name {name} inside the conference {conferenceId}");

            var isAllowed = _context.Delegates.Any(n => n.Delegation.DelegationId == preferedDelegation.DelegationId &&
            n.ApplicationState == EApplicationStates.DelegationApplication);

            if (!isAllowed)
                throw new ApplicationTypeNotAllowedException($"No Role inside the Delegation {preferedDelegation.Name} for conference {conferenceId} accepts a Delegation Application.");

            application.DelegationWishes.Add(new DelegationApplicationPickedDelegation()
            {
                Delegation = preferedDelegation,
                Application = application,
                Priority = (byte)application.DelegationWishes.Count
            });
            return this;
        }

        public DelegationApplicationBuilder WithFieldInput(string fieldName, string value)
        {
            var field = _context.ConferenceApplicationFields.FirstOrDefault(n => n.FieldName == fieldName &&
            n.Forumula.FormulaType == ConferenceApplicationFormulaTypes.Delegation &&
            n.Forumula.Options.Conference.ConferenceId == conferenceId);
            if (field == null)
                throw new ArgumentException($"Unable to find the field with the name {fieldName} for the conference {conferenceId} searching byFormulaTypes Delegation");

            application.FormulaInputs.Add(new ConferenceDelegationApplicationFieldInput()
            {
                Value = value,
                Application = application,
                Field = field
            });
            return this;
        }

        public DelegationApplicationBuilder IsOpenedToPublic()
        {
            this.application.OpenToPublic = true;
            return this;
        }

        public DelegationApplication Submit()
        {
            _context.DelegationApplications.Add(this.application);
            _context.SaveChanges();
            return application;
        }

        public DelegationApplicationBuilder(MunityContext context, string conferenceId)
        {
            this._context = context;
            this.conferenceId = conferenceId;

            this.application = new DelegationApplication()
            {
                DelegationWishes = new List<DelegationApplicationPickedDelegation>(),
                Users = new List<DelegationApplicationUserEntry>(),
                FormulaInputs = new List<ConferenceDelegationApplicationFieldInput>(),
                ApplyDate = DateTime.Now,
                OpenToPublic = false,
                Status = ApplicationStatuses.Writing,
                Conference = context.Conferences.Find(conferenceId)
            };
        }
    }

}
