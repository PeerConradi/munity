using MUNity.Base;
using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.User;

public interface IUserPrivacySettings
{

    /// <summary>
    /// The way the User Forename and Lastname will be given out, to an not logged in user.
    /// </summary>
    ENameDisplayMode PublicNameDisplayMode { get; set; }

    /// <summary>
    /// The way the User Forename and lastname will be shown to a user that is logged in
    /// into the backend. Note that this will be used on all types of users. Moderators
    /// and Admins can view the full name anyway! Also the Team of a conference will
    /// also be able to see the complete user data.
    /// </summary>
    ENameDisplayMode InternalNameDisplayMode { get; set; }

    /// <summary>
    /// The way other users of a conference you are in will be able to see this
    /// users fore- and lastname
    /// </summary>
    ENameDisplayMode ConferenceNameDisplayMode { get; set; }

    /// <summary>
    /// The history of conferences this user participated in
    /// </summary>
    EDisplayAuthMode ConferenceHistoryDisplayMode { get; set; }

    /// <summary>
    /// The friends this user has added
    /// </summary>
    EDisplayAuthMode FriendslistDisplayMode { get; set; }

    /// <summary>
    /// Who is allowed to see the pinboard of the user.
    /// </summary>
    EDisplayAuthMode PinboardDisplayMode { get; set; }

    /// <summary>
    /// The age of this user
    /// </summary>
    EDisplayAuthMode AgeDisplayMode { get; set; }
}
