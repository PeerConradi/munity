using MUNity.Database.Context;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI
{
    public class OrganizationTools
    {
        public MunityContext DbContext { get; private set; }

        public Organization AddOrganization(Action<OrganizationOptionsBuilder> options)
        {
            var builder = new OrganizationOptionsBuilder(DbContext);
            options(builder);
            var orga = builder.Organization;
            DbContext.Organizations.Add(orga);
            DbContext.SaveChanges();
            return orga;
        }

        public OrganizationMember AddMemberIntoRole(OrganizationRole role,
            MunityUser user)
        {
            var membership = DbContext.OrganizationMembers.FirstOrDefault(n => n.User.Id == user.Id &&
                                                                          n.Role.OrganizationRoleId ==
                                                                          role.OrganizationRoleId);
            if (membership != null)
                throw new AlreadyMemberException($"The given user {user.UserName} is already inside the role {role.RoleName} ({role.OrganizationRoleId})");

            membership = new OrganizationMember();
            membership.Role = role;
            membership.User = user;
            membership.JoinedDate = DateTime.Now;

            DbContext.OrganizationMembers.Add(membership);
            DbContext.SaveChanges();
            return membership;
        }

        public OrganizationTools(MunityContext context)
        {
            this.DbContext = context;
        }
    }
}
