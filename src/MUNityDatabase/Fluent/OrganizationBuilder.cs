using MUNity.Database.Context;
using MUNity.Database.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Fluent
{
    public class OrganizationBuilder : IOrganizationBuilder
    {
        private Organization organizationToCreate;

        private MunityContext context;

        public void Build()
        {
            context.Organizations.Add(organizationToCreate);
        }

        public void SetContext(MunityContext context)
        {
            this.context = context;
        }

        public void SetOrganization(Organization organization)
        {
            organizationToCreate = organization;
        }

        
    }

    public interface IOrganizationBuilder
    {

        void SetOrganization(Organization organization);

        void SetContext(MunityContext context);

        void Build();
    }

    public class OrganizationOptionsBuilder
    {
        public Organization Organization { get; private set; }

        public OrganizationOptionsBuilder()
        {
            Organization = new Organization();
        }

        public OrganizationOptionsBuilder WithName(string name)
        {
            Organization.OrganizationName = name;
            return this;
        }
    }

    public static class OrganizationBuilderExtensions
    {
        public static IOrganizationBuilder WithOrganization(this MunityContext context, Organization organization)
        {
            var builder = new OrganizationBuilder();
            builder.SetContext(context);
            builder.SetOrganization(organization);
            return builder;
        }

        public static IOrganizationBuilder WithOrganization(this MunityContext context, Action<OrganizationOptionsBuilder> optionsBuilder)
        {
            var builder = new OrganizationBuilder();
            builder.SetContext(context);
            var oBuilder = new OrganizationOptionsBuilder();
            optionsBuilder(oBuilder);
            return builder;
        }
    }

    public class CodeDemo
    {
        public void Test()
        {
            MunityContext context = null;
            context.WithOrganization(options =>
            {
                options.WithName("");
            }).Build();
        }
    }
}
