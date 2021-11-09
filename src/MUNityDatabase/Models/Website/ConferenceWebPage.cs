using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.User;

namespace MUNity.Database.Models.Website;

public class ConferenceWebPage
{
    public string ConferenceWebPageId { get; set; }

    public string Title { get; set; }

    public MunityUser CreatedUser { get; set; }

    public Models.Conference.Conference Conference { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public bool IsIndexPage { get; set; }

    public ICollection<AbstractConferenceWebPageElement> Components { get; set; }

    public ConferenceWebPage()
    {
        ConferenceWebPageId = Guid.NewGuid().ToString();
    }
}
