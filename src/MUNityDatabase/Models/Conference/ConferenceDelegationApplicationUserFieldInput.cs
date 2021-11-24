namespace MUNity.Database.Models.Conference
{
    public class ConferenceDelegationApplicationUserFieldInput
    {
        public int ConferenceDelegationApplicationUserFieldInputId { get; set; }

        public DelegationApplicationUserEntry DelegationApplicationUserEntry { get; set; }

        public ConferenceDelegationApplicationUserUserField Field { get; set; }

        public string Value { get; set; }

        public string ValueSecondary { get; set; }
    }
}
