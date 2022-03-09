using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Session;
using MUNity.Database.Models.Simulation;
using NUnit.Framework;

namespace MUNity.Database.Test.MUNBW22Tests;

public partial class FullDMUN2022Tests
{
    [Test]
    [Order(2800)]
    public void TestCreateGO()
    {
        var munsh = _context.Conferences.FirstOrDefault(n => n.ConferenceId == "munsh22");

        var go = new RuleOfProcedure()
        {
            Title = "Geschäftsordnung"
        };
        munsh.RuleOfProcedure = go;
        _context.SaveChanges();

        Assert.AreEqual(1, _context.RuleOfProcedures.Count());

    }

    [Test]
    [Order(2810)]
    public void TestAddPetitionTypes()
    {
        var go = _context.RuleOfProcedures.FirstOrDefault();

        var gv = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-gv");
        var ha3 = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-ha3");
        var wiso = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-wiso");
        var sek = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-sek");
        var sr = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-sr");
        var kfk = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-kfk");
        var mrr = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-mrr");
        var uv = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-uv");
        var gr1 = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-gr1");
        var gr2 = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-gr2");
        var gr3 = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-gr3");
        var gr4 = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-gr4");
        var gr5 = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-gr5");
        var gr6 = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-gr6");
        var ego = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-ego");

        List<Committee> committees = new List<Committee>();
        committees.Add(gv);
        committees.Add(ha3);
        committees.Add(wiso);
        committees.Add(sek);
        committees.Add(sr);
        committees.Add(kfk);
        committees.Add(mrr);
        committees.Add(uv);
        committees.Add(gr1);
        committees.Add(gr2);
        committees.Add(gr3);
        committees.Add(gr4);
        committees.Add(gr5);
        committees.Add(gr6);
        committees.Add(ego);

        // Persönliche Anträge

        var p1 = new PetitionType()
        {
            Category = "Persönlicher Antrag",
            SortOrder = 1,
            RuleOfProcedure = go,
            Description = "Für Fragen zur Geschäftsordnung oder zum Verfahren (z.B.zu Anträgen, Einreichen von Arbeitspapieren). Außerdem für Bitten(z.B.Fenster öffnen, Licht einschalten, lauter sprechen).",
            Name = "Recht auf Information",
            Ruling = Base.PetitionRulings.Chairs,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = true,
            AllowedCommittees = new List<Committee>()
        };

        committees.ForEach(p => p1.AllowedCommittees.Add(p));
        

        var p2 = new PetitionType()
        {
            Category = "Persönlicher Antrag",
            SortOrder = 2,
            RuleOfProcedure = go,
            Description = "Um Verfahrensfehler oder Verstöße gegen die Geschäftsordnung zur Sprache zu bringen.",
            Name = "Recht auf Wiederherstellung der Ordnung",
            Ruling = Base.PetitionRulings.Chairs,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = true,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p2.AllowedCommittees.Add(p));


        var p3 = new PetitionType()
        {
            Category = "Persönlicher Antrag",
            SortOrder = 3,
            RuleOfProcedure = go,
            Description = "Nur nach einer Erwiderung des Redners/der Rednerin auf eine eigene missverstandene und unbeantwortet gelassene Frage oder Kurzbemerkung möglich.",
            Name = "Recht auf Klärung eines Missverständnisses",
            Ruling = Base.PetitionRulings.Chairs,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = true,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p3.AllowedCommittees.Add(p));



        // Anträge an die Geschäftsordnung

        var p4 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 4,
            RuleOfProcedure = go,
            Description = "Abstimmung, bei der die Staaten in alphabetischer Reihenfolge aufgerufen werden und ihre Stimme verkünden. Nur bei knappen oder unklaren Ergebnissen möglich.",
            Name = "mündliche Abstimmung",
            Ruling = Base.PetitionRulings.Chairs,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = true,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p4.AllowedCommittees.Add(p));


