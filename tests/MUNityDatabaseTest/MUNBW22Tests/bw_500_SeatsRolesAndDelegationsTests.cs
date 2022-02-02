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
    public void TestCreateMUNBW2022Delegations()
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

        // Asien
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Bangladesch").WithCountry("Bangladesch").InsideCommitteeByShort("GV", "KFK", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Bangladesch (online)").WithCountry("Bangladesch").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Volksrepublik China").WithCountry("Volksrepublik China").InsideCommitteeByShort("GV", "HA3","SR", "KFK", "WiSo", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Volksrepublik China (online)").WithCountry("Volksrepublik China").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Fidschi").WithCountry("Fidschi").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Indien").WithCountry("Indien").InsideCommitteeByShort("GV", "HA3", "SR", "KFK", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Indonesien").WithCountry("Indonesien").InsideCommitteeByShort("GV", "HA3", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Indonesien (online)").WithCountry("Indonesien").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Iran").WithCountry("Iran").InsideCommitteeByShort("GV", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Japan").WithCountry("Japan").InsideCommitteeByShort("GV", "WiSo", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Jemen").WithCountry("Jemen").InsideCommitteeByShort("GV", "KFK", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kasachstan").WithCountry("Kasachstan").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Katar").WithCountry("Katar").InsideCommitteeByShort("GV").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Myanmar").WithCountry("Myanmar").InsideCommitteeByShort("GV", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Myanmar (online)").WithCountry("Myanmar").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Nordkorea (online)").WithCountry("Nordkorea").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Pakistan").WithCountry("Pakistan").InsideCommitteeByShort("GV", "HA3", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Pakistan (online)").WithCountry("Pakistan").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Palau").WithCountry("Palau").InsideCommitteeByShort("GV").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Papua-Neuguinea").WithCountry("Papua-Neuguinea").InsideCommitteeByShort("GV", "HA3", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Philippinen").WithCountry("Philippinen").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Samoa").WithCountry("Samoa").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Saudi-Arabien").WithCountry("Saudi-Arabien").InsideCommitteeByShort("GV", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Singapur").WithCountry("Singapur").InsideCommitteeByShort("GV", "WiSo").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Südkorea").WithCountry("Südkorea").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Syrien").WithCountry("Syrien").InsideCommitteeByShort("GV", "HA3").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Thailand").WithCountry("Thailand").InsideCommitteeByShort("GV", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Usbekistan").WithCountry("Usbekistan").InsideCommitteeByShort("GV", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Usbekistan (online)").WithCountry("Usbekistan").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Vereingite Arabische Emirate").WithCountry("Vereinigte Arabische Emirate").InsideCommitteeByShort("GV", "SR", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Vereingite Arabische Emirate (online)").WithCountry("Vereinigte Arabische Emirate").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Vietnam").WithCountry("Vietnam").InsideCommitteeByShort("GV", "HA3").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Zypern").WithCountry("Zypern").InsideCommitteeByShort("GV", "KFK").Save());

        // Osteuropa
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Albanien").WithCountry("Albanien").InsideCommitteeByShort("GV", "SR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Bosnien und Herzegowina").WithCountry("Bosnien und Herzegowina").InsideCommitteeByShort("GV").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Bosnien und Herzegowina (online)").WithCountry("Bosnien und Herzegowina").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Estland").WithCountry("Estland").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kroatien").WithCountry("Kroatien").InsideCommitteeByShort("GV", "HA3", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Lettland").WithCountry("Lettland").InsideCommitteeByShort("GV", "WiSo").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Polen").WithCountry("Polen").InsideCommitteeByShort("GV", "HA3", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Polen (online)").WithCountry("Polen").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Rumänien").WithCountry("Rumänien").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Russland").WithCountry("Russland").InsideCommitteeByShort("GV", "HA3", "SR", "KFK", "WiSo", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Russland (online)").WithCountry("Russland").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ukraine").WithCountry("Ukraine").InsideCommitteeByShort("GV", "KFK", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ungarn").WithCountry("Ungarn").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ungarn (online)").WithCountry("Ungarn").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Belarus").WithCountry("Belarus").InsideCommitteeByShort("GV", "IOM").Save());

        // Lateinamerika
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Argentinien").WithCountry("Argentinien").InsideCommitteeByShort("GV", "HA3", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Brasilien").WithCountry("Brasilien").InsideCommitteeByShort("GV", "SR", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Brasilien (online)").WithCountry("Brasilien").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Chile").WithCountry("Chile").InsideCommitteeByShort("GV", "HA3").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Costa Rica").WithCountry("Costa Rica").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Costa Rica (online)").WithCountry("Costa Rica").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Dominikanische Republik").WithCountry("Dominikanische Republik").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Ecuador").WithCountry("Ecuador").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Haiti").WithCountry("Haiti").InsideCommitteeByShort("GV", "KFK", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Haiti (online)").WithCountry("Haiti").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Jamaika").WithCountry("Jamaika").InsideCommitteeByShort("GV").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kolumbien").WithCountry("Kolumbien").InsideCommitteeByShort("GV", "KFK", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Kuba").WithCountry("Kuba").InsideCommitteeByShort("GV", "WiSo").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Mexiko").WithCountry("Mexiko").InsideCommitteeByShort("GV", "SR", "WiSo", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Mexiko (online)").WithCountry("Mexiko").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Nicaragua").WithCountry("Nicaragua").InsideCommitteeByShort("GV", "WiSo", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Peru").WithCountry("Peru").InsideCommitteeByShort("GV", "HA3", "WiSo").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Trinidad und Tobago").WithCountry("Trinidad und Tobago").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Uruguay").WithCountry("Uruguay").InsideCommitteeByShort("GV", "WiSo").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Venezuela").WithCountry("Venezuela").InsideCommitteeByShort("GV", "KFK", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(n => n.WithName("Venezuela (online)").WithCountry("Venezuela").InsideCommitteeByShort("MRR").Save());


        //// Westeuropa
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Australien").WithCountry("Australien").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Deutschland").WithCountry("Deutschland").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Frankreich").WithCountry("Frankreich").InsideCommitteeByShort("GV", "HA3", "SR", "KFK", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Frankreich (online)").WithCountry("Frankreich").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Irland").WithCountry("Irland").InsideCommitteeByShort("GV", "SR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Israel").WithCountry("Israel").InsideCommitteeByShort("GV").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Italien").WithCountry("Italien").InsideCommitteeByShort("GV", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Kanada").WithCountry("Kanada").InsideCommitteeByShort("GV", "KFK", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Niederlande").WithCountry("Niederlande").InsideCommitteeByShort("GV", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Niederlande (online)").WithCountry("Niederlande").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Norwegen").WithCountry("Norwegen").InsideCommitteeByShort("GV", "SR", "KFK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Norwegen (online)").WithCountry("Norwegen").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Schweden").WithCountry("Schweden").InsideCommitteeByShort("GV", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Schweiz").WithCountry("Schweiz").InsideCommitteeByShort("GV", "WiSo").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Türkei").WithCountry("Türkei").InsideCommitteeByShort("GV", "HA3", "IOM").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Vereinigte Staaten").WithCountry("Vereinigte Staaten").InsideCommitteeByShort("GV", "HA3", "SR", "KFK", "WiSo", "IOM", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Vereinigte Staaten (online)").WithCountry("Vereinigte Staaten").InsideCommitteeByShort("MRR").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Vereinigtes Königreich").WithCountry("Vereinigtes Königreich").InsideCommitteeByShort("GV", "HA3", "SR", "KFK", "WiSo", "KK").Save());
        _context.Fluent.ForConference("munbw22").AddDelegation(options => options.WithName("Vereinigtes Königreich (online)").WithCountry("Vereinigtes Königreich").InsideCommitteeByShort("MRR").Save());

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

        Assert.AreEqual(92, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-gv"), "Expected 92 people inside the GV");
        Assert.AreEqual(24, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-ha3"), "Expected 24 people inside the HA3");
        Assert.AreEqual(15, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-sr"), "Expected 15 people inside the SR");
        Assert.AreEqual(18, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kfk"), "Expected 18 people inside the KFK");
        Assert.AreEqual(29, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-wiso"), "Expected 29 people inside the WiSo");
        Assert.AreEqual(27, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-iom"), "Expected 27 people inside the IOM");
        Assert.AreEqual(49, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-kk"), "Expected 49 people inside the KK");
        Assert.AreEqual(30, _context.Delegates.Count(n => n.Committee.CommitteeId == "munbw22-mrr"), "Expected 30 people inside the MRR");

        Assert.AreEqual(284, _context.Delegates.Count(n => n.Conference.ConferenceId == "munbw22"));
        Assert.AreEqual(122, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22"));
        Assert.AreEqual(3, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 7), "Expected 3 delegations with size 7");
        var sizeSixDelegationNames = _context.Delegations.Where(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 6).Select(n => n.Name).ToArray();
        Assert.IsTrue(sizeSixDelegationNames.Contains("Indien"), "Missing Indien");
        Assert.IsTrue(sizeSixDelegationNames.Contains("Frankreich"), "Missing Frankreich");
        Assert.IsTrue(sizeSixDelegationNames.Contains("Vereinigtes Königreich"), "Missing Vereinigtes Königreich");
        Assert.AreEqual(3, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 6), "Expected 3 delegations with size 6");
        Assert.AreEqual(1, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 5), "Expected 1 delegations with size 6");
        var sizeFourDelegations = _context.Delegations.Where(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 4).Select(n => n.Name).ToArray();
        Assert.Contains("Ägypten", sizeFourDelegations);
        Assert.Contains("Äthiopien", sizeFourDelegations);
        Assert.Contains("Nigeria", sizeFourDelegations);
        Assert.Contains("Südafrika", sizeFourDelegations);
        Assert.Contains("Bangladesch", sizeFourDelegations);
        Assert.Contains("Indonesien", sizeFourDelegations);
        Assert.Contains("Japan", sizeFourDelegations);
        Assert.Contains("Polen", sizeFourDelegations);
        Assert.Contains("Brasilien", sizeFourDelegations);
        Assert.Contains("Deutschland", sizeFourDelegations);
        Assert.Contains("Kanada", sizeFourDelegations);
        Assert.AreEqual(11, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 4), "Expected 11 delegations with the size 4");
        Assert.AreEqual(25, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 3), "Expected 25 delegations with the size 3");
        Assert.AreEqual(42, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 2), "Expected 42 delegations with the size 2");
        Assert.AreEqual(37, _context.Delegations.Count(n => n.Conference.ConferenceId == "munbw22" && n.Roles.Count == 1), "Expected 37 delegations with the size 1");
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
