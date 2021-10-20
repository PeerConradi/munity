using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Extensions
{
    public static class ApplicationFluentAPI
    {

        public static ConferenceApplicationBuilder AddApplication(this MunityContext context)
        {
            return new ConferenceApplicationBuilder(context);
        }


    }

    public class ConferenceApplicationBuilder
    {
        private MunityContext _context;

        public ApplicationBuilder WithConference(string conferenceId)
        {
            var applicationBuilder = new ApplicationBuilder(_context, conferenceId);
            return applicationBuilder;
        }

        public ConferenceApplicationBuilder(MunityContext context)
        {
            this._context = context;
        }
    }

    public class ApplicationBuilder
    {

        private MunityContext _context;

        private string _conferenceId;

        public DelegationApplicationBuilder DelegationApplication()
        {
            var builder = new DelegationApplicationBuilder(_context, _conferenceId);
            return builder;
        }

        public ApplicationBuilder(MunityContext context, string conferenceId)
        {
            this._context = context;
            this._conferenceId = conferenceId;
        }
    }

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
            throw new NotImplementedException();
        }

        public DelegationApplicationBuilder WithPreferedDelegation(string delegationId)
        {
            throw new NotImplementedException();
        }

        public DelegationApplicationBuilder WithPreferedDelegationByName(string name)
        {
            var preferedDelegation =
                _context.Delegations.FirstOrDefault(
                    n => n.Conference.ConferenceId == conferenceId && n.Name == name);

            if (preferedDelegation == null)
                throw new ArgumentException($"Unable to find a delegation with the name {name} inside the conference {conferenceId}");

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

        public DelegationApplication Build()
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
                Status = ApplicationStatuses.Writing
            };
        }
    }

}
