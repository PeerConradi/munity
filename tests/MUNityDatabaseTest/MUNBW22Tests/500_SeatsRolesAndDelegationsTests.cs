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

public partial class FullMUNBW22Tests
{
    [Test]
    [Order(500)]
    public void TestAddSeatsToGV()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-gv").AddSeatsByCountryNames(
            "Ägypten", "Algerien", "Angola", "Äthiopien",
            "Burkina Faso", "Elfenbeinküste", "Eritrea", "Gabun",
            "Ghana", "Kamerun", "Kenia", "Demokratische Republik Kongo",
            "Republik Kongo", "Libyen", "Madagaskar", "Mosambik",
            "Nigeria", "Ruanda", "Seychellen", "Südafrika",
            "Sudan", "Südsudan", "Tschad", "Tunesien",
            "Uganda", "Zentralafrikanische Republik",

            // Asien
            "Bangladesch", "Volksrepublik China", "Fidschi", "Indien",
            "Indonesien", "Iran", "Japan", "Jemen",
            "Kasachstan", "Katar", "Myanmar", "Pakistan",
            "Palau", "Papua-Neuguinea", "Philippinen", "Samoa",
            "Saudi-Arabien", "Singapur", "Südkorea", "Syrien",
            "Thailand", "Usbekistan", "Vereinigte Arabische Emirate", "Vietnam",
            "Zypern",

            // Osteuropa
            "Albanien", "Bosnien und Herzegowina", "Estland", "Kroatien",
            "Lettland", "Polen", "Rumänien", "Russland", "Ukraine",
            "Ungarn", "Belarus",

            // Lateinamerika
            "Argentinien", "Brasilien", "Chile", "Costa Rica",
            "Dominikanische Republik", "Ecuador", "Haiti", "Jamaika",
            "Kolumbien", "Kuba", "Mexiko", "Nicaragua",
            "Peru", "Trinidad und Tobago", "Uruguay", "Venezuela",

