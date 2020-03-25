using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class ConferenceUserAuth
    {
        public int AuthId { get; set; }

        public Conference Conference { get; set; }

        public User User { get; set; }

        public bool CanRead { get; set; }

        public bool CanEditSettings { get; set; }

        public bool CanEditPublicRelations { get; set; }

        public bool CanEditGallerie { get; set; }

        public bool CanSendMails { get; set; }

        public bool CanEditTeam { get; set; }

        public bool CanLinkResolutions { get; set; }

        public bool CanDeleteConference { get; set; }
    }
}