        var p5 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 5,
            RuleOfProcedure = go,
            Description = "Entscheidungen des Vorsitzes können vorbehaltlich anderer Regelungen revidiert werden. Vor der Abstimmung soll der Vorsitz seine Entscheidung begründen.",
            Name = "Revision einer Entscheidung des Vorsitzes",
            Ruling = Base.PetitionRulings.TwoThirds,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p5.AllowedCommittees.Add(p));


        var p6 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 6,
            RuleOfProcedure = go,
            Description = "Der Vorsitz kann über diesen Antrag entscheiden",
            Name = "informelle Sitzung",
            Ruling = Base.PetitionRulings.simpleMajority,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = true,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p6.AllowedCommittees.Add(p));


        var p7 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 7,
            RuleOfProcedure = go,
            Description = "Der neue Tagesordnungspunkt wird unmittelbar behandelt. Der aktuelle Tagesordnungspunkt wird automatisch zum nächsten Tagesordnungspunkt.",
            Name = "Aufnahme eines neuen Tagesordnungspunktes",
            Ruling = Base.PetitionRulings.TwoThirdsPlusPermanentMembers,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        p7.AllowedCommittees.Add(sr);

        var p8 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 8,
            RuleOfProcedure = go,
            Description = "Der/die Antragsteller*in erklärt, welche Punkte beim verabschiedeten Resolutionsentwurf geändert werden sollen. Es können mehrere Anträge dieser Art angenommen werden.",
            Name = "Zurückschicken eines Resolutionsentwurfes",
            Ruling = Base.PetitionRulings.simpleMajority,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p8.AllowedCommittees.Add(p));


        var p9 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 9,
            RuleOfProcedure = go,
            Description = "Der aktuelle Tagesordnungspunkt wird an das Ende der Tagesordnung verschoben. Der / die Antragsteller *in muss denjenigen Tagesordnungspunkt nennen, mit dem das Gremium als nächstes fortfahren soll.",
            Name = "Vertagung eines Tagesordnungspunktes",
            Ruling = Base.PetitionRulings.simpleMajority,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p9.AllowedCommittees.Add(p));


        var p10 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 10,
            RuleOfProcedure = go,
            Description = "Es verfallen alle Resolutionsentwürfe und Änderungsanträge und die Allgemeine Debatte beginnt von Neuem.",
            Name = "Rückkehr zur Allgemeinen Debatte",
            Ruling = Base.PetitionRulings.TwoThirds,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p10.AllowedCommittees.Add(p));


        var p11 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 11,
            RuleOfProcedure = go,
            Description = "Die aktuelle Debatte wird sofort beendet und mit dem nächsten Verfahrensbestandteil fortgefahren.",
            Name = "Ende der aktuellen Debatte",
            Ruling = Base.PetitionRulings.TwoThirds,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p11.AllowedCommittees.Add(p));


        var p12 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 12,
            RuleOfProcedure = go,
            Description = "Sofortige Abstimmung über den Resolutionsentwurf in´seiner jetzigen Form. Es werden weder die ausstehenden Änderungsanträge behandelt noch erfolgt eine Abstimmung über die einzelnen operativen Absätze.",
            Name = "Vorgezogene Abstimmung über den Resolutionsentwurf als Ganzes",
            Ruling = Base.PetitionRulings.TwoThirds,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p12.AllowedCommittees.Add(p));


        var p13 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 13,
            RuleOfProcedure = go,
            Description = "Bezieht sich entweder auf die Redeliste für Redebeiträge oder auf die Redeliste für Fragen und Kurzbemerkungen. Der Vorsitz kann über diesen Antrag entscheiden.",
            Name = "Abschluss oder Wiedereröffnung der Redeliste",
            Ruling = Base.PetitionRulings.simpleMajority,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p13.AllowedCommittees.Add(p));


        var p14 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 14,
            RuleOfProcedure = go,
            Description = "Der Antrag kann sich sowohl auf die Redezeit für Redebeiträge als auch für Fragen und Kurzbemerkungen beziehen. Der Vorsitz kann über diesen Antrag entscheiden. ",
            Name = "Änderung der Redezeit",
            Ruling = Base.PetitionRulings.simpleMajority,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p14.AllowedCommittees.Add(p));


        var p15 = new PetitionType()
        {
            Category = "Antrag an die Geschäftsordnung",
            SortOrder = 15,
            RuleOfProcedure = go,
            Description = "Nur zum aktuellen Tagesordnungspunkt möglich",
            Name = "Anhörung eines Gastredners/ einer Gastrednerin",
            Ruling = Base.PetitionRulings.simpleMajority,
            AllowCountyDelegates = true,
            AllowNonCountryDelegates = false,
            AllowedCommittees = new List<Committee>()
        };
        committees.ForEach(p => p15.AllowedCommittees.Add(p));



        _context.PetitionTypes.Add(p1);
        _context.PetitionTypes.Add(p2);
        _context.PetitionTypes.Add(p3);

        _context.PetitionTypes.Add(p4);
        _context.PetitionTypes.Add(p5);
        _context.PetitionTypes.Add(p6);
        _context.PetitionTypes.Add(p7);
        _context.PetitionTypes.Add(p8);
        _context.PetitionTypes.Add(p9);
        _context.PetitionTypes.Add(p10);
        _context.PetitionTypes.Add(p11);
        _context.PetitionTypes.Add(p12);
        _context.PetitionTypes.Add(p13);
        _context.PetitionTypes.Add(p14);
        _context.PetitionTypes.Add(p15);

        _context.SaveChanges();

        Assert.AreEqual(15, _context.PetitionTypes.Count(), "Expected a petition type to be saved");
        Assert.AreEqual(14, _context.Committees.Where(n => n.CommitteeId == "munsh22-gv").Select(n => n.AllowedPetitionTypes.Count).FirstOrDefault(), "Expected the 'Generalversammlung' to know about this petition type.");

    }

    [Test]
    [Order(2820)]
    public void TestCreateASession()
    {
        var gv = _context.Committees.FirstOrDefault(n => n.CommitteeId == "munsh22-gv");
        var session = new CommitteeSession()
        {
            Committee = gv,
            StartDate = new DateTime(2022, 2, 2, 12, 0, 0),
            EndDate = new DateTime(2022, 2, 2, 19, 0, 0),
            Name = "Erster Sitzungsblock"
        };
        _context.CommitteeSessions.Add(session);
        gv.CurrentSession = session;
        _context.SaveChanges();
        Assert.AreEqual(1, _context.CommitteeSessions.Where(n => n.Committee.CommitteeId == "munsh22-gv").Count());
    }

    [Test]
    [Order(2821)]
    public void TestCreateAnAgendaItem()
    {
        var session = _context.CommitteeSessions.FirstOrDefault(n => n.Committee.CommitteeId == "munsh22-gv");
        Assert.NotNull(session);

        var agendaItem = new AgendaItem()
        {
            Name = "Test Tagesordnungspunkt"
        };
        session.CurrentAgendaItem = agendaItem;
        _context.SaveChanges();
        Assert.NotNull(_context.CommitteeSessions.Include(n => n.CurrentAgendaItem).FirstOrDefault(n => n.Committee.CommitteeId == "munsh22-gv").CurrentAgendaItem);
    }

    [Test]
    [Order(2822)]
    public void TestCreateAPetition()
    {
        var agendaItem = _context.AgendaItems.FirstOrDefault();
        var petitionType = _context.PetitionTypes.FirstOrDefault(n => n.AllowedCommittees.Any(a => a.CommitteeId == "munsh22-gv"));
        var role = _context.Delegates.FirstOrDefault(n => n.Committee.CommitteeId == "munsh22-gv");

        Assert.NotNull(agendaItem, "This tests needs an agenda item to create the petition on.");
        Assert.NotNull(petitionType, "This test needs an allowed petition type to use.");
        Assert.NotNull(role, "This test needs a role");
        var petition = new Petition()
        {
            AgendaItem = agendaItem,
            PetitionDate = DateTime.Now,
            PetitionType = petitionType,
            Status = Base.EPetitionStates.Unkown,
            Text = "",
            User = role
        };
        _context.Petitions.Add(petition);
        _context.SaveChanges();
        Assert.AreEqual(1, _context.Petitions.Count());
    }

    [Test]
    [Order(2824)]
    public void TestCreateAListOfSpeakersForMUNBWGV()
    {
        foreach(var committee in _context.Committees.Where(n => n.Conference.ConferenceId == "munsh22" && n.CommitteeId != "munsh22-gv"))
        {
            var session = new CommitteeSession()
            {
                Committee = committee,
                StartDate = new DateTime(2022, 4, 10, 9, 0, 0),
                EndDate = new DateTime(2022, 4, 13, 20, 0, 0),
                Name = "Sitzungsblock"
            };
            _context.CommitteeSessions.Add(session);
            committee.CurrentSession = session;

            var agendaItem = new AgendaItem()
            {
                Name = "Tagesordnungspunkt"
            };
            session.CurrentAgendaItem = agendaItem;
            _context.SaveChanges();
        }
        
    }
}
