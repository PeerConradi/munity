using MUNity.Base;

namespace MUNity.Database.Models.Conference;

public class ConferenceDelegationApplicationUserUserField
{
    public int ConferenceDelegationApplicationUserUserFieldId { get; set; }

    public ConferenceApplicationFormula Forumula { get; set; }

    public string FieldName { get; set; }

    public string FieldDescription { get; set; }

    public bool IsRequired { get; set; }

    public ConferenceApplicationFieldTypes FieldType { get; set; }

    public string DefaultValue { get; set; }

}
