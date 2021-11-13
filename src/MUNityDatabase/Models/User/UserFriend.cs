using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Base;

namespace MUNity.Database.Models.User;

public class UserFriend
{
    public long UserFriendId { get; set; }

    [InverseProperty(nameof(MunityUser.Friends))]
    public MunityUser SourceUser { get; set; }

    [InverseProperty(nameof(MunityUser.InverseFriends))]
    public MunityUser TargetUser { get; set; }

    public DateTime AddedDateTime { get; set; }

    [MaxLength(300)]
    public string Comment { get; set; }

    public FriendshipStates State { get; set; }
}
