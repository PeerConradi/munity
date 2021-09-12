using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class CreateTeamRoleGroupRequest
    {
        [Required]
        public string ConferenceId { get; set; }

        [Required]
        public string GroupName { get; set; }

        [Required]
        public string GroupFullName { get; set; }

        [Required]
        public string GroupShort { get; set; }

        [Range(1, 9999)]
        public int GroupLevel { get; set; } = 1;
    }

    public class CreateTeamRoleGroupResponse
    {
        public enum ResponseStatuses
        {
            Success,
            Error,
            NameTaken,
            FullNameTaken,
            ShortTaken,
            NoPermission,
            ConferenceNotFound
        }

        public ResponseStatuses Status { get; set; }

        public int CreatedGroupId { get; set; }
    }
}