            // Westeuropa
            "Australien", "Deutschland", "Frankreich", "Irland",
            "Israel", "Italien", "Kanada", "Niederlande",
            "Norwegen", "Schweden", "Schweiz", "Türkei",
            "Vereinigte Staaten", "Vereinigtes Königreich"
            );

        Assert.AreEqual(92, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-gv"));
    }

    [Test]
    [Order(501)]
    public void TestAddSeatsToHA3()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-ha3").AddSeatsByCountryNames(
        "Ägypten", "Burkina Faso", "Ghana", "Kamerun", 
        "Nigeria", "Südafrika", "Zentralafrikanische Republik",

        // Asien
        "Volksrepublik China", "Indien", "Indonesien", "Pakistan",
        "Papua-Neuguinea", "Syrien", "Vietnam",

        // Osteuropa
        "Kroatien", "Polen", "Russland",

        // Lateinamerika
        "Argentinien", "Chile", "Peru",

        // Westeuropa
        "Frankreich", "Türkei", "Vereinigte Staaten", "Vereinigtes Königreich");


        Assert.AreEqual(24, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-ha3"));
    }

    [Test]
    [Order(502)]
    public void TestAddSeatsToSR()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-sr").AddSeatsByCountryNames(
            "Gabun", "Ghana", "Kenia",

        // Asien
        "Volksrepublik China", "Indien", "Vereinigte Arabische Emirate",

        // Osteuropa
        "Albanien", "Russland",

        // Lateinamerika
        "Brasilien", "Mexiko",

        // Westeuropa
        "Frankreich", "Irland", "Norwegen", "Vereinigte Staaten",
        "Vereinigtes Königreich");


        Assert.AreEqual(15, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-sr"));
    }

    [Test]
    [Order(503)]
    public void TestAddSeatsToKFK()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-kfk").AddSeatsByCountryNames(
        "Äthiopien", "Nigeria", "Südafrika",

        // Asien
        "Bangladesch", "Volksrepublik China", "Indien",  "Jemen", 
        "Zypern",

        // Osteuropa
        "Russland", "Ukraine",

        // Lateinamerika
        "Haiti", "Kolumbien", "Venezuela",

        // Westeuropa
        "Frankreich", "Kanada", "Norwegen", "Vereinigte Staaten", 
        "Vereinigtes Königreich");

        Assert.AreEqual(18, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kfk"));
    }

    [Test]
    [Order(504)]
    public void TestAddSeatsToWiSo()
    {
        // Afrika
        _context.Fluent.ForCommittee("munbw22-wiso").AddSeatsByCountryNames(
        "Ägypten", "Angola", "Äthiopien", "Elfenbeinküste", 
        "Demokratische Republik Kongo", "Libyen", "Nigeria",

        // Asien
        "Volksrepublik China", "Indien", "Indonesien", "Japan", 
        "Saudi-Arabien", "Singapur", "Thailand",

        // Osteuropa
        "Lettland", "Polen", "Russland", 

        // Lateinamerika
        "Brasilien", "Kuba", "Mexiko", "Nicaragua", 
        "Peru", "Uruguay",

        // Westeuropa
        "Deutschland", "Frankreich", "Niederlande", "Schweiz", 
        "Vereinigte Staaten", "Vereinigtes Königreich");



        Assert.AreEqual(29, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-wiso"));
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
        "Algerien", "Eritrea", "Libyen", "Ruanda",
        "Sudan", "Südsudan", "Uganda",

        // Asien
        "Bangladesch", "Volksrepublik China", "Iran", "Japan",
        "Jemen", "Myanmar", "Pakistan", "Usbekistan",

        // Osteuropa
        "Russland", "Ukraine", "Belarus",

        // Lateinamerika
        "Kolumbien", "Mexiko", "Nicaragua", "Venezuela",

        // Westeuropa
        "Deutschland", "Italien", "Kanada", "Türkei",
        "Vereinigte Staaten");


        Assert.AreEqual(27, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-iom"));
    }

    [Test]
    [Order(507)]
    public void TestAddSeatsToKK()
    {
        _context.Fluent.ForCommittee("munbw22-kk").AddSeatsByCountryNames(
            "Ägypten", "Äthiopien", "Elfenbeinküste", "Kamerun", 
            "Kenia", "Demokratische Republik Kongo", "Madagaskar", "Ruanda", 
            "Seychellen", "Südafrika", "Sudan", "Tschad", 
            "Tunesien",

            "Bangladesch", "Volksrepublik China", "Fidschi", "Indien",
            "Indonesien", "Japan", "Kasachstan",
            "Papua-Neuguinea", "Philippinen", "Samoa", "Saudi-Arabien",
            "Südkorea", "Thailand", "Vereinigte Arabische Emirate",

            "Estland", "Kroatien", "Polen", "Rumänien", 
            "Russland", "Ungarn",

            "Argentinien", "Brasilien", "Costa Rica", "Dominikanische Republik", 
            "Ecuador", "Haiti", "Mexiko", "Trinidad und Tobago", 

            "Australien", "Deutschland", "Frankreich",
            "Kanada", "Niederlande", "Schweden", "Vereinigte Staaten", 
            "Vereinigtes Königreich"
            );

        Assert.AreEqual(49, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kk"));
    }

    [Test]
    [Order(508)]
    public void TestAddSeatsToMRR()
    {
        _context.Fluent.ForCommittee("munbw22-mrr").AddSeatsByCountryNames(
            "Algerien", "Burkina Faso", "Elfenbeinküste", "Eritrea",
            "Libyen", "Sudan", "Tschad", "Zentralafrikanische Republik",

            "Bangladesch", "Volksrepublik China", "Indonesien", "Myanmar",
            "Nordkorea", "Pakistan", "Usbekistan", "Vereinigte Arabische Emirate",

            "Bosnien und Herzegowina", "Polen", "Russland", "Ungarn",

            "Brasilien", "Costa Rica", "Haiti", "Mexiko",
            "Venezuela",

            "Frankreich", "Niederlande", "Norwegen", "Vereinigte Staaten",
            "Vereinigtes Königreich");

        Assert.AreEqual(30, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-mrr"));
    }

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

    [Test]
    [Order(511)]
    public void TestCreateDelegations()
    {
        // Afrika
        // Save after every insert, otherwise the change-tracker that assignes the easy ids cant handle it...
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ägypten").WithCountry("Ägypten").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Algerien").WithCountry("Algerien").InsideCommitteeByShort("GV", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Algerien (online)").WithCountry("Algerien").InsideCommittee("munbw22-mrr").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Angola").WithCountry("Angola").InsideCommitteeByShort("GV", "WiSo").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Äthiopien").WithCountry("Äthiopien").InsideCommitteeByShort("GV", "KFK", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Burkina Faso").WithCountry("Burkina Faso").InsideCommitteeByShort("GV", "HA3").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Burkina Faso (online)").WithCountry("Burkina Faso").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Elfenbeinküste").WithCountry("Elfenbeinküste").InsideCommitteeByShort("GV", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Elfenbeinküsten (online)").WithCountry("Elfenbeinküste").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Eritrea").WithCountry("Eritrea").InsideCommitteeByShort("GV", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Eritrea (online)").WithCountry("Eritrea").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Gabun").WithCountry("Gabun").InsideCommitteeByShort("GV", "SR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ghana").WithCountry("Ghana").InsideCommitteeByShort("GV", "HA3", "SR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kamerun").WithCountry("Kamerun").InsideCommitteeByShort("GV", "HA3", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kenia").WithCountry("Kenia").InsideCommitteeByShort("GV", "SR", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Demokratische Republik Kongo").WithCountry("Demokratische Republik Kongo").InsideCommitteeByShort("GV", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Republik Kongo").WithCountry("Republik Kongo").InsideCommitteeByShort("GV").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Libyen").WithCountry("Libyen").InsideCommitteeByShort("GV", "WiSo", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Libyen (online)").WithCountry("Libyen").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Madagaskar").WithCountry("Madagaskar").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Mosambik").WithCountry("Mosambik").InsideCommitteeByShort("GV").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Nigeria").WithCountry("Nigeria").InsideCommitteeByShort("GV", "HA3", "KFK", "WiSo").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ruanda").WithCountry("Ruanda").InsideCommitteeByShort("GV", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Seychellen").WithCountry("Seychellen").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Südafrika").WithCountry("Südafrika").InsideCommitteeByShort("GV", "HA3", "KFK", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Sudan").WithCountry("Sudan").InsideCommitteeByShort("GV", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Sudan (online)").WithCountry("Sudan").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Südsudan").WithCountry("Südsudan").InsideCommitteeByShort("GV", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Tschad").WithCountry("Tschad").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Tschad (online)").WithCountry("Tschad").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Tunesien").WithCountry("Tunesien").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Uganda").WithCountry("Uganda").InsideCommitteeByShort("GV", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Zentralafrikanische Republik").WithCountry("Zentralafrikanische Republik").InsideCommitteeByShort("GV", "HA3").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Zentralafrikanische Republik (online)").WithCountry("Zentralafrikanische Republik").InsideCommitteeByShort("MRR").Save());

        //// Westeuropa
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Deutschland").WithCountry("Deutschland").InsideAnyCommittee().Save());

        //var allDelegations = _context.Delegations
        //    .Include(n => n.ChildDelegations)
        //    .Include(n => n.Roles)
        //    .Where(n => n.Conference.ConferenceId == "munbw22")
        //    .ToList();

        //Assert.AreEqual(93, allDelegations.Count);


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
    }

    [Test]
    [Order(512)]
    public void TestAddCostRuleForMrr()
    {
        _context.Fluent.ForCommittee("munbw22-mrr").AddCostRule(50, "Preis für ein Online Gremium");
        Assert.AreEqual(1, _context.ConferenceParticipationCostRules.Count());
    }

    //[Test]
    //[Order(513)]
    public void TestAddTeacherToDelegationDeutschland()
    {
        var deutschland =
            _context.Delegations.FirstOrDefault(n =>
                n.Name == "Deutschland" && n.Conference.ConferenceId == "munbw22");
        Assert.NotNull(deutschland);

        var teacher = new ConferenceDelegateRole()
        {
            Conference = _context.Conferences.Find("munbw22"),
            ApplicationState = EApplicationStates.Closed,
            Committee = null,
            DelegateCountry = null,
            DelegateType = "Lehrkraft",
            Delegation = deutschland,
            IsDelegationLeader = true,
            RoleName = "Lehrkraft",
            RoleFullName = "Lehrkraft"
        };

        _context.Delegates.Add(teacher);
        _context.SaveChanges();

        var hasLehrkraft = _context.Delegations.Any(n =>
            n.DelegationId == deutschland.DelegationId && n.Roles.Any(a => a.RoleName == "Lehrkraft"));
        Assert.IsTrue(hasLehrkraft);
    }

    [Test]
    [Order(514)]
    public void TestAddCostRuleForTeacher()
    {
        _context.Fluent.ForConference("munbw22").AddCostRuleForRolesOfSubType("Lehrkraft", 0, "Preis für Lehrkraft");
        //var lehrkraftRole = _context.Delegates.FirstOrDefault(n => n.RoleName == "Lehrkraft");

        //Assert.NotNull(lehrkraftRole);
        //var costs = _context.Fluent.ForConference("munbw22").GetCostOfRole(lehrkraftRole.RoleId);
        //Assert.AreEqual(0, costs);

    }
}
