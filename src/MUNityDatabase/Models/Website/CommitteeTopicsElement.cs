using MUNity.Database.Models.Conference;

namespace MUNity.Database.Models.Website;

public class CommitteeTopicsElement : AbstractConferenceWebPageElement
{
    public string Title { get; set; }

    public Committee Committee { get; set; }
}
