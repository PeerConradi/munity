using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.User;

public class MunityUserRole : IdentityUserRole<string>
{
    [ForeignKey(nameof(UserId))]
    public virtual MunityUser User { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual MunityRole Role { get; set; }

    public MunityUserRole()
    {

    }
}
