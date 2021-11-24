using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI
{
    public class DelegationBuilder
    {
        internal Delegation Delegation { get; private set; }

        private MunityContext _dbContext;

        private string _conferenceId;

        public DelegationBuilder WithName(string name)
        {
            // Handle easyId
            
            Delegation.Name = name;
            //SetIdAsEasyIdByName();
            return this;
        }

        private void SetIdAsEasyIdByName()
        {
            if (!string.IsNullOrWhiteSpace(Delegation.DelegationId)) return;

            var easyNameDelegation = Util.IdGenerator.AsPrimaryKey(Delegation.Name);
            var easyId = _conferenceId + "-" + easyNameDelegation;
            if (_dbContext.Delegations.All(n => n.DelegationId != easyId) && _dbContext.ChangeTracker.Entries<Delegation>().All(n => n.Entity.DelegationId != easyId))
            {
                Console.WriteLine($"Assign easy id to Delegation {Delegation.Name}: {easyId}");
                Delegation.DelegationId = easyId;
            }
            else
                Delegation.DelegationId = Guid.NewGuid().ToString();
        }

        public DelegationBuilderCommitteeSelector WithCountry(string countryName)
        {
            short countryId = _dbContext.Countries.AsNoTracking()
                .Where(n => n.Name == countryName || n.FullName == countryName)
                .Select(n => n.CountryId)
                .FirstOrDefault();
            if (countryId == 0)
                throw new CountryNotFoundException($"No country with the name {countryName} found!");

            return new DelegationBuilderCommitteeSelector(_dbContext, _conferenceId, countryId, Delegation);
        }

        public DelegationBuilder(MunityContext context, string conferenceId)
        {
            this._dbContext = context;
            this._conferenceId = conferenceId;
            var conference = _dbContext.Conferences.Find(conferenceId);

            if (conference == null)
                throw new ConferenceNotFoundException($"No conference with the id {conferenceId} found!");

            this.Delegation = new Delegation()
            {
                Conference = conference,
                Roles = new List<ConferenceDelegateRole>()
            };
        }
    }

    public class DelegationBuilderCommitteeSelector
    {

        private MunityContext _dbContext;

        private string _conferenceId;

        internal Delegation Delegation { get; private set; }

        private int _countryId;

        public BuildReadyDelegationBuilder InsideCommittee(params string[] committeeIds)
        {
                
            foreach(var committeeId in committeeIds)
            {
                // Find a fitting role
                var role = _dbContext.Delegates
                    .Where(n => n.Delegation == null &&
                    n.DelegateCountry.CountryId == _countryId &&
                    n.Committee.CommitteeId == committeeId)
                    .FirstOrDefault();
                if (role == null)
                {
                    var isCountryRepresentedInsideCommittee = _dbContext.Delegates.Any(n => n.DelegateCountry.CountryId == _countryId && n.Committee.CommitteeId == committeeId);
                    var msg = $"No fitting role to add was found for committee id: {committeeId} in conference {_conferenceId}, countryId: {_countryId}.";
                    if (!isCountryRepresentedInsideCommittee)
                        msg += $" The country {_dbContext.Countries.Find((short)_countryId)?.Name} is not represented inside the committee";
                    else
                        msg += $" The country seems to be represented inside the given committee, but it seems to already be assigned to delegation {_dbContext.Delegates.Where(n => n.DelegateCountry.CountryId == _countryId && n.Committee.CommitteeId == committeeId && n.Delegation != null).Select(n => n.Delegation.Name).FirstOrDefault()}";
                    throw new ConferenceRoleNotFoundException(msg);
                }
                else
                    Delegation.Roles.Add(role);

            }

            return new BuildReadyDelegationBuilder(this._dbContext, Delegation);
        }

        public BuildReadyDelegationBuilder InsideCommitteeByShort(params string[] committeeShorts)
        {

            foreach (var committeeShort in committeeShorts)
            {
                // Find a fitting role
                var role = _dbContext.Delegates
                    .Where(n => n.Delegation == null &&
                    n.DelegateCountry.CountryId == _countryId &&
                    n.Committee.CommitteeShort == committeeShort &&
                    n.Conference.ConferenceId == _conferenceId)
                    .FirstOrDefault();
                if (role == null)
                {
                    var isCountryRepresentedInsideCommittee = _dbContext.Delegates.Any(n => n.DelegateCountry.CountryId == _countryId && n.Committee.CommitteeShort == committeeShort);
                    var msg = $"No fitting role to add was found for committee short: {committeeShort} in conference {_conferenceId}, countryId: {_countryId}.";
                    if (!isCountryRepresentedInsideCommittee)
                        msg += $" The country {_dbContext.Countries.Find((short)_countryId)?.Name} is not represented inside the committee!";
                    else
                        msg = $"Unable to map a role for the country {_dbContext.Countries.Where(n => n.CountryId == _countryId).Select(n => $"{n.Name} ({n.CountryId})").FirstOrDefault()} seems to be represented inside the given committee, but it seems to already be assigned to delegation: {_dbContext.Delegates.Where(n => n.DelegateCountry.CountryId == _countryId && n.Committee.CommitteeShort == committeeShort && n.Delegation != null).Select(n => n.Delegation.Name).FirstOrDefault()}";
                    throw new ConferenceRoleNotFoundException(msg);
                }
                else
                    Delegation.Roles.Add(role);

            }

            return new BuildReadyDelegationBuilder(this._dbContext, Delegation);
        }

        public BuildReadyDelegationBuilder InsideAnyCommittee()
        {
            var roles = _dbContext.Delegates
                .Where(n => n.DelegateCountry.CountryId == _countryId &&
                n.Conference.ConferenceId == _conferenceId &&
                n.Delegation == null).ToList();
            Delegation.Roles = roles;
            return new BuildReadyDelegationBuilder(this._dbContext, Delegation);
        }

        public DelegationBuilderCommitteeSelector(MunityContext context, string conferenceId, int countryId, Delegation delegation)
        {
            this._dbContext = context;
            this._conferenceId = conferenceId;
            this.Delegation = delegation;
            this._countryId = countryId;
        }
    }

    public class BuildReadyDelegationBuilder
    {
        private MunityContext _context;

        public Delegation Delegation { get; private set; }

        public Delegation Save()
        {
            _context.Delegations.Add(Delegation);
            _context.SaveChanges();
            return Delegation;
        }

        public BuildReadyDelegationBuilder(MunityContext context, Delegation delegation)
        {
            this._context = context;
            this.Delegation = delegation;
        }
    }
}
