using MUNity.Database.Context;
using MUNity.Schema.Conference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class ConferenceApplicationService
    {
        private MunityContext _dbContext;

        public enum AvailableDelegationsTypes
        {
            All,
            OnlineOnly,
            PresenceOnly
        }

        public List<ApplicationAvailableDelegation> AvailableDelegations(string conferenceId, int minRolesCount = 1, AvailableDelegationsTypes type = AvailableDelegationsTypes.All)
        {
            var query = _dbContext.Delegations.Where(n => n.Conference.ConferenceId == conferenceId &&
                n.Roles.Count >= minRolesCount);

            if (type == AvailableDelegationsTypes.OnlineOnly)
            {
                query = query.Where(n => n.Roles.All(a => a.Committee.CommitteeType == MUNityBase.CommitteeTypes.Online));
            }
            else if (type == AvailableDelegationsTypes.PresenceOnly)
            {
                query = query.Where(n => n.Roles.All(a => a.Committee.CommitteeType == MUNityBase.CommitteeTypes.AtLocation));
            }

                return query.Select(n => new ApplicationAvailableDelegation()
                {
                    DelegationId = n.DelegationId,
                    Name = n.Name,
                    Roles = n.Roles.Select(role => new ApplicationDelegationRoleSlot()
                    {
                        CommitteeName = role.Committee.Name,
                        Costs = _dbContext.Fluent.ForConference(conferenceId).GetCostOfRole(role.RoleId),
                        RoleName = role.RoleName,
                        CountryName = role.DelegateCountry.Name,
                        CountryIso = role.DelegateCountry.Iso
                    }).ToList()
                }).ToList();
        }

        public ConferenceApplicationService(MunityContext context)
        {
            this._dbContext = context;
        }
    }
}
