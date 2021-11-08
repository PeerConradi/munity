namespace MUNity.Database.Models.Conference;

public class ConferenceDelegationApplicationFieldInput
{
    public long ConferenceDelegationApplicationFieldInputId { get; set; }

    public DelegationApplication Application { get; set; }

    public ConferenceApplicationField Field { get; set; }

    public string Value { get; set; }

    public string ValueSecondary { get; set; }
}
