using Microsoft.EntityFrameworkCore;
using MUNity.Base;
using MUNity.Database.FluentAPI;
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
    [Order(300)]
    public void TestAddMUNBW2022Conference()
    {
        var tonyStark = _context.Users.FirstOrDefault(n => n.UserName == "tonystark");
        Assert.NotNull(tonyStark);
        var conference = _context.Fluent.Conference.AddConference(options =>
            options.WithShort("MUNBW 22")
                .WithName("Model United Nations Baden-Würrtemberg 2022")
                .WithFullName("Model United Nations Baden-Würrtemberg 2022")
                .WithProject("munbw")
                .WithStartDate(new DateTime(2022, 5, 12))
                .WithEndDate(new DateTime(2022, 5, 16))
                .WithBasePrice(70m)
                .ByUser(tonyStark));
        Assert.NotNull(conference);
        Assert.AreEqual(1, _context.Conferences.Count());
        Assert.IsTrue(_context.Conferences.Any(n => n.ConferenceId == "munbw22"));
        Assert.IsTrue(_context.Conferences.Any(n => n.Name == "Model United Nations Baden-Würrtemberg 2022"));
        Assert.IsTrue(_context.Conferences.Any(n => n.FullName == "Model United Nations Baden-Würrtemberg 2022"));
    }

    [Test]
    [Order(310)]
    public void TestAddGeneralversammlungUndHA3()
    {

        _context.Fluent.ForConference("munbw22").AddCommittee(gv =>
            gv
                .WithName("Generalversammlung")
                .WithFullName("Generalversammlung")
                .WithShort("GV")
                .WithType(CommitteeTypes.AtLocation)
                .WithTopic("Nachhaltige soziale und wirtschaftliche Entwicklung nach Covid-19")
                .WithTopic("Beschleunigung der Maßnahmen zur Umsetzung des Pariser Klimaabkommens"));

        var recaff = _context.SaveChanges();
        Assert.AreEqual(1, _context.Committees.Count());
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-gv"));
        Assert.AreEqual(2, _context.CommitteeTopics.Count());
    }



    [Test]
    [Order(311)]
    public void TestAddSicherheitsratUndKFK()
    {
        var conference = _context.Conferences.FirstOrDefault(n => n.ConferenceId == "munbw22");
        Assert.NotNull(conference);

        _context.Fluent.ForConference("munbw22").AddCommittee(sr => sr
            .WithName("Sicherheitsrat")
            .WithFullName("Sicherheitsrat")
            .WithShort("SR")
            .WithType(CommitteeTypes.AtLocation)
            .WithTopic("Aktuelle Situation in Afghanistan")
            .WithTopic("Nukleare Situation im Iran nach dem JCPOA")
            .WithTopic("Bedrohung des internationalen Friedens durch nichtstaatliche Akteure")

            .WithSubCommittee(kfk => kfk
                .WithName("Komission für Friedenskonsolidierung")
                .WithFullName("Komission für Friedenskonsolidierung")
                .WithShort("KFK")
                .WithType(CommitteeTypes.AtLocation)
                .WithTopic("Einsatz robuster Mandate in der Friedenssicherung")
                .WithTopic("Langfristiger Frieden in Zypern")
                .WithTopic("Internationale Kooperation in der Krisenprävention")));
        Assert.AreEqual(3, _context.Committees.Count());
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-sr"));
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kfk"));
    }

    [Test]
    [Order(312)]
    public void TestAddWiSoUndKBE()
    {
        _context.Fluent.ForConference("munbw22").AddCommittee(wiso => wiso
            .WithName("Wirtschafts- und Sozialrat")
            .WithFullName("Wirtschafts- und Sozialrat")
            .WithShort("WiSo")
            .WithType(CommitteeTypes.AtLocation)
            .WithTopic("Implementierung von Kreislaufwirtschaft zum Erreichen der nachhaltigen Entwicklungsziele")
            .WithTopic("Einführung einer globalen Mindeststeuer")

            //.WithSubCommittee(kbe => kbe
            //    .WithName("Komission für Bevölkerung und Entwicklung")
            //    .WithFullName("Komission für Bevölkerung und Entwicklung")
            //    .WithShort("KBE")
            //    .WithType(CommitteeTypes.AtLocation)
            //    .WithTopic("Maßnahmen zur Bekämpfung der Luftverschmutzung")
            //    .WithTopic("Tourismus und nachhaltige Entwicklung")
            //    .WithTopic("Resiliente und nachhaltige Landwirtschaft"))
            );
        Assert.AreEqual(4, _context.Committees.Count());
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-wiso"));
        //Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kbe"));
    }

    [Test]
    [Order(313)]
    public void TestAddRatderInternationalenOrganisation()
    {
        _context.Fluent.ForConference("munbw22").AddCommittee(rat => rat
            .WithName("Rat der Internationalen Organisation für Migration")
            .WithFullName("Rat der Internationalen Organisation für Migration")
            .WithShort("IOM")
            .WithType(CommitteeTypes.AtLocation)
            .WithTopic("Implementierung des UN-Migrationspakts")
            .WithTopic("Gesundheitsversorgung von Migrant*innen")
            .WithTopic("Umgang mit traumatisierten Geflüchteten"));
        Assert.AreEqual(5, _context.Committees.Count());
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-iom"));
    }

    [Test]
    [Order(314)]
    public void TestAddKlimakonferenz()
    {
        _context.Fluent.ForConference("munbw22").AddCommittee(kk => kk
            .WithName("Klimakonferenz")
            .WithFullName("Klimakonferenz")
            .WithShort("KK")
            .WithType(CommitteeTypes.AtLocation)
            .WithTopic("Rolle der Jugend bei der Umsetzung des Pariser Klimaabkommens")
            .WithTopic("Adaption an den Meeresspiegelanstieg in tiefliegenden Gebieten und Inselstaaten")
            .WithTopic("Umsetzung von SDG 7: Nachhaltige Energie für alle"));
        Assert.AreEqual(6, _context.Committees.Count());
        Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-kk"));
    }

    //[Test]
    //[Order(315)]
    //public void TestAddMenschenrechtsrat()
    //{
    //    _context.Fluent.ForConference("munbw22").AddCommittee(mrr => mrr
    //        .WithName("Menschenrechtsrat")
    //        .WithFullName("Menschenrechtsrat")
    //        .WithShort("MRR")
    //        .WithType(CommitteeTypes.Online)
    //        .WithTopic("Menschenrechtslage in der Republik Myanmar")
    //        .WithTopic("Bekämpfung der Diskriminierung aufgrund sexueller Orientierung und Identität")
    //        .WithTopic("Pressefreiheit und Schutz von Journalist*innen"));
    //    Assert.AreEqual(8, _context.Committees.Count());
    //    Assert.IsTrue(_context.Committees.Any(n => n.CommitteeId == "munbw22-mrr"));
    //}

    [Test]
    [Order(316)]
    public void TestConferenceHas6Committees()
    {
        var conference = _context.Conferences
            .Include(n => n.Committees)
            .FirstOrDefault(n => n.ConferenceId == "munbw22");

        Assert.AreEqual(6, conference.Committees.Count);
    }

    [Test]
    [Order(317)]
    public void TestCreateDashboardCard()
    {
        var card = new ConferenceDashboardCard()
        {
            Active = true,
            Conference = _context.Conferences.Find("munbw22"),
            ExternalWebsiteLink = null,
            LanguageCode = "de",
            ShowToRegistrationButton = true,
            ShowToWebsiteButton = true,
            Title = "Konferenz im Landtag Stuttgart",
            Text = "Diese Konferenz ist sehr gut."
        };

        _context.ConferenceDashboardCards.Add(card);
        _context.SaveChanges();

    }
}
