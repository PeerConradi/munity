using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Organization;

namespace MUNity.Database.Extensions
{
    public static class OrganizationFluentExtensions
    {
        public static Organization AddOrganization(this MunityContext context, Action<OrganizationOptionsBuilder> options)
        {
            var builder = new OrganizationOptionsBuilder(context);
            options(builder);
            var orga = builder.Build();
            context.SaveChanges();
            return orga;
        }
    }

    public class OrganizationOptionsBuilder
    {
        private MunityContext _context;

        private Organization _organization = new Organization();

        private bool _useEasyId = false;

        public bool IsBuild { get; private set; }

        public OrganizationOptionsBuilder WithName(string name)
        {
            _organization.OrganizationName = name;
            return this;
        }

        public OrganizationOptionsBuilder WithShort(string shortName)
        {
            _organization.OrganizationShort = shortName;
            return this;
        }

        public OrganizationOptionsBuilder UseEasyId()
        {
            this._useEasyId = true;
            return this;
        }

        public OrganizationOptionsBuilder(MunityContext context)
        {
            this._context = context;
        }

        public Organization Build()
        {
            if (!this.IsBuild)
            {
                if (_useEasyId && !string.IsNullOrWhiteSpace(_organization.OrganizationShort))
                {
                    var easyId = Util.IdGenerator.AsPrimaryKey(_organization.OrganizationShort);
                    if (_context.Organizations.All(n => n.OrganizationId != easyId))
                        _organization.OrganizationId = easyId;
                }
                _context.Organizations.Add(_organization);
            }

            return _organization;
        }
    }

}
