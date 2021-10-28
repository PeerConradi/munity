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
            Delegation.Name = name;
            return this;
        }

        public DelegationBuilderCommitteeSelector WithCountry(string countryName)
        {
            short? countryId = _dbContext.Countries.AsNoTracking()
                .Where(n => n.Name == countryName || n.FullName == countryName)
                .Select(n => n.CountryId)
                .FirstOrDefault();
            if (countryId == null)
                throw new CountryNotFoundException($"No country with the name {countryName} found!");

            return new DelegationBuilderCommitteeSelector(_dbContext, _conferenceId, countryId.Value, Delegation);
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

        public DelegationBuilderCommitteeSelector InsideCommittee(params string[] committeeIds)
        {
            var roles = _dbContext.Delegates.Where(n => n.Conference.ConferenceId == _conferenceId &&
            committeeIds.ToArray().Contains(n.Committee.CommitteeId) &&
            n.Delegation == null &&
            n.DelegateCountry.CountryId == _countryId);

            if (!roles.Any())
                throw new ConferenceRoleNotFoundException($"No fitting role to add was found for committee {committeeIds}, countryId: {_countryId}. Maybe the given role is already attached to another delegation.");

            foreach(var role in roles)
            {
                Delegation.Roles.Add(role);
            }
            return this;
        }

        public void InsideAnyCommittee()
        {
            var roles = _dbContext.Delegates
                .Where(n => n.DelegateCountry.CountryId == _countryId &&
                n.Conference.ConferenceId == _conferenceId &&
                n.Delegation == null).ToList();
            Delegation.Roles = roles;
        }

        public DelegationBuilderCommitteeSelector(MunityContext context, string conferenceId, int countryId, Delegation delegation)
        {
            this._dbContext = context;
            this._conferenceId = conferenceId;
            this.Delegation = delegation;
            this._countryId = countryId;
        }
    }
}
