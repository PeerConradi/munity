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
        p1.AllowedCommittees.Add(gv);
        p1.AllowedCommittees.Add(ha3);
        p1.AllowedCommittees.Add(wiso);
        p1.AllowedCommittees.Add(sek);
        p1.AllowedCommittees.Add(sr);
        p1.AllowedCommittees.Add(kfk);
        p1.AllowedCommittees.Add(mrr);
        p1.AllowedCommittees.Add(uv);
        

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
        p2.AllowedCommittees.Add(gv);
        p2.AllowedCommittees.Add(ha3);
        p2.AllowedCommittees.Add(wiso);
        p2.AllowedCommittees.Add(sek);
        p2.AllowedCommittees.Add(sr);
        p2.AllowedCommittees.Add(kfk);
        p2.AllowedCommittees.Add(mrr);
        p2.AllowedCommittees.Add(uv);

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
        p3.AllowedCommittees.Add(gv);
        p3.AllowedCommittees.Add(ha3);
        p3.AllowedCommittees.Add(wiso);
        p3.AllowedCommittees.Add(sek);
        p3.AllowedCommittees.Add(sr);
        p3.AllowedCommittees.Add(kfk);
        p3.AllowedCommittees.Add(mrr);
        p3.AllowedCommittees.Add(uv);


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
        p4.AllowedCommittees.Add(gv);
        p4.AllowedCommittees.Add(ha3);
        p4.AllowedCommittees.Add(wiso);
        p4.AllowedCommittees.Add(sek);
        p4.AllowedCommittees.Add(sr);
        p4.AllowedCommittees.Add(kfk);
        p4.AllowedCommittees.Add(mrr);
        p4.AllowedCommittees.Add(uv);

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
        p5.AllowedCommittees.Add(gv);
        p5.AllowedCommittees.Add(ha3);
        p5.AllowedCommittees.Add(wiso);
        p5.AllowedCommittees.Add(sek);
        p5.AllowedCommittees.Add(sr);
        p5.AllowedCommittees.Add(kfk);
        p5.AllowedCommittees.Add(mrr);
        p5.AllowedCommittees.Add(uv);

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
        p6.AllowedCommittees.Add(gv);
        p6.AllowedCommittees.Add(ha3);
        p6.AllowedCommittees.Add(wiso);
        p6.AllowedCommittees.Add(sek);
        p6.AllowedCommittees.Add(sr);
        p6.AllowedCommittees.Add(kfk);
        p6.AllowedCommittees.Add(mrr);
        p6.AllowedCommittees.Add(uv);

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
        p8.AllowedCommittees.Add(gv);
        p8.AllowedCommittees.Add(ha3);
        p8.AllowedCommittees.Add(wiso);
        p8.AllowedCommittees.Add(sek);
        p8.AllowedCommittees.Add(sr);
        p8.AllowedCommittees.Add(kfk);
        p8.AllowedCommittees.Add(mrr);
        p8.AllowedCommittees.Add(uv);

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
        p9.AllowedCommittees.Add(gv);
        p9.AllowedCommittees.Add(ha3);
        p9.AllowedCommittees.Add(wiso);
        p9.AllowedCommittees.Add(sek);
        p9.AllowedCommittees.Add(sr);
        p9.AllowedCommittees.Add(kfk);
        p9.AllowedCommittees.Add(mrr);
        p9.AllowedCommittees.Add(uv);

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
        p10.AllowedCommittees.Add(gv);
        p10.AllowedCommittees.Add(ha3);
        p10.AllowedCommittees.Add(wiso);
        p10.AllowedCommittees.Add(sek);
        p10.AllowedCommittees.Add(sr);
        p10.AllowedCommittees.Add(kfk);
        p10.AllowedCommittees.Add(mrr);
        p10.AllowedCommittees.Add(uv);

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
        p11.AllowedCommittees.Add(gv);
        p11.AllowedCommittees.Add(ha3);
        p11.AllowedCommittees.Add(wiso);
        p11.AllowedCommittees.Add(sek);
        p11.AllowedCommittees.Add(sr);
        p11.AllowedCommittees.Add(kfk);
        p11.AllowedCommittees.Add(mrr);
        p11.AllowedCommittees.Add(uv);

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
        p12.AllowedCommittees.Add(gv);
        p12.AllowedCommittees.Add(ha3);
        p12.AllowedCommittees.Add(wiso);
        p12.AllowedCommittees.Add(sek);
        p12.AllowedCommittees.Add(sr);
        p12.AllowedCommittees.Add(kfk);
        p12.AllowedCommittees.Add(mrr);
        p12.AllowedCommittees.Add(uv);

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
        p13.AllowedCommittees.Add(gv);
        p13.AllowedCommittees.Add(ha3);
        p13.AllowedCommittees.Add(wiso);
        p13.AllowedCommittees.Add(sek);
        p13.AllowedCommittees.Add(sr);
        p13.AllowedCommittees.Add(kfk);
        p13.AllowedCommittees.Add(mrr);
        p13.AllowedCommittees.Add(uv);

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
        p14.AllowedCommittees.Add(gv);
        p14.AllowedCommittees.Add(ha3);
        p14.AllowedCommittees.Add(wiso);
        p14.AllowedCommittees.Add(sek);
        p14.AllowedCommittees.Add(sr);
        p14.AllowedCommittees.Add(kfk);
        p14.AllowedCommittees.Add(mrr);
        p14.AllowedCommittees.Add(uv);

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
        p15.AllowedCommittees.Add(gv);
        p15.AllowedCommittees.Add(ha3);
        p15.AllowedCommittees.Add(wiso);
        p15.AllowedCommittees.Add(sek);
        p15.AllowedCommittees.Add(sr);
        p15.AllowedCommittees.Add(kfk);
        p15.AllowedCommittees.Add(mrr);
        p15.AllowedCommittees.Add(uv);


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
            Name = "Erste Setzung"
        };
        _context.CommitteeSessions.Add(session);
        Assert.AreEqual(1, _context.CommitteeSessions.Where(n => n.Committee.CommitteeId == "munsh22-gv").Count());
    }

    [Test]
    [Order(2821)]
    public void TestCreateAnAgendaItem()
    {
        var session = _context.CommitteeSessions.Include(n => n.AgendaItems).FirstOrDefault(n => n.Committee.CommitteeId == "munsh22-gv");
        Assert.NotNull(session);

        var agendaItem = new AgendaItem()
        {
            Name = "Test Tagesordnungspunkt"
        };
        session.AgendaItems.Add(agendaItem);
        _context.SaveChanges();
    }
}
