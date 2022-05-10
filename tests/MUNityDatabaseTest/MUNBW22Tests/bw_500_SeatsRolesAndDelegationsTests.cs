using Microsoft.EntityFrameworkCore;
using MUNity.Base;
using MUNity.Database.Models.Conference.Roles;
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
    [Order(500)]
    public void TestAddSeatsToGV()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-gv").AddSeatsByCountryNames(
            "Ägypten", "Albanien", "Äthiopien", "Australien",
            "Belarus", "Bosnien und Herzegowina", "Brasilien", "Chile",
            "Volksrepublik China", "Elfenbeinküste", "Deutschland", "Estland",
            "Fidschi", "Frankreich", "Ghana", "Indien",
            "Iran", "Irland", "Israel", "Italien",
            "Jamaika", "Japan", "Kanada", "Kolumbien",
            "Südkorea", "Kuba", "Mexiko", "Mosambik",
            "Niederlande", "Nigeria", "Norwegen", "Pakistan",
            "Polen", "Russland", "Saudi-Arabien", "Schweden",
            "Schweiz", "Südafrika", "Türkei", "Ukraine",
            "Uruguay", "Venezuela", "Vereinigte Arabische Emirate",
            "Vereinigtes Königreich", "Vereinigte Staaten"

            );

        Assert.AreEqual(45, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-gv"));
    }

    //[Test]
    //[Order(501)]
    //public void TestAddSeatsToHA3()
    //{
    //    // Afrika
    //    _context.Fluent.ForCommittee("munbw22-ha3").AddSeatsByCountryNames(
    //    "Ägypten", "Burkina Faso", "Ghana", "Kamerun", 
    //    "Nigeria", "Südafrika", "Zentralafrikanische Republik",

    //    // Asien
    //    "Volksrepublik China", "Indien", "Indonesien", "Pakistan",
    //    "Papua-Neuguinea", "Syrien", "Vietnam",

    //    // Osteuropa
    //    "Kroatien", "Polen", "Russland",

    //    // Lateinamerika
    //    "Argentinien", "Chile", "Peru",

    //    // Westeuropa
    //    "Frankreich", "Türkei", "Vereinigte Staaten", "Vereinigtes Königreich");


    //    Assert.AreEqual(24, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-ha3"));
    //}

    [Test]
    [Order(502)]
    public void TestAddSeatsToSR()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-sr").AddSeatsByCountryNames(
            "Albanien", "Brasilien", "Volksrepublik China", "Frankreich",
            "Ghana", "Indien", "Irland", "Mexiko",
            "Nigeria", "Norwegen", "Pakistan", "Russland",
            "Vereinigte Arabische Emirate", "Vereinigtes Königreich", "Vereinigte Staaten"
        );


        Assert.AreEqual(15, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-sr"));
    }

    [Test]
    [Order(503)]
    public void TestAddSeatsToKFK()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-kfk").AddSeatsByCountryNames(
            "Ägypten", "Äthiopien", "Volksrepublik China", "Frankreich",
            "Ghana", "Indien", "Kolumbien", "Nigeria",
            "Norwegen", "Polen", "Russland", "Südafrika",
            "Türkei", "Ukraine", "Venezuela", "Vereinigte Staaten",
            "Zypern");

        Assert.AreEqual(17, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kfk"));
    }

    [Test]
    [Order(504)]
    public void TestAddSeatsToWiSo()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-wiso").AddSeatsByCountryNames(
        "Ägypten", "Äthiopien", "Brasilien", "Volksrepublik China",
        "Elfenbeinküste", "Deutschland", "Frankreich", "Indien",
        "Japan", "Kuba", "Mexiko", "Niederlande",
        "Nigeria", "Polen", "Russland", "Saudi-Arabien",
        "Schweiz", "Uruguay", "Vereinigtes Königreich", "Vereinigte Staaten");
        _context.Fluent.ForCommittee("munbw22-wiso").AddNonGovernmentSeat("Greenpeace International", "GPI");
        _context.Fluent.ForCommittee("munbw22-wiso").AddNonGovernmentSeat("Human Rights Watch", "HRW");
        _context.Fluent.ForCommittee("munbw22-wiso").AddNonGovernmentSeat("Oxfam International", "OXI");
        _context.Fluent.ForCommittee("munbw22-wiso").AddNonGovernmentSeat("The Club of Rome International", "CRI");
        _context.Fluent.ForCommittee("munbw22-wiso").AddNonGovernmentSeat("UN Women", "UNW");


        Assert.AreEqual(25, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-wiso"));
    }

    //[Test]
    //[Order(505)]
    //public void TestAddSeatsToKBE()
    //{
    //    // Afrika
    //    _context.Fluent.ForCommittee("munbw22-kbe").AddSeatsByCountryNames(
    //        "Ägypten", "Äthiopien", "Burkina Faso", "Elfenbeinküste",
    //    "Demokratische Republik Kongo", "Libyen",

    //    // Asien
    //    "Bangladesch", "Volksrepublik China", "Indien", "Iran",
    //    "Japan", "Philippinen",

    //    // Osteuropa
    //    "Russland", "Ukraine",

    //    // Lateinamerika
    //    "Argentinien", "Haiti", "Kolumbien", "Mexiko",

    //    // Westeuropa
    //    "Australien", "Israel", "Niederlande", "Türkei",
    //    "Vereinigte Staaten");

    //    Assert.AreEqual(23, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kbe"));
    //}

    [Test]
    [Order(506)]
    public void TestAddSeatsToIOM()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-iom").AddSeatsByCountryNames(
            "Belarus", "Chile", "Volksrepublik China", "Deutschland",
            "Frankreich", "Iran", "Italien", "Kanada",
            "Mexiko", "Pakistan", "Russland", "Südafrika",
            "Türkei", "Ukraine", "Venezuela", "Vereinigtes Königreich",
            "Vereinigte Staaten");


        Assert.AreEqual(17, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-iom"));
    }

    [Test]
    [Order(507)]
    public void TestAddSeatsToKK()
    {
        _context.Fluent.ForCommittee("munbw22-kk").AddSeatsByCountryNames(
            "Ägypten", "Äthiopien", "Australien", "Brasilien",
            "Volksrepublik China", "Elfenbeinküste", "Deutschland", "Estland",
            "Fidschi", "Frankreich", "Indien", "Japan",
            "Kanada", "Südkorea", "Mexiko", "Niederlande",
            "Polen", "Russland", "Saudi-Arabien", "Schweden",
            "Südafrika", "Vereinigte Arabische Emirate", "Vereinigtes Königreich", "Vereinigte Staaten");

        Assert.AreEqual(24, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kk"));
    }

    //[Test]
    //[Order(508)]
    //public void TestAddSeatsToMRR()
    //{
    //    _context.Fluent.ForCommittee("munbw22-mrr").AddSeatsByCountryNames(
    //        "Algerien", "Burkina Faso", "Elfenbeinküste", "Eritrea",
    //        "Libyen", "Sudan", "Tschad", "Zentralafrikanische Republik",

    //        "Bangladesch", "Volksrepublik China", "Indonesien", "Myanmar",
    //        "Nordkorea", "Pakistan", "Usbekistan", "Vereinigte Arabische Emirate",

    //        "Bosnien und Herzegowina", "Polen", "Russland", "Ungarn",

    //        "Brasilien", "Costa Rica", "Haiti", "Mexiko",
    //        "Venezuela",

    //        "Frankreich", "Niederlande", "Norwegen", "Vereinigte Staaten",
    //        "Vereinigtes Königreich");

    //    Assert.AreEqual(30, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-mrr"));
    //}

    [Test]
    [Order(509)]
    public void TestAddNAs()
    {

    }

    [Test]
    [Order(510)]
    public void TestAddPress()
    {

    }

    //[Test]
    [Order(511)]
    public void TestCreateMUNBW2022Delegations()
    {
        // Afrika
        // Save after every insert, otherwise the change-tracker that assignes the easy ids cant handle it...
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ägypten").WithCountry("Ägypten").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Albanien").WithCountry("Albanien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Äthiopien").WithCountry("Äthiopien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Australien").WithCountry("Australien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Belarus").WithCountry("Belarus").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Bosnien und Herzegowina").WithCountry("Bosnien und Herzegowina").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Brasilien").WithCountry("Brasilien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Chile").WithCountry("Chile").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Volksrepublik China").WithCountry("Volksrepublik China").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Elfenbeinküste").WithCountry("Elfenbeinküste").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Deutschland").WithCountry("Deutschland").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Estland").WithCountry("Estland").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Fidschi").WithCountry("Fidschi").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Frankreich").WithCountry("Frankreich").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ghana").WithCountry("Ghana").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Haiti").WithCountry("Haiti").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Indien").WithCountry("Indien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Iran").WithCountry("Iran").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Irland").WithCountry("Irland").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Israel").WithCountry("Israel").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Italien").WithCountry("Italien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Jamaika").WithCountry("Jamaika").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Japan").WithCountry("Japan").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kanada").WithCountry("Kanada").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kolumbien").WithCountry("Kolumbien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Südkorea").WithCountry("Südkorea").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kuba").WithCountry("Kuba").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Mexiko").WithCountry("Mexiko").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Mosambik").WithCountry("Mosambik").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Niederlande").WithCountry("Niederlande").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Nigeria").WithCountry("Nigeria").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Norwegen").WithCountry("Norwegen").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Pakistan").WithCountry("Pakistan").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Polen").WithCountry("Polen").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Russland").WithCountry("Russland").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Saudi-Arabien").WithCountry("Saudi-Arabien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Schweden").WithCountry("Schweden").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Schweiz").WithCountry("Schweiz").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Südafrika").WithCountry("Südafrika").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Türkei").WithCountry("Türkei").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ukraine").WithCountry("Ukraine").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Uruguay").WithCountry("Uruguay").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Venezuela").WithCountry("Venezuela").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Vereinigte Arabische Emirate").WithCountry("Vereinigte Arabische Emirate").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Vereinigtes Königreich").WithCountry("Vereinigtes Königreich").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Vereinigte Staaten").WithCountry("Vereinigte Staaten").InsideAnyCommittee().Save());

      


        Assert.IsTrue(_context.Delegations.Any(n => n.DelegationId == "munbw22-deutschland"));
        // Check for all Delegations in Africa
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Ägypten"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Algerien"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Angola"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Äthiopien"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Burkina Faso"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Elfenbeinküste"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Eritrea"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Gabun"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Ghana"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Kamerun"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Kenia"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Demokratische Republik Kongo"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Republik Kongo"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Libyen"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Madagaskar"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Mosambik"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Nigeria"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Ruanda"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Seychellen"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Südafrika"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Sudan"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Südsudan"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Tschad"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Tunesien"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Uganda"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Zentralafrikanische Republik"));


        //Assert.AreEqual(284, _context.Delegates.Count(n => n.Conference.ConferenceId == "munbw22"));
        //Assert.AreEqual(122, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22"));
        //Assert.AreEqual(3, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 7), "Expected 3 delegations with size 7");
        //var sizeSixDelegationNames = _context.Delegations.Where(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 6).Select(n => n.Name).ToArray();
        //Assert.IsTrue(sizeSixDelegationNames.Contains("Indien"), "Missing Indien");
        //Assert.IsTrue(sizeSixDelegationNames.Contains("Frankreich"), "Missing Frankreich");
        //Assert.IsTrue(sizeSixDelegationNames.Contains("Vereinigtes Königreich"), "Missing Vereinigtes Königreich");
        //Assert.AreEqual(3, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 6), "Expected 3 delegations with size 6");
        //Assert.AreEqual(1, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 5), "Expected 1 delegations with size 6");
        //var sizeFourDelegations = _context.Delegations.Where(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 4).Select(n => n.Name).ToArray();
        //Assert.Contains("Ägypten", sizeFourDelegations);
        //Assert.Contains("Äthiopien", sizeFourDelegations);
        //Assert.Contains("Nigeria", sizeFourDelegations);
        //Assert.Contains("Südafrika", sizeFourDelegations);
        //Assert.Contains("Bangladesch", sizeFourDelegations);
        //Assert.Contains("Indonesien", sizeFourDelegations);
        //Assert.Contains("Japan", sizeFourDelegations);
        //Assert.Contains("Polen", sizeFourDelegations);
        //Assert.Contains("Brasilien", sizeFourDelegations);
        //Assert.Contains("Deutschland", sizeFourDelegations);
        //Assert.Contains("Kanada", sizeFourDelegations);
        //Assert.AreEqual(11, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 4), "Expected 11 delegations with the size 4");
        //Assert.AreEqual(25, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 3), "Expected 25 delegations with the size 3");
        //Assert.AreEqual(42, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 2), "Expected 42 delegations with the size 2");
        //Assert.AreEqual(37, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 1), "Expected 37 delegations with the size 1");
    }

    //[Test]
    //[Order(512)]
    //public void TestAddCostRuleForMrr()
    //{
    //    _context.Fluent.ForCommittee("munbw22-mrr").AddCostRule(50, "Preis für ein Online Gremium");
    //    Assert.AreEqual(1, _context.ConferenceParticipationCostRules.Count());
    //}

    //[Test]
    //[Order(513)]
    //public void TestAddTeacherToDelegationDeutschland()
    //{
    //    var deutschland =
    //        _context.Delegations.FirstOrDefault(n =>
    //            n.Name == "Deutschland" && n.Conference.ConferenceId == "munbw22");
    //    Assert.NotNull(deutschland);

    //    var teacher = new ConferenceDelegateRole()
    //    {
    //        Conference = _context.Conferences.Find("munbw22"),
    //        ApplicationState = EApplicationStates.Closed,
    //        Committee = null,
    //        DelegateCountry = null,
    //        DelegateType = "Lehrkraft",
    //        Delegation = deutschland,
    //        IsDelegationLeader = true,
    //        RoleName = "Lehrkraft",
    //        RoleFullName = "Lehrkraft"
    //    };

    //    _context.Delegates.Add(teacher);
    //    _context.SaveChanges();

    //    var hasLehrkraft = _context.Delegations.Any(n =>
    //        n.DelegationId == deutschland.DelegationId && n.Roles.Any(a => a.RoleName == "Lehrkraft"));
    //    Assert.IsTrue(hasLehrkraft);
    //}

    //[Test]
    //[Order(514)]
    //public void TestAddCostRuleForTeacher()
    //{
    //    _context.Fluent.ForConference("munbw22").AddCostRuleForRolesOfSubType("Lehrkraft", 0, "Preis für Lehrkraft");
    //    //var lehrkraftRole = _context.Delegates.FirstOrDefault(n => n.RoleName == "Lehrkraft");

    //    //Assert.NotNull(lehrkraftRole);
    //    //var costs = _context.Fluent.ForConference("munbw22").GetCostOfRole(lehrkraftRole.RoleId);
    //    //Assert.AreEqual(0, costs);

    //}
}
