using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI;

public class ProjectTools
{
    private MunityContext _dbContext;

    public Project AddProject(Action<ProjectOptionsBuilder> options)
    {
        var builder = new ProjectOptionsBuilder(_dbContext);
        options(builder);
        var project = builder.Project;
        _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();
        return project;
    }

    public ProjectTools(MunityContext context)
    {
        _dbContext = context;
    }
}
