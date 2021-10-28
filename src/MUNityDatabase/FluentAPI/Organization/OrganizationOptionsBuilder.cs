using System;
using MUNity.Database.Context;
using MUNity.Database.Models.Organization;

namespace MUNity.Database.FluentAPI
{
    public class OrganizationOptionsBuilder
    {
        private MunityContext _context;

        public Organization Organization { get; }

        public OrganizationOptionsBuilder WithName(string name)
        {
            Organization.OrganizationName = name;
            return this;
        }

        public OrganizationOptionsBuilder WithShort(string shortName)
        {
            Organization.OrganizationShort = shortName;
            return this;
        }

        public OrganizationOptionsBuilder WithAdminRole()
        {
            var adminRole = new OrganizationRole();
            adminRole.Organization = this.Organization;
            this.Organization.Roles.Add(adminRole);
            adminRole.RoleName = "Admin";
            adminRole.CanCreateProject = true;
            adminRole.CanManageMembers = true;
            return this;
        }

        public OrganizationOptionsBuilder WithMemberRole(string roleName = "Member")
        {
            var memberRole = new OrganizationRole()
            {
                CanCreateProject = false,
                CanCreateRoles = false,
                CanManageMembers = false,
                Organization = this.Organization,
                RoleName = roleName
            };
            this.Organization.Roles.Add(memberRole);
            return this;
        }

        public OrganizationOptionsBuilder(MunityContext context)
        {
            this._context = context;
            this.Organization = new Organization();
        }

        public OrganizationOptionsBuilder WithProject(Action<ProjectOptionsBuilder> options)
        {
            var builder = new ProjectOptionsBuilder(this._context, this.Organization);
            options(builder);
            this.Organization.Projects.Add(builder.Project);
            return this;
        }

    }
}
