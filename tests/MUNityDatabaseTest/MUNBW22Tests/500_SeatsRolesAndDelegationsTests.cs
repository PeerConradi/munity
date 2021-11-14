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
        // Ägypten
        var aegypten = _context.Fluent.ForConference("munbw22")
            .AddDelegation(options => options
                .WithName("Ägypten")
                .WithCountry("Ägypten")
                .InsideAnyCommittee()
                .Save());
        Assert.AreEqual(4, aegypten.Roles.Count);

        // Algerien
        var algerien = _context.Fluent.ForConference("munbw22")
            .AddDelegation(del => del
                .WithName("Algerien")
                .WithCountry("Algerien")
                .InsideCommitteeByShort("GV", "IOM")
                .Save());
        Assert.AreEqual(2, algerien.Roles.Count);

        var algerienOnline = _context.Fluent.ForConference("munbw22")
            .AddDelegation(del => del
            .WithName("Algerien (online)")
            .WithCountry("Algerien")
            .InsideCommittee("munbw22-mrr")
            .Save());
        Assert.AreEqual(1, algerienOnline.Roles.Count);

        // Angola
        var angola = _context.Fluent.ForConference("munbw22")
            .AddDelegation(options => options
                .WithName("Angola")
                .WithCountry("Angola")
                .InsideCommitteeByShort("GV", "WiSo")
                .Save());
        Assert.AreEqual(2, angola.Roles.Count);

        // Äthiopien
        var aetiopien = _context.Fluent.ForConference("munbw22")
            .AddDelegation(options => options
                .WithName("Äthiopien")
                .WithCountry("Äthiopien")
                .InsideCommitteeByShort("GV", "KFK", "WiSo", "KK")
                .Save());
        Assert.AreEqual(4, aetiopien.Roles.Count);

        // Burkina Faso
        var burkinaFaso = _context.Fluent.ForConference("munbw22")
            .AddDelegation(options => options
                .WithName("Burkina Faso")
                .WithCountry("Burkina Faso")
                .InsideCommitteeByShort("GV", "HA3")
                .Save());
        Assert.AreEqual(2, burkinaFaso.Roles.Count);

        var burkinaFasoOnline = _context.Fluent.ForConference("munbw22")
            .AddDelegation(options => options
                .WithName("Burkina Faso (online)")
                .WithCountry("Burkina Faso")
                .InsideCommitteeByShort("MRR")
                .Save());
        Assert.AreEqual(1, burkinaFasoOnline.Roles.Count);

        // Elfenbenküste
        var elfenbeimkueste = _context.Fluent.ForConference("munbw22")
            .AddDelegation(options => options
                .WithName("Elfenbeinküste")
                .WithCountry("Elfenbeinküste")
                .InsideCommitteeByShort("GV", "WiSo", "KK")
                .Save());

        var elfenbeinkuesteOnline = _context.Fluent.ForConference("munbw22")
            .AddDelegation(options => options
                .WithName("Elfenbeinküsten (online)")
                .WithCountry("Elfenbeinküste")
                .InsideCommitteeByShort("MRR")
                .Save());

        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Elfenbeinküste");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Eritrea");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Gabun");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ghana");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kamerun");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kenia");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Demokratische Republik Kongo");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Republik Kongo");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Libyen");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Madagaskar");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Mosambik");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Nigeria");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ruanda");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Seychellen");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Südafrika");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Sudan");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Südsudan");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Tschad");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Tunesien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Uganda");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Zentralafrikanische Republik");

        //// Asien
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Bangladesch");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Volksrepublik China");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Fidschi");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Indien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Indonesien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Iran");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Japan");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Jemen");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kasachstan");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Katar");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Myanmar");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Pakistan");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Palau");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Papua-Neuguinea");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Philippinen");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Samoa");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Saudi-Arabien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Singapur");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Südkorea");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Syrien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Thailand");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Usbekistan");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Vereinigte Arabische Emirate");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Vietnam");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Zypern");

        //// Osteuropa
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Albanien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Bosnien und Herzegowina");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Estland");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kroatien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Lettland");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Polen");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Rumänien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Russland");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ukraine");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ungarn");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Belarus");

        //// Lateinamerika
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Argentinien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Brasilien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Chile");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Costa Rica");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Dominikanische Republik");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Ecuador");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Haiti");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Jamaika");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kolumbien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kuba");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Mexiko");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Nicaragua");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Peru");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Trinidad und Tobago");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Uruguay");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Venezuela");

        //// Westeuropa
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Australien");
        var deutschland = _context.Fluent.ForConference("munbw22")
            .AddDelegation(options => options
                .WithName("Deutschland")
                .WithCountry("Deutschland")
                .InsideAnyCommittee()
                .Save());
        Assert.AreEqual(4, aetiopien.Roles.Count);
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Frankreich");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Irland");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Israel");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Italien");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Kanada");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Niederlande");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Norwegen");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Schweden");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Schweiz");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Türkei");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Vereinigte Staaten");
        //_context.Fluent.ForConference("munbw22").GroupRolesOfCountryIntoADelegation("Vereinigtes Königreich");



        //var allDelegations = _context.Delegations
        //    .Include(n => n.ChildDelegations)
        //    .Include(n => n.Roles)
        //    .Where(n => n.Conference.ConferenceId == "munbw22")
        //    .ToList();

        //Assert.AreEqual(93, allDelegations.Count);


        Assert.IsTrue(_context.Delegations.Any(n => n.DelegationId == "munbw22-deutschland"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Ägypten"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Algerien"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Angola"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Äthiopien"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Burkina Faso"));
        Assert.IsTrue(_context.Delegations.Any(n => n.Conference.ConferenceId == "munbw22" && n.Name == "Elfenbeinküste"));
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
