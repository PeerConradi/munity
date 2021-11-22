using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Context;
using MUNity.Schema.Conference.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class ConferenceApplicationSortingService
    {
        private List<ApplicationSortingSession> sortingSessions = new List<ApplicationSortingSession>();

        private readonly IServiceScopeFactory scopeFactory;

        public ApplicationSortingSession GetOrCreateSessionForConference(string conferenceId)
        {
            var session = sortingSessions.FirstOrDefault(n => n.ConferenceId == conferenceId);
            if (session == null)
            {
                session = CreateForConference(conferenceId);
                sortingSessions.Add(session);
            }
            return session;
        }

        private ApplicationSortingSession CreateForConference(string conferenceId)
        {
            var session = new ApplicationSortingSession();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MunityContext>();

                session.ConferenceId = conferenceId;
                session.Applications = dbContext.DelegationApplications
                    .Where(n => n.Conference.ConferenceId == conferenceId)
                    .AsNoTracking()
                    .Select(n => new SortableApplication()
                    {
                        ApplicationId = n.DelegationApplicationId,
                        CountOfUsers = n.Users.Count(),
                        DelegationWishes = n.DelegationWishes
                        .Select(a => new DelegationWish()
                        {
                            Accepted = false,
                            DelegationId = a.Delegation.DelegationId,
                            DelegationName = a.Delegation.Name,
                            Priority = a.Priority
                        }).ToList()
                    }).ToList();
            }
            return session;

        }

        public ConferenceApplicationSortingService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
    }
}
