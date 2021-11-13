using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUNity.Database.Models.User;

public class UserBlocked
{
    public long UserBlockedId { get; set; }

    [InverseProperty(nameof(MunityUser.BlockedUsers))]
    public MunityUser SourceUser { get; set; }

    [InverseProperty(nameof(MunityUser.BlockedBy))]
    public MunityUser TargetUser { get; set; }

    public DateTime BlockedDateTime { get; set; }

    [MaxLength(300)]
    public string Comment { get; set; }
}
