using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;

namespace MUNity.Database.Extensions
{
    public static class CalculationExtensions
    {
        public static DelegationCostResult CostsOfDelegation(this MunityContext context, string delegationId)
        {
            var result = new DelegationCostResult();


            var delegation = context.Delegations
                .Include(n => n.Roles)
                .Select(n => new
                {
                    DelegationId = n.DelegationId,
                    ConferenceId = n.Conference.ConferenceId,
                    Roles = n.Roles.Select(a => new
                    {
                        RoleId = a.RoleId,
                        RoleName = a.RoleName,
                        CommitteeId = a.Committee.CommitteeId
                    })
                })
                .FirstOrDefault(n => n.DelegationId == delegationId);
                

            if (delegation == null)
                throw new NullReferenceException($"The Delegation with Id {delegationId} was not found!");

            decimal priceForConference =
                context.Conferences.Where(n => n.ConferenceId == delegation.ConferenceId)
                    .Select(a => a.GeneralParticipationCost)
                    .FirstOrDefault();

            decimal? priceForDelegation =
                context.ConferenceParticipationCostRules.Where(n => n.Delegation.DelegationId == delegationId)
                    .Select(n => n.Costs)
                    .FirstOrDefault();

            foreach (var role in delegation.Roles)
            {
                // Check for role price
                decimal? rolePrice = context.ConferenceParticipationCostRules
                    .Where(n => n.Role.RoleId == role.RoleId)
                    .Select(n => n.Costs)
                    .FirstOrDefault();



                // Check for Committee Price
                if (rolePrice == null && role.CommitteeId != null)
                {
                    
                    if (priceForDelegation != null)
                    {
                        // Prio 2: Price for the delegation
                        result.Costs.Add(new DelegationCostPoint()
                        {
                            CommitteeName = context.Delegates.Where(n => n.RoleId == role.RoleId).Select(n => n.Committee.Name).FirstOrDefault(),
                            Cost = priceForDelegation.Value,
                            RoleId = role.RoleId,
                            RoleName = role.RoleName
                        });
                    }
                    else
                    {
                        // Prio 3: Price for the committee
                        decimal? committeePrice = context.ConferenceParticipationCostRules
                            .Where(n => n.Committee.CommitteeId == role.CommitteeId)
                            .Select(n => n.Costs)
                            .FirstOrDefault();

                        if (committeePrice != null)
                        {
                            result.Costs.Add(new DelegationCostPoint()
                            {
                                CommitteeName = context.Delegates.Where(n => n.RoleId == role.RoleId).Select(n => n.Committee.Name).FirstOrDefault(),
                                Cost = committeePrice.Value,
                                RoleId = role.RoleId,
                                RoleName = role.RoleName
                            });
                        }
                        else
                        {
                            result.Costs.Add(new DelegationCostPoint()
                            {
                                CommitteeName = context.Delegates.Where(n => n.RoleId == role.RoleId).Select(n => n.Committee.Name).FirstOrDefault(),
                                Cost = priceForConference,
                                RoleId = role.RoleId,
                                RoleName = role.RoleName
                            });
                        }
                    }
                }
                else if (rolePrice != null)
                {
                    // Prio 1: Role Price (Highest)
                    result.Costs.Add(new DelegationCostPoint()
                    {
                        CommitteeName = context.Delegates.Where(n => n.RoleId == role.RoleId).Select(n => n.Committee.Name).FirstOrDefault(),
                        Cost = rolePrice.Value,
                        RoleId = role.RoleId,
                        RoleName = role.RoleName
                    });
                }

            }

            return result;
        }

    }
}
