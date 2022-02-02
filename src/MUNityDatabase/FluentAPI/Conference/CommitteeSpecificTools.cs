using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.General;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI;

public class CommitteeSpecificTools
{
    private MunityContext _dbContext;

    private string _committeeId;

    public ConferenceDelegateRole AddNonGovernmentSeat(string name, string iso, string authTypeName = "Participant")
    {
        var committee = _dbContext.Committees
            .Include(n => n.Conference)
            .FirstOrDefault(n => n.CommitteeId == _committeeId);

        var participantAuth = _dbContext.ConferenceRoleAuthorizations.FirstOrDefault(n =>
            n.RoleAuthName == authTypeName && n.Conference.ConferenceId == committee.Conference.ConferenceId);

        var role = new ConferenceDelegateRole()
        {
            Committee = committee,
            Conference = committee.Conference,
            ConferenceRoleAuth = participantAuth,
            RoleName = name,
            DelegateCountry = null,
            RoleFullName = name,
            DelegateType = "NA",
            RoleShort = iso
        };

        _dbContext.Delegates.Add(role);
        _dbContext.SaveChanges();
        return role;
    }

    public ConferenceDelegateRole AddSeatByCountryName(string countryName, string authTypeName = "Participant")
    {
        var committee = _dbContext.Committees
            .Include(n => n.Conference)
            .FirstOrDefault(n => n.CommitteeId == _committeeId);
        if (committee == null)
            throw new CommitteeNotFoundException($"The given Committee ({_committeeId}) was not found.");

        var country = _dbContext.Countries
            .FirstOrDefault(n => n.Name == countryName ||
            n.FullName == countryName);

        if (country == null)
        {
            country = _dbContext.CountryNameTranslations.Where(n => n.TranslatedFullName == countryName ||
                n.TranslatedName == countryName).Select(a => a.Country).FirstOrDefault();
        }

        if (country == null)
            throw new NullReferenceException($"No country with the name {countryName} was found...");

        var participantAuth = _dbContext.ConferenceRoleAuthorizations.FirstOrDefault(n =>
            n.RoleAuthName == authTypeName && n.Conference.ConferenceId == committee.Conference.ConferenceId);

        var role = new ConferenceDelegateRole()
        {
            Committee = committee,
            Conference = committee.Conference,
            ConferenceRoleAuth = participantAuth,
            RoleName = country.Name,
            DelegateCountry = country,
            RoleFullName = country.FullName,
            DelegateType = "Delegate",
            RoleShort = country.Iso
        };

        _dbContext.Delegates.Add(role);
        _dbContext.SaveChanges();
        return role;
    }

    /// <summary>
    /// Whenever you need to add multiple countries in bulk into the committee use this method instead of multiple:
    /// AddSeatByCountryName-Method calls. It will perform significantly faster.
    /// Not this method will use the AuthRole named: Participate. Make sure this auth exists otherwise it will assign
    /// null as RoleAuth. The DelegateType will be set to Delegate
    /// </summary>
    /// <param name="countryNames"></param>
    /// <returns></returns>
    public List<ConferenceDelegateRole> AddSeatsByCountryNames(params string[] countryNames)
    {
        var list = new List<ConferenceDelegateRole>();

        var committee = _dbContext.Committees
            .Include(n => n.Conference)
            .FirstOrDefault(n => n.CommitteeId == _committeeId);
        if (committee == null)
            throw new CommitteeNotFoundException($"The given Committee ({_committeeId}) was not found.");

        var nameArray = countryNames.ToList();

        var countries = _dbContext.Countries
            .Where(n => nameArray.Contains(n.FullName) || nameArray.Contains(n.Name)).Distinct().ToList();

        // TODO: Also search countries by their translation

        var participantAuth = _dbContext.ConferenceRoleAuthorizations.FirstOrDefault(n =>
            n.RoleAuthName == "Participate" && n.Conference.ConferenceId == committee.Conference.ConferenceId);

        foreach (var countryName in nameArray)
        {
            var fittingCountry = countries.FirstOrDefault(n => n.Name == countryName || n.FullName == countryName);

            if (fittingCountry == null)
                throw new NullReferenceException($"No country found for the name: {countryName}");

            var role = new ConferenceDelegateRole()
            {
                Committee = committee,
                Conference = committee.Conference,
                ConferenceRoleAuth = participantAuth,
                RoleName = fittingCountry.Name,
                DelegateCountry = fittingCountry,
                RoleFullName = fittingCountry.FullName,
                DelegateType = "Delegate",
                RoleShort = fittingCountry.Iso
            };

            _dbContext.Delegates.Add(role);
        }
        //if (country == null)
        //{
        //    country = _dbContext.CountryNameTranslations.Where(n => n.TranslatedFullName == countryName ||
        //        n.TranslatedName == countryName).Select(a => a.Country).FirstOrDefault();
        //}


        _dbContext.SaveChanges();
        return list;
    }

    public ConferenceDelegateRole AddSeat(string name, int? countryId = null, string shortName = null, string subTypeName = "Participant")
    {
        var committee = _dbContext.Committees.Include(n => n.Conference)
            .FirstOrDefault(n => n.CommitteeId == _committeeId);

        if (committee == null)
            throw new ArgumentException($"The committe with the given id {_committeeId} was not found!");

        var participantAuth = _dbContext.ConferenceRoleAuthorizations.FirstOrDefault(n =>
            n.RoleAuthName == subTypeName && n.Conference.ConferenceId == committee.Conference.ConferenceId);

        Country country = null;
        if (countryId.HasValue)
        {
            country = _dbContext.Countries.FirstOrDefault(n => n.CountryId == countryId);
            if (country == null)
                throw new ArgumentException($"The given country with id: {countryId} was not found!");
        }

        if (participantAuth == null)
            throw new ArgumentException($"The given authorization was not found!");

        var role = new ConferenceDelegateRole()
        {
            Committee = committee,
            Conference = committee.Conference,
            ConferenceRoleAuth = participantAuth,
            RoleName = name,
            DelegateCountry = country,
            RoleFullName = name,
            DelegateType = subTypeName,
            RoleShort = shortName
        };

        _dbContext.Delegates.Add(role);
        _dbContext.SaveChanges();
        return role;
    }

    public ConferenceParticipationCostRule AddCostRule(decimal cost, string name)
    {
        var committee = _dbContext.Committees.FirstOrDefault(n => n.CommitteeId == this._committeeId);
        var costRule = new ConferenceParticipationCostRule()
        {
            Committee = committee,
            Conference = null,
            Role = null,
            Delegation = null,
            AddPercentage = null,
            CostRuleTitle = name,
            Costs = cost,
            CutPercentage = null,
            UserMaxAge = null,
            UserMinAge = null
        };
        _dbContext.ConferenceParticipationCostRules.Add(costRule);
        _dbContext.SaveChanges();
        return costRule;
    }

    public CommitteeSpecificTools(MunityContext context, [NotNull] string committeeId)
    {
        this._dbContext = context;
        this._committeeId = committeeId;
    }
}
