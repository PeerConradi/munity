namespace MUNity.Database.Models.Conference;

public class ConferenceFillApplicationFieldInput
{
    public long ConferenceFillApplicationFieldInputId { get; set; }

    public FillApplication Application { get; set; }

    public ConferenceApplicationField Field { get; set; }

    public string Value { get; set; }

    public string ValueSecondary { get; set; }
}
