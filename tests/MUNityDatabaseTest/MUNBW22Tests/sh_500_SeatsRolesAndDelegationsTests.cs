using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Base;
using NUnit.Framework;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullDMUN2022Tests
{ 
    [Test]
    [Order(2500)]
    public void TestAddMUNSH2022SeatsToGV()
    {
        _context.Fluent.ForCommittee("munsh22-gv").AddSeatsByCountryNames(
            "Ägypten", "Bangladesch",
            "Brasilien", "Bolivien", "Botsuana", "Volksrepublik China",
            "Deutschland",
            "Fidschi",
            "Indien", "Indonesien", "Irland", "Italien",
            "Kanada", "Kasachstan", "Kolumbien", "Libyen",
            "Niederlande", "Polen", "Ruanda",
            "Salomonen", "Tschechische Republik", "Tunesien", "Ukraine",
             "Venezuela");

        _context.Fluent.ForCommittee("munsh22-gv").AddNonGovernmentSeat("Human rights watch", "HRW");
        _context.Fluent.ForCommittee("munsh22-gv").AddNonGovernmentSeat("World economic forum", "WEF");
        Assert.AreEqual(25, _context.Delegates.Where(n => n.Committee.CommitteeId == "munsh22-gv").Count());
    }

    [Test]
    [Order(2501)]
    public void TestAddMUNSH2022SeatsToHA3()
    {
        _context.Fluent.ForCommittee("munsh22-ha3").AddSeatsByCountryNames(
            "Armenien", "Äthiopien", "Bangladesch", "Kasachstan",
            "Kuba", "Libanon", "Mexiko", "Myanmar",
            "Russland", "Volksrepublik China", "Deutschland", "Sudan",
            "Türkei", "Zentralafrikanische Republik");

        _context.Fluent.ForCommittee("munsh22-ha3").AddNonGovernmentSeat("Save the Children", "StC");
        _context.Fluent.ForCommittee("munsh22-ha3").AddNonGovernmentSeat("UN Women", "UNW");
        Assert.AreEqual(16, _context.Delegates.Where(n => n.Committee.CommitteeId == "munsh22-ha3").Count());
    }

    [Test]
    [Order(2502)]
    public void TestAddMUNSH2022SeatsToWiSo()
    {
        _context.Fluent.ForCommittee("munsh22-wiso").AddSeatsByCountryNames(
            "Argentinien", "Australien", "Bangladesch", "Gabun",
            "Italien", "Japan", "Kasachstan", "Kroatien",
            "Madagaskar", "Nicaragua", "Portugal", "Salomonen",
            "Bolivien", "Botswana", "Volksrepublik China", "Indien",
            "Lettland", "Norwegen", "Oman", "Simbabwe",
            "Tansania", "Thailand", "Vereinigte Staaten", "Nigeria");

        //_context.Fluent.ForCommittee("munsh22-wiso").AddNonGovernmentSeat("International Institute for Sustainable Development", "IISD");
        //_context.Fluent.ForCommittee("munsh22-wiso").AddNonGovernmentSeat("World Wide Fund for Nature", "WWF");
        Assert.AreEqual(24, _context.Delegates.Where(n => n.Committee.CommitteeId == "munsh22-wiso").Count());
    }

    [Test]
    [Order(2503)]
    public void TestAddMUNSH2022SeatsToSEK()
    {
        _context.Fluent.ForCommittee("munsh22-sek").AddSeatsByCountryNames(
            "Brasilien", "Bulgarien", "Volksrepublik China", "Indien",
            "Iran", "Katar", "Kuba", "Libyen",
            "Nigeria", "Russland", "Südafrika", "Türkei",
            "Vereinigte Staaten"
            );

        //_context.Fluent.ForCommittee("munsh22-sek").AddNonGovernmentSeat("Weltbankgruppe", "WBG");
        //_context.Fluent.ForCommittee("munsh22-sek").AddNonGovernmentSeat("Welthungerhilfe", "WHH");
        Assert.AreEqual(13, _context.Delegates.Where(n => n.Committee.CommitteeId == "munsh22-sek").Count());
    }

    [Test]
    [Order(2504)]
    public void TestAddMUNSH2022SeatsToSR()
    {
        _context.Fluent.ForCommittee("munsh22-sr").AddSeatsByCountryNames(
            "Gabun", "Ghana", "Irland", "Mexiko",
            "Russland", "Brasilien", "Volksrepublik China", "Norwegen",
            "Vereinigte Arabische Emirate", "Vereinigtes Königreich", "Vereinigte Staaten");
        _context.Fluent.ForCommittee("munsh22-sr").AddNonGovernmentSeat("Welthungerhilfe", "WHH");
        Assert.AreEqual(12, _context.Delegates.Where(n => n.Committee.CommitteeId == "munsh22-sr").Count());
    }

    [Test]
    [Order(2505)]
    public void TestAddMUNSH2022SeatsToKFK()
    {
        _context.Fluent.ForCommittee("munsh22-kfk").AddSeatsByCountryNames(
            "Ägypten", "Albanien", "Äthiopien", "Bangladesch",
            "Brasilien", "Volksrepublik China", "Costa Rica", "Deutschland",
            "Frankreich", "Gabun", "Ghana", "Indien",
            "Irland", "Japan", "Kanada", "Kenia",
            "Kolumbien", "Südkorea", "Lettland", "Libanon",
            "Mexiko", "Niederlande", "Nigeria", "Norwegen",
            "Pakistan", "Peru", "Ruanda", "Russland",
            "Schweden", "Schweiz", "Slowakei", "Südafrika",
            "Thailand", "Vereinigte Arabische Emirate", "Vereinigtes Königreich", "Vereinigte Staaten"
            );

        //_context.Fluent.ForCommittee("munsh22-kfk").AddNonGovernmentSeat("Global Network of Women Peacebuilders", "gnwp");
        //_context.Fluent.ForCommittee("munsh22-kfk").AddNonGovernmentSeat("Islamic Relief Worldwide", "ISLAMIC");
        Assert.AreEqual(36, _context.Delegates.Where(n => n.Committee.CommitteeId == "munsh22-kfk").Count());
    }

    [Test]
    [Order(2506)]
    public void TestAddMUNSH2022SeatsMRR()
    {
        _context.Fluent.ForCommittee("munsh22-mrr").AddSeatsByCountryNames(
            "Armenien", "Bangladesch", "Italien", "Japan",
            "Kamerun", "Südkorea", "Libyen", "Malawi",
            "Mexiko", "Namibia", "Nepal", "Niederlande",
            "Philippinen", "Russland", "Bolivien", "Brasilien",
            "Burkina Faso", "Volksrepublik China", "Elfenbeinküste", "Deutschland",
            "Eritrea", "Fidschi", "Indien", "Indonesien",
            "Österreich", "Pakistan", "Somalia", "Sudan",
            "Tschechische Republik", "Ukraine", "Usbekistan", "Venezuela",
            "Vereinigtes Königreich");
        _context.Fluent.ForCommittee("munsh22-mrr").AddNonGovernmentSeat("Amnesty International", "AI");
        _context.Fluent.ForCommittee("munsh22-mrr").AddNonGovernmentSeat("Reporter ohne Grenzen", "RoG");
        Assert.AreEqual(35, _context.Delegates.Where(n => n.Committee.CommitteeId == "munsh22-mrr").Count());
    }

    [Test]
    [Order(2507)]
    public void TestAddMUNSH2022SeatsUV()
    {
        _context.Fluent.ForCommittee("munsh22-uv").AddSeatsByCountryNames(


            "Ägypten", "Albanien", "Australien", "Bangladesch",
            "Ghana", "Griechenland", "Madagaskar", "Nicaragua",
            "Papua-Neuguinea", "Brasilien", "Bulgarien", "Volksrepublik China",
            "Fidschi", "Haiti", "Indien", "Lettland",
            "Norwegen", "Südafrika", "Tansania", "Thailand",
            "Tunesien", "Türkei", "Venezuela", "Vereinigte Arabische Emirate",
            "Vereinigte Staaten", "Nigeria");
        //_context.Fluent.ForCommittee("munsh22-uv").AddNonGovernmentSeat("Friends of the Earth", "FoE");
        //_context.Fluent.ForCommittee("munsh22-uv").AddNonGovernmentSeat("Greenpeace International", "GP");
        Assert.AreEqual(26, _context.Delegates.Where(n => n.Committee.CommitteeId == "munsh22-uv").Count());
    }

    [Test]
    [Order(2508)]
    public void TestCreateGruppeAndEgo()
    {
        for (int i = 1; i < 7; i++)
        {
            _context.Fluent.ForCommittee($"munsh22-gr{i}").AddSeatsByCountryNames(
            "Afghanistan", "Ägypten", "Algerien", "Argentinien",
            "Bahrain", "Belgien", "Brasilien", "Volksrepublik China",
            "Dschibuti", "Frankreich", "Indonesien", "Italien",
            "Japan", "Kenia", "Kolumbien", "Mali",
            "Myanmar", "Norwegen", "Österreich", "Pakistan",
            "Philippinen", "Saudi-Arabien", "Singapur", "Vereinigtes Königreich",
            "Vereinigte Staaten"
            );
            _context.Fluent.ForCommittee($"munsh22-gr{i}").AddNonGovernmentSeat("IKRK", "IKRK");
            _context.Fluent.ForCommittee($"munsh22-gr{i}").AddNonGovernmentSeat("Save the Children", "STC");
        }

        _context.Fluent.ForCommittee($"munsh22-ego").AddSeatsByCountryNames(
            "Afghanistan", "Ägypten", "Algerien", "Argentinien",
            "Bahrain", "Belgien", "Brasilien", "Volksrepublik China",
            "Dschibuti", "Frankreich", "Indonesien", "Italien",
            "Japan", "Kenia", "Kolumbien", "Mali",
            "Myanmar", "Norwegen", "Österreich", "Pakistan",
            "Philippinen", "Saudi-Arabien", "Singapur", "Vereinigtes Königreich",
            "Vereinigte Staaten"
            );
        _context.Fluent.ForCommittee("munsh22-ego").AddNonGovernmentSeat("IKRK", "IKRK");
        _context.Fluent.ForCommittee("munsh22-ego").AddNonGovernmentSeat("Save the Children", "STC");
    }

    [Test]
    [Order(2510)]
    public void TestCreateMUNSH2022Delegations()
    {
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Ägypten").WithCountry("Ägypten").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Albanien").WithCountry("Albanien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Argentinien").WithCountry("Argentinien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Armenien").WithCountry("Armenien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Äthiopien").WithCountry("Äthiopien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Australien").WithCountry("Australien").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Bangladesch").WithCountry("Bangladesch").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Belarus").WithCountry("Belarus").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Benin").WithCountry("Benin").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Bolivien").WithCountry("Bolivien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Botswana").WithCountry("Botswana").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Brasilien").WithCountry("Brasilien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Bulgarien").WithCountry("Bulgarien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Burkina Faso").WithCountry("Burkina Faso").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Chile").WithCountry("Chile").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Volksrepublik China").WithCountry("Volksrepublik China").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Costa Rica").WithCountry("Costa Rica").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Elfenbeinküste").WithCountry("Elfenbeinküste").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Deutschland").WithCountry("Deutschland").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Ecuador").WithCountry("Ecuador").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Eritrea").WithCountry("Eritrea").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Eswatini").WithCountry("Eswatini").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Fidschi").WithCountry("Fidschi").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Frankreich").WithCountry("Frankreich").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Gabun").WithCountry("Gabun").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Ghana").WithCountry("Ghana").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Griechenland").WithCountry("Griechenland").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Haiti").WithCountry("Haiti").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Indien").WithCountry("Indien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Indonesien").WithCountry("Indonesien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Iran").WithCountry("Iran").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Irland").WithCountry("Irland").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Italien").WithCountry("Italien").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Jamaika").WithCountry("Jamaika").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Japan").WithCountry("Japan").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Jemen").WithCountry("Jemen").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Kamerun").WithCountry("Kamerun").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Kanada").WithCountry("Kanada").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Kasachstan").WithCountry("Kasachstan").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Katar").WithCountry("Katar").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Kenia").WithCountry("Kenia").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Kolumbien").WithCountry("Kolumbien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Demokratische Republik Kongo").WithCountry("Demokratische Republik Kongo").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Republik Korea").WithCountry("Südkorea").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Kroatien").WithCountry("Kroatien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Kuba").WithCountry("Kuba").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Lettland").WithCountry("Lettland").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Libanon").WithCountry("Libanon").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Libyen").WithCountry("Libyen").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Madagaskar").WithCountry("Madagaskar").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Malawi").WithCountry("Malawi").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Malaysia").WithCountry("Malaysia").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Mali").WithCountry("Mali").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Marshallinseln").WithCountry("Marshallinseln").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Mexiko").WithCountry("Mexiko").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Myanmar").WithCountry("Myanmar").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Namibia").WithCountry("Namibia").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Nepal").WithCountry("Nepal").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Nicaragua").WithCountry("Nicaragua").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Niederlande").WithCountry("Niederlande").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Nigeria").WithCountry("Nigeria").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Norwegen").WithCountry("Norwegen").InsideAnyCommittee().Save());

        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Oman").WithCountry("Oman").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Österreich").WithCountry("Österreich").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Pakistan").WithCountry("Pakistan").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Papua-Neuguinea").WithCountry("Papua-Neuguinea").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Peru").WithCountry("Peru").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Philippinen").WithCountry("Philippinen").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Polen").WithCountry("Polen").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Portugal").WithCountry("Portugal").InsideAnyCommittee().Save());
        
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Ruanda").WithCountry("Ruanda").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Russland").WithCountry("Russland").InsideAnyCommittee().Save());

        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Salomonen").WithCountry("Salomonen").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Schweden").WithCountry("Schweden").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Schweiz").WithCountry("Schweiz").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Seychellen").WithCountry("Seychellen").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Sierra Leone").WithCountry("Sierra Leone").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Simbabwe").WithCountry("Simbabwe").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Slowakei").WithCountry("Slowakei").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Somalia").WithCountry("Somalia").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Südafrika").WithCountry("Südafrika").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Sudan").WithCountry("Sudan").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Syrien").WithCountry("Syrien").InsideAnyCommittee().Save());

        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Tansania").WithCountry("Tansania").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Thailand").WithCountry("Thailand").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Togo").WithCountry("Togo").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Tschechien").WithCountry("Tschechien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Tunesien").WithCountry("Tunesien").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Türkei").WithCountry("Türkei").InsideAnyCommittee().Save());

        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Ukraine").WithCountry("Ukraine").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Uruguay").WithCountry("Uruguay").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Usbekistan").WithCountry("Usbekistan").InsideAnyCommittee().Save());

        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Venezuela").WithCountry("Venezuela").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Vereinigte Arabische Emirate").WithCountry("Vereinigte Arabische Emirate").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Vereinigtes Königreich").WithCountry("Vereinigtes Königreich").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Vereinigte Staaten").WithCountry("Vereinigte Staaten").InsideAnyCommittee().Save());
        _context.Fluent.ForConference("munsh22").AddDelegation(n => n.WithName("Zentralafrikanische Republik").WithCountry("Zentralafrikanische Republik").InsideAnyCommittee().Save());

        Assert.AreEqual(97, _context.Delegations.Where(n => n.Conference.ConferenceId == "munsh22").Count());
    }
}
