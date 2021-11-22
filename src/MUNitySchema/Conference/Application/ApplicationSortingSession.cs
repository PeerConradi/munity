using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUNity.Schema.Conference.Application
{
    public class ApplicationSortingSession
    {
        public string Id { get; set; }

        public List<SortableApplication> Applications { get; set; } = new List<SortableApplication>();

        public string ConferenceId { get; set; }

        public ApplicationSortingSession()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }

    public class SortableApplication
    {
        public int ApplicationId { get; set; }

        public int CountOfUsers { get; set; }

        public List<DelegationWish> DelegationWishes { get; set; }

        public bool IsAccepted => DelegationWishes != null && DelegationWishes.Any(n => n.Accepted);
    }

    public class DelegationWish
    {
        public string DelegationId { get; set; }

        public int Priority { get; set; }

        public string DelegationName { get; set; }

        public bool Accepted { get; set; }
    }

}
