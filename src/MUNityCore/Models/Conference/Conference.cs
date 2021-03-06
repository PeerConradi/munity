﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MUNityCore.Models.User;

namespace MUNityCore.Models.Conference
{

    /// <summary>
    /// A conference is as the name tells a single conference.
    /// Like Model United Nations London 2021. This means it is a single
    /// event of maybe a list of Model United Nation Conferences. If there are more
    /// the conference, for example a conference every year, then this will can be
    /// found by the Project
    /// </summary>
    public class Conference
    {
        public enum EConferenceVisibilityMode
        {
            /// <summary>
            /// The conference is only visible to the creator and everyone that
            /// participate in it.
            /// </summary>
            Participants,
            /// <summary>
            /// The conference is visible to every account that is registered inside
            /// the core.
            /// </summary>
            Users,
            /// <summary>
            /// The conference is visible to the public. Everyone calling the API
            /// can access information about this conference.
            /// </summary>
            Public
        }

        [MaxLength(80)]
        public string ConferenceId { get; set; }
        
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string FullName { get; set; }

        [MaxLength(18)]
        public string ConferenceShort { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreationDate { get; set; }

        public MunityUser CreationUser { get; set; }

        public List<Committee> Committees { get; set; }


        public List<AbstractRole> Roles { get; set; }

        public Project ConferenceProject { get; set; }

        public EConferenceVisibilityMode Visibility { get; set; }

        [Timestamp]
        public byte[] ConferenceTimestamp { get; set; }

        public Conference()
        {
            ConferenceId = Guid.NewGuid().ToString();
            Committees = new List<Committee>();
            Visibility = EConferenceVisibilityMode.Users;
        }
    }
}
