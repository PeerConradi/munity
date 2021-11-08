using MUNity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference;

/// <summary>
/// The committees are listed inside each Conference. Every conference
/// needs to create its own list of committees, they should not be reused inside of other
/// Conferences. The seats of the committee are defined by the DelegateRoles.
///
/// For example a delegate role for Germany that references this committee would be one seat for germany.
/// Two roles of the same State in this committee would also mean two seats.
///
/// You cannot define seats for press or ngos inside a committee.
/// </summary>
public class Committee
{
    /// <summary>
    /// The id of the committee.
    /// </summary>
    public string CommitteeId { get; set; } = "";

    /// <summary>
    /// The short name of the committee
    /// </summary>
    [Required]
    [MaxLength(150)]
    public string Name { get; set; }

    /// <summary>
    /// The full (long) name of the committee
    /// </summary>
    [MaxLength(250)]
    public string FullName { get; set; }

    /// <summary>
    /// A short for the committee. This is limited to ten characters but it should be between 2 and 5 letters.
    /// For example GA gor general assembly.
    /// </summary>
    [MaxLength(10)]
    public string CommitteeShort { get; set; }

    /// <summary>
    /// The article of the committee. This may very depending on the language of the conference.
    /// For english conferences it will be "the", for german conferences it will be "der, die, das".
    /// </summary>
    [MaxLength(10)]
    public string Article { get; set; }

    public CommitteeTypes CommitteeType { get; set; }

    /// <summary>
    /// The ResolutlyCommittee is the parent or head committee of this committee.
    /// </summary>
    public Committee ResolutlyCommittee { get; set; }

    public ICollection<Committee> ChildCommittees { get; set; }

    /// <summary>
    /// The conference of this committee.
    /// </summary>
    public Conference Conference { get; set; }

    /// <summary>
    /// Topics of this committee.
    /// </summary>
    public ICollection<CommitteeTopic> Topics { get; set; }

    /// <summary>
    /// List of sessions of the committee.
    /// </summary>
    public ICollection<Session.CommitteeSession> Sessions { get; set; }

    public ICollection<Resolution.ResolutionAuth> Resolutions { get; set; }


    public Committee()
    {
        Topics = new List<CommitteeTopic>();
        ChildCommittees = new List<Committee>();
    }
}
