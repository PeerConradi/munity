using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Resolution;

/// <summary>
/// A ResolutionAuth saves a resolution inside the Database and manages
/// the access to this element. Note that this Logic is seperated from the
/// MUN-Core this is due to a discussion that resulted in a clear split
/// between basic MUN functions and advanced editors.
/// </summary>
public class ResolutionAuth
{
    [Key]
    public int ResolutionAuthId { get; set; }

    public MunityUser CreationUser { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastChangeTime { get; set; }

    public ICollection<ResolutionUser> Users { get; set; }

    public bool AllowPublicRead { get; set; }

    public bool AllowPublicEdit { get; set; }

    public bool AllowConferenceRead { get; set; }

    public bool AllowCommitteeRead { get; set; }

    public bool AllowOnlineAmendments { get; set; }

    public string PublicShortKey { get; set; }

    public string EditPassword { get; set; }

    public string ReadPassword { get; set; }

    /// <summary>
    /// Links to a MUN Core Committee
    /// </summary>
    public Conference.Committee Committee { get; set; }

    /// <summary>
    /// Links the document to a conference. It can be linked to a conference
    /// but no committee.
    /// </summary>
    public Conference.Conference Conference { get; set; }

    public ResaElement Resolution { get; set; }

    public ResolutionAuth()
    {

    }
}
