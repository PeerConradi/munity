using MUNity.Database.Models.User;
using MUNityBase;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MUNityCore.Models.User
{
    /// <summary>
    /// The Authorization of one or more users.
    /// </summary>
    public class MunityUserAuth
    {

        public int MunityUserAuthId { get; set; }

        [MaxLength(150)]
        public string UserAuthName { get; set; }

        public List<MunityUser> Users { get; set; }

        public bool CanCreateOrganization { get; set; }

        public EAuthLevel AuthLevel { get; set; }

        public MunityUserAuth()
        {
            Users = new List<MunityUser>();
            CanCreateOrganization = false;
            AuthLevel = EAuthLevel.New;
        }

        public MunityUserAuth(string name)
        {
            Users = new List<MunityUser>();
            CanCreateOrganization = false;
            AuthLevel = EAuthLevel.New;
            UserAuthName = name;
        }
    }
}
