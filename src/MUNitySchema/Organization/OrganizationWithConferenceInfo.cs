namespace MUNity.Schema.Organization
{
    public class OrganizationWithConferenceInfo : OrganizationTinyInfo
    {
        public int ProjectCount { get; set; }

        public int ConferenceCount { get; set; }
    }
}
