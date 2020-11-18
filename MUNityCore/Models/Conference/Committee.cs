﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;
using MUNityCore.Schema.Request.Conference;

namespace MUNityCore.Models.Conference
{

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
    [DataContract]
    public class Committee : ICommitteeFacade
    {
        /// <summary>
        /// The id of the committee.
        /// </summary>
        [DataMember]
        public string CommitteeId { get; set; }

        /// <summary>
        /// The short name of the committee
        /// </summary>
        [DataMember]
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        /// <summary>
        /// The full (long) name of the committee
        /// </summary>
        [DataMember]
        [MaxLength(250)]
        public string FullName { get; set; }

        /// <summary>
        /// A short for the committee. This is limited to ten characters but it should be between 2 and 5 letters.
        /// For example GA gor general assembly.
        /// </summary>
        [DataMember]
        [MaxLength(10)]
        public string CommitteeShort { get; set; }

        /// <summary>
        /// The article of the committee. This may very depending on the language of the conference.
        /// For english conferences it will be "the", for german conferences it will be "der, die, das".
        /// </summary>
        [DataMember]
        [MaxLength(10)]
        public string Article { get; set; }

        /// <summary>
        /// The ResolutlyCommittee is the parent or head committee of this committee.
        /// </summary>
        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Committee ResolutlyCommittee { get; set; }

        /// <summary>
        /// The id of the parent/head committee for the json serialization.
        /// </summary>
        [DataMember]
        [NotMapped]
        public string ResolutlyCommitteeId => ResolutlyCommittee?.ResolutlyCommitteeId ?? "";

        /// <summary>
        /// the conferenceId for the json serialization
        /// </summary>
        [DataMember]
        [NotMapped]
        public string ConferenceId => Conference?.ConferenceId ?? "";

        /// <summary>
        /// The conference of this committee.
        /// </summary>
        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Conference Conference { get; set; }

        /// <summary>
        /// Topics of this committee.
        /// </summary>
        public List<CommitteeTopic> Topics { get; set; }

        /// <summary>
        /// The timestamp when this committee has been created or last changed.
        /// </summary>
        [IgnoreDataMember]
        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] CommitteeTimestamp { get; set; }

        public Committee(ConferenceRequests.CreateCommittee request)
        {
            CommitteeId = Guid.NewGuid().ToString();
            Article = request.Article;
            Name = request.Name;
            FullName = request.FullName;
            CommitteeShort = request.Abbreviation;
        }

        public Committee()
        {
            CommitteeId = Guid.NewGuid().ToString();
        }
    }
}
