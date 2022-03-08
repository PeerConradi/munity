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
    [Order(2300)]
    public void TestAddMUNSH2022Conference()
    {
        var tonyStark = _context.Users.FirstOrDefault(n => n.UserName == "tonystark");
        Assert.NotNull(tonyStark);
        var conference = _context.Fluent.Conference.AddConference(options =>
            options.WithShort("MUN-SH 22")
                .WithName("Model United Nations Schleswig-Holstein 2022")
                .WithFullName("Model United Nations Schleswig-Holstein 2022")
                .WithProject("munsh")
                .WithStartDate(new DateTime(2022, 3, 10))
                .WithEndDate(new DateTime(2022, 3, 14))
                .WithBasePrice(70m)
                .ByUser(tonyStark));
        Assert.NotNull(conference);
        Assert.IsTrue(_context.Conferences.Any(n => n.ConferenceId == "munsh22"));
        Assert.IsTrue(_context.Conferences.Any(n => n.Name == "Model United Nations Schleswig-Holstein 2022"));
        Assert.IsTrue(_context.Conferences.Any(n => n.FullName == "Model United Nations Schleswig-Holstein 2022"));
    }

   [Test]
    [Order(2301)]
    public void TestAddMUNBS2022GVAndHA3()
    {
        _context.Fluent.ForConference("munsh22").AddCommittee(gv =>
            gv
                .WithName("Generalversammlung")
                .WithFullName("Generalversammlung")
                .WithShort("GV")
                .WithType(CommitteeTypes.Online)
                .WithTopic("Gerechte Verteilung von Ressourcen zur Bekämpfung ansteckender Krankheiten")
                .WithTopic("Umgang mit Dual-Use Gütern")

                .WithSubCommittee(ha3 => ha3.WithName("Hauptausschuss 3")
                    .WithFullName("Ausschuss für soziale, humanitäre und kulturelle Fragen")
                    .WithShort("HA3")
                    .WithType(CommitteeTypes.Online)
                    .WithTopic("Gewährleistung des Zugangs zu Bildung")
                    .WithTopic("Sport als Beitrag zu Frieden und nachhaltiger Entwicklung")
                    .WithTopic("Zusammenarbeit mit lokalen Akteur*innen in der Konfliktprävention")));
        var recaff = _context.SaveChanges();
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munsh22-gv"));
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munsh22-ha3"));
        Assert.AreEqual(2, _context.Committees.Where(n => n.Conference.ConferenceId == "munsh22").Count());
    }

    [Test]
    [Order(2302)]
    public void TestAddMUNSH2022WiSoAndSEK()
    {
        _context.Fluent.ForConference("munsh22").AddCommittee(gv =>
            gv
                .WithName("Wirtschafts- und Sozialrat")
                .WithFullName("Wirtschafts- und Sozialrat")
                .WithShort("WiSo")
                .WithType(CommitteeTypes.Online)
                .WithTopic("Nachhaltige Gestaltung von städtischem Raum")
                .WithTopic("Umgang mit globalen Abfallströmen")

                .WithSubCommittee(ha3 => ha3.WithName("Sozialentwicklungskomission")
                    .WithFullName("Sozialentwicklungskomission")
                    .WithShort("SEK")
                    .WithType(CommitteeTypes.Online)
                    .WithTopic("Beendigung aller Formen von Armut")
                    .WithTopic("Maßnahmen zur Förderung nachhaltiger Konsummuster")
                    .WithTopic("Erreichung menschenwürdiger Arbeit für alle")));
        var recaff = _context.SaveChanges();
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munsh22-wiso"));
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munsh22-sek"));
        Assert.AreEqual(4, _context.Committees.Where(n => n.Conference.ConferenceId == "munsh22").Count());
    }

    [Test]
    [Order(2303)]
    public void TestAddMUNSH2022SRAndKFK()
    {
        _context.Fluent.ForConference("munsh22").AddCommittee(gv =>
            gv
                .WithName("Sicherheitsrat")
                .WithFullName("Sicherheitsrat")
                .WithShort("SR")
                .WithType(CommitteeTypes.Online)
                .WithTopic("Situation in Äthiopien")
                .WithTopic("Situation in Mali")
                .WithTopic("Zukunft der UN-Friedenssicherung")

                .WithSubCommittee(ha3 => ha3.WithName("Kommission für Friedenskonsolidierung")
                    .WithFullName("Kommission für Friedenskonsolidierung")
                    .WithShort("KFK")
                    .WithType(CommitteeTypes.Online)
                    .WithTopic("Langfristiger Frieden in Bergkarabach")
                    .WithTopic("Zusammenarbeit mit religiösen Akteur*innen in der Friedenskonsolidierung")
                    .WithTopic("Natürliche Ressourcen in der Friedenskonsolidierung")));
        var recaff = _context.SaveChanges();
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munsh22-sr"));
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munsh22-kfk"));
        Assert.AreEqual(6, _context.Committees.Where(n => n.Conference.ConferenceId == "munsh22").Count());
    }

    [Test]
    [Order(2304)]
    public void TestAddMUNSH2022MRR()
    {
        _context.Fluent.ForConference("munsh22").AddCommittee(kk => kk
            .WithName("Menschenrechtsrat")
            .WithFullName("Menschenrechtsrat")
            .WithShort("MRR")
            .WithType(CommitteeTypes.Online)
            .WithTopic("Umgang mit Gefangenen")
            .WithTopic("Die Menschenrechtssituation in Belarus")
            .WithTopic("Eindämmung von Menschenhandel"));
        Assert.AreEqual(7, _context.Committees.Where(n => n.Conference.ConferenceId == "munsh22").Count());
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munsh22-mrr"));
    }

    [Test]
    [Order(2305)]
    public void TestAddMUNSH2022UV()
    {
        _context.Fluent.ForConference("munsh22").AddCommittee(kk => kk
            .WithName("Umweltversammlung der Vereinten Nationen")
            .WithFullName("Umweltversammlung der Vereinten Nationen")
            .WithShort("UV")
            .WithType(CommitteeTypes.Online)
            .WithTopic("Gewährleistung nachhaltiger Landnutzung")
            .WithTopic("Schutz von Meeresökosystemen"));
        Assert.AreEqual(8, _context.Committees.Where(n => n.Conference.ConferenceId == "munsh22").Count());
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munsh22-uv"));
    }

    [Test]
    [Order(2305)]
    public void TestAddMUNSH2022Gruppen()
    {
        for (int i = 1; i < 7; i++)
        {
            _context.Fluent.ForConference("munsh22").AddCommittee(kk => kk
            .WithName($"Gruppe {i}")
            .WithFullName($"Gruppe {i}")
            .WithShort($"gr{i}")
            .WithType(CommitteeTypes.Online));
        }

        // EGO
        _context.Fluent.ForConference("munsh22").AddCommittee(kk => kk
            .WithName($"EGO")
            .WithFullName($"EGO")
            .WithShort($"ego")
            .WithType(CommitteeTypes.Online));

        Assert.AreEqual(15, _context.Committees.Where(n => n.Conference.ConferenceId == "munsh22").Count());
    }

}