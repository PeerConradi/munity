﻿using MUNity.Base;
using MUNity.Database.Models.Conference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference;

public class PetitionType
{
    /// <summary>
    /// The identifier of the Petition type.
    /// </summary>
    public int PetitionTypeId { get; set; }

    public int SortOrder { get; set; }

    /// <summary>
    /// A Name to Display this petition. For example: Right of information.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// More information to give about this petition type
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// A Reference to where you get more information about this petition. This could be a link or
    /// a name of the paragraph insiide the rules of procedure.
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    /// How do you get to vote on this Petitiontype
    /// </summary>
    public PetitionRulings Ruling { get; set; }

    /// <summary>
    /// A category for the petitiontype
    /// </summary>
    public string Category { get; set; }

    public bool AllowCountyDelegates { get; set; }

    public bool AllowNonCountryDelegates { get; set; }

    public ICollection<Committee> AllowedCommittees { get; set; }

    public RuleOfProcedure RuleOfProcedure { get; set; }

}
