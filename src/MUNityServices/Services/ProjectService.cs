using Microsoft.EntityFrameworkCore;
using MUNity.Database.Models.Conference;
using MUNity.Schema.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class ProjectService
    {
        private readonly MUNity.Database.Context.MunityContext context;

        public CreateProjectResponse CreateProject(CreateProjectModel model)
        {
            var response = new CreateProjectResponse();

            // TODO: Check user is allowed to create projects!

            var organization = context.Organizations.FirstOrDefault(n => n.OrganizationId == model.OrganizationId);
            if (organization == null)
            {
                response.Status = CreateProjectResponse.CreateProjectStatus.OrganizationNotFound;
                return response;
            }


            var project = new Project()
            {
                ProjectName = model.Name,
                ProjectShort = model.Short,
                ProjectOrganization = organization
            };

            string cleanedId = Util.IdGenerator.AsPrimaryKey(model.Short);
            if (context.Projects.All(n => n.ProjectId != cleanedId))
                project.ProjectId = cleanedId;

            context.Projects.Add(project);
            context.SaveChanges();
            response.ProjectId = project.ProjectId;
            return response;
        }

        public ProjectDashboardInfo GetDashboardInfo(string projectId)
        {
            return context.Projects.AsNoTracking()
                .Select(n => new ProjectDashboardInfo()
                {
                    Name = n.ProjectName,
                    Short = n.ProjectShort,
                    ProjectId = n.ProjectId,
                    Conferences = n.Conferences.Select(a => new ProjectDashboardConferenceInfo()
                    {
                        ConferenceId = a.ConferenceId,
                        EndDate = a.EndDate,
                        Name = a.Name,
                        FullName = a.FullName,
                        Short = a.ConferenceShort,
                        StartDate = a.StartDate,
                        CreationDate = a.CreationDate,
                        CreationUserUsername = a.CreationUser.UserName
                    }).ToList()
                })
                .FirstOrDefault(n => n.ProjectId == projectId);
        }

        public ProjectService(Database.Context.MunityContext context)
        {
            this.context = context;
        }
    }
}
