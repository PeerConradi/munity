using MUNity.Database.Context;
using MUNity.Database.Models.Organization;
using System.Linq;
using System;

namespace MUNity.Database.FluentAPI
{
    public class OrganizationSpecificTools
    {
        private string _organizationId;

        private MunityContext _dbContext;

        public OrganizationMember AddUserIntoRole(string username, string roleName)
        {
            var user = this._dbContext.Users.FirstOrDefault(n => n.NormalizedUserName == username.ToUpper());

            if (user == null)
                throw new UserNotFoundException($"No user with the username {username} was found.");

            var role = this._dbContext.OrganizationRoles.FirstOrDefault(n => n.Organization.OrganizationId == _organizationId &&
            n.RoleName == roleName);

            if (role == null)
                throw new OrganizationRoleNotFoundException($"No Role with the name {roleName} was found for the organization {this._organizationId}");

            var membership = new OrganizationMember()
            {
                JoinedDate = DateTime.Now,
                Organization = _dbContext.Organizations.Find(_organizationId),
                Role = role,
                User = user
            };

            _dbContext.OrganizationMembers.Add(membership);
            _dbContext.SaveChanges();
            return membership;
        }

        public bool HasUserMembership(string username)
        {
            return _dbContext.OrganizationMembers.Any(n => n.Organization.OrganizationId == _organizationId &&
            n.User.UserName == username);
        }

        public OrganizationSpecificTools(MunityContext context, string organizationId)
        {
            this._dbContext = context;
            this._organizationId = organizationId;
        }
    }


}