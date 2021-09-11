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

        public ProjectService(Database.Context.MunityContext context)
        {
            this.context = context;
        }
    }
}
