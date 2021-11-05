using System.Collections.Generic;

namespace MUNity.Database.Models.Website
{
    public class ConferenceWebMenuEntry
    {
        public int ConferenceWebMenuEntryId { get; set; }

        public Conference.Conference Conference { get; set; }

        public string Title { get; set; }

        public ConferenceWebMenuEntry Parent { get; set; }

        public ICollection<ConferenceWebMenuEntry> ChildEntries { get; set; }

        public ConferenceWebPage TargetedPage { get; set; }
    }
}
