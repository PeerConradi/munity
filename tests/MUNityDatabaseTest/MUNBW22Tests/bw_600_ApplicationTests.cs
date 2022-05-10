using Microsoft.EntityFrameworkCore;
using MUNity.Base;
using MUNity.Database.Models.Conference;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullDMUN2022Tests
{
    [Test]
    [Order(600)]
    [Description("This Test should show how the application phase is created within a conference.")]
    public void TestSetupTheApplicationPhase()
    {
        var conference = _context.Conferences.Find("munbw22");
        Assert.NotNull(conference);

        var options = new ConferenceApplicationOptions()
        {
            Conference = conference,
            AllowCountryWishApplication = false,
            AllowDelegationApplication = true,
            AllowDelegationWishApplication = true,
            AllowRoleApplication = true,
            ApplicationEndDate = new DateTime(2022, 2, 20, 12, 0, 0),
            ApplicationStartDate = new DateTime(2021, 12, 12, 12, 0, 0),
            Formulas = new List<ConferenceApplicationFormula>(),
            IsActive = true
        };

        var formula = new ConferenceApplicationFormula()
        {
            Options = options,
            Title = "Anmeldung",
            RequiresAddress = true,
            RequiresName = true,
            MaxWishes = 3
        };

        formula.Fields = new List<ConferenceApplicationField>();
        formula.UserFields = new List<ConferenceDelegationApplicationUserUserField>();

        formula.Fields.Add(new ConferenceApplicationField()
        {
            IsRequired = true,
            DefaultValue = null,
            FieldName = "Motivation",
            FieldDescription = "Tragen Sie hier ein, warum Sie unbedingt diese Rolle haben möchten.",
            FieldType = ConferenceApplicationFieldTypes.MultiLineText,
            Forumula = formula
        });

        formula.Fields.Add(new ConferenceApplicationField()
        {
            IsRequired = true,
            DefaultValue = null,
            FieldName = "Erfahrung",
            FieldDescription = "Tragen Sie hier Ihre Erfahrung rund um das Thema Model United Nations und anderweitig ein.",
            FieldType = ConferenceApplicationFieldTypes.MultiLineText,
            Forumula = formula
        });

        formula.UserFields.Add(new ConferenceDelegationApplicationUserUserField()
        {
            FieldDescription = "Ernährungshinweis",
            FieldName = "Ernährungshinweis",
            FieldType= ConferenceApplicationFieldTypes.MultiLineText,
            DefaultValue = null,
            Forumula = formula,
            IsRequired = false
        });

        _context.ConferenceApplicationFormulas.Add(formula);
        _context.SaveChanges();
        Assert.IsTrue(_context.ConferenceApplicationFormulas.Any(n => n.Options.Conference.ConferenceId == "munbw22"));
        var formulaReload = _context.ConferenceApplicationFormulas
            .Include(n => n.Fields)
            .FirstOrDefault(n => n.Options.Conference.ConferenceId == "munbw22" &&
            n.FormulaType == ConferenceApplicationFormulaTypes.Delegation);

        Assert.NotNull(formulaReload);
        Assert.NotNull(formulaReload.Fields);
        Assert.AreEqual(2, formulaReload.Fields.Count);
        Assert.IsTrue(_context.ConferenceApplicationOptions.Any(n => n.ConferenceId == "munbw22"));
    }

    [Test]
    [Order(610)]
    [Description("This test sets the application phase on all the roles that are linked to a country inside the Delegation named Deutschland.")]
    public void TestSetApplicationStateOnDelegations()
    {
        var delegations = _context.Delegations
            .Include(n => n.Roles)
            .ThenInclude(n => n.DelegateCountry)
            .Where(n => n.Conference.ConferenceId == "munbw22");


        foreach (var delegation in delegations)
        {
            foreach (var role in delegation.Roles)
            {
                if (role.DelegateCountry is not null)
                    role.ApplicationState = EApplicationStates.DelegationApplication;
            }
        }

        var result = _context.SaveChanges();
    }

    //[Test]
    //[Order(620)]
    //[Description("This test show how a delegation Application can be created, that is targeting three different Delegations.")]
    //public void TestCreateDelegationApplication()
    //{

    //    var application = _context.Fluent
    //        .ForConference("munbw22")
    //        .CreateDelegationApplication()
    //        .WithAuthor(TestUsers.XMen.OriginalMembers.ProfessorX.UserName)
    //        .WithMember(TestUsers.XMen.OriginalMembers.Angel.UserName)
    //        .WithPreferedDelegationByName("Elfenbeinküste")
    //        .WithPreferedDelegationByName("Ghana")
    //        .WithPreferedDelegationByName("Kenia")
    //        .WithFieldInput("Motivation", "Wir sind sehr Motiviert bei dieser Konferenz dabei zu sein :)")
    //        .IsOpenedToPublic()
    //        .Submit();


    //    Assert.NotNull(application);
    //    Assert.AreEqual(1, _context.DelegationApplications.Count());

    //    var applications = _context.DelegationApplications
    //        .Where(n => n.DelegationWishes
    //            .Any(a => a.Delegation.Conference.ConferenceId == "munbw22"))
    //        .ToList();

    //    Assert.AreEqual(1, applications.Count, "Should only have one application by now");
    //    Assert.AreEqual(2, _context.DelegationApplicationUserEntries.Count(), "The application should have two users");
    //    Assert.AreEqual(3, _context.DelegationApplicationPickedDelegations.Count());
    //}

    //[Test]
    [Order(621)]
    [Description("This test should allow a user to become part of an application. This should only be allowed if the application is Open.")]
    public void TestUserCanApplyOnApplication()
    {
        var user = _context.Users.FirstOrDefault(n => n.UserName == TestUsers.XMen.OriginalMembers.Beast.UserName);
        Assert.NotNull(user, "User Beast not found but is needed for this test...");

        var application = _context.Fluent.ForConference("munbw22")
            .ApplicationsWithFreeSlots()
            .FirstOrDefault();

        Assert.NotNull(application, "Expected to have an application with a free slot but there is none. Make sure that in the previous test an application is created that does not fit the size of the committee it refers to.");

        var entry = new DelegationApplicationUserEntry()
        {
            User = user,
            Application = application,
            Status = DelegationApplicationUserEntryStatuses.RequestJoining,
            CanWrite = false,
            Message = "Hallo, ich bin Beast und möchte bei euch dabei sein :)."
        };

        _context.DelegationApplicationUserEntries.Add(entry);

        _context.SaveChanges();

        var reloadApplication = _context.DelegationApplications
            .Include(n => n.Users)
            .FirstOrDefault();
        Assert.AreEqual(3, reloadApplication.Users.Count);
    }
}
