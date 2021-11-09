using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MUNity.Database.Test.GeneralTests;

public class TablesCreatedTest : AbstractDatabaseTests
{
    public TablesCreatedTest() : base("databaseGeneralTest")
    {

    }

    [Test]
    public void TestHasCountries()
    {
        Assert.IsFalse(_context.Countries.Any());
    }

    [Test]
    public void TestHasCountryNameTranslations()
    {
        Assert.IsFalse(_context.CountryNameTranslations.Any());
    }

    [Test]
    public void TestHasOrganizations()
    {
        Assert.IsFalse(_context.Organizations.Any());
    }

    [Test]
    public void TestHasOrganizationRoles()
    {
        Assert.IsFalse(_context.OrganizationRoles.Any());
    }

    [Test]
    public void TestHasOrganizationMembers()
    {
        Assert.IsFalse(_context.OrganizationMembers.Any());
    }

    [Test]
    public void TestHasProjects()
    {
        Assert.IsFalse(_context.Projects.Any());
    }

    [Test]
    public void TestHasCommittees()
    {
        Assert.IsFalse(_context.Committees.Any());
    }

    [Test]
    public void TestHasDelegations()
    {
        Assert.IsFalse(_context.Delegations.Any());
    }

    [Test]
    public void TestHasTeamRoles()
    {
        Assert.IsFalse(_context.ConferenceTeamRoles.Any());
    }

    [Test]
    public void TestHasSecretaryGenerals()
    {
        Assert.IsFalse(_context.SecretaryGenerals.Any());
    }

    [Test]
    public void TestHasConferences()
    {
        Assert.IsFalse(_context.Conferences.Any());
    }

    [Test]
    public void TestHasTopics()
    {
        Assert.IsFalse(_context.CommitteeTopics.Any());
    }

    [Test]
    public void TestHasAttendanceStates()
    {
        Assert.IsFalse(_context.AttendanceStates.Any());
    }

    [Test]
    public void TestHasAttendanceChecks()
    {
        Assert.IsFalse(_context.AttendanceChecks.Any());
    }

    [Test]
    public void TestHasCommitteeSessions()
    {
        Assert.IsFalse(_context.CommitteeSessions.Any());
    }

    [Test]
    public void TestHasParticipations()
    {
        Assert.IsFalse(_context.Participations.Any());
    }

    [Test]
    public void TestHasConferenceRoleAuthorizations()
    {
        Assert.IsFalse(_context.ConferenceRoleAuthorizations.Any());
    }

    [Test]
    public void TestHasRoleApplication()
    {
        Assert.IsFalse(_context.RoleApplications.Any());
    }

    [Test]
    public void TestHasDelegationApplication()
    {
        Assert.IsFalse(_context.DelegationApplications.Any());
    }

    [Test]
    public void TestHasResolutionUsers()
    {
        Assert.IsFalse(_context.ResolutionUsers.Any());
    }

    [Test]
    public void TestHasListOfSpeakers()
    {
        Assert.IsFalse(_context.ListOfSpeakers.Any());
    }

    [Test]
    public void TestHasSpeakers()
    {
        Assert.IsFalse(_context.Speakers.Any());
    }

    [Test]
    public void TestHasListOfSpeakersLogs()
    {
        Assert.IsFalse(_context.ListOfSpeakersLogs.Any());
    }

    [Test]
    public void TestHasSettings()
    {
        Assert.IsFalse(_context.Settings.Any());
    }

    [Test]
    public void TestHasResolutions()
    {
        Assert.IsFalse(_context.Resolutions.Any());
    }

    [Test]
    public void TestHasPreambleParagraphs()
    {
        Assert.IsFalse(_context.PreambleParagraphs.Any());
    }

    [Test]
    public void TestHasOperativeParagraphs()
    {
        Assert.IsFalse(_context.OperativeParagraphs.Any());
    }

    [Test]
    public void TestHasResolutionSupporters()
    {
        Assert.IsFalse(_context.ResolutionSupporters.Any());
    }

    [Test]
    public void TestHasResolutionDeleteAmendments()
    {
        Assert.IsFalse(_context.ResolutionDeleteAmendments.Any());
    }

    [Test]
    public void TestHasResolutionChangeAmendments()
    {
        Assert.IsFalse(_context.ResolutionChangeAmendments.Any());
    }

    [Test]
    public void TestHasResolutionMoveAmendments()
    {
        Assert.IsFalse(_context.ResolutionMoveAmendments.Any());
    }

    [Test]
    public void TestHasResolutionAddAmendments()
    {
        Assert.IsFalse(_context.ResolutionAddAmendments.Any());
    }


}
