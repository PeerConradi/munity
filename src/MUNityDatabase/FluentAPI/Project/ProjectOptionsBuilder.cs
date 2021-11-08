using System;
using System.Linq;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;

namespace MUNity.Database.FluentAPI;

public class ProjectOptionsBuilder
{
    public Project Project { get; }

    private readonly MunityContext _context;

    private bool _useEasyId = true;

    public ProjectOptionsBuilder WithName(string name)
    {
        this.Project.ProjectName = name;
        return this;
    }

    public ProjectOptionsBuilder WithShort(string shortName)
    {
        this.Project.ProjectShort = shortName;
        return this;
    }

    public ProjectOptionsBuilder WithCreationUser(MunityUser user)
    {
        this.Project.CreationUser = user;
        return this;
    }

    public ProjectOptionsBuilder WithOrganization(string organizationId)
    {
        var organization = _context.Organizations.FirstOrDefault(n => n.OrganizationId == organizationId);
        if (organization == null)
            throw new NullReferenceException("The Organization with the given Id cannot be found!");

        Project.ProjectOrganization = organization;
        return this;
    }

    public ProjectOptionsBuilder WithConference(Action<ConferenceOptionsBuilder> options)
    {
        var builder = new ConferenceOptionsBuilder(_context, Project);
        options(builder);
        var conference = builder.Conference;
        _context.Conferences.Add(conference);
        return this;
    }

    public ProjectOptionsBuilder(MunityContext context, Organization organization = null)
    {
        this.Project = new Project();
        this._context = context;
        this.Project.ProjectOrganization = organization;
    }
}
