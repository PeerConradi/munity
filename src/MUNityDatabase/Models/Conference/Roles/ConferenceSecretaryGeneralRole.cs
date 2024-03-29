﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference.Roles;

/// <summary>
/// The Secretary General Role is one of the most important roles of the conference.
/// You are technically able to create more than one of this roles.
/// </summary>
public class ConferenceSecretaryGeneralRole : AbstractConferenceRole
{

    /// <summary>
    /// The Title of this role for Example:
    /// His Excellence the Secretary General.
    /// </summary>
    [MaxLength(250)]
    public string Title { get; set; }

    public ConferenceSecretaryGeneralRole() : base()
    {

    }
}
