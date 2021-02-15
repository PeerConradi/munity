using System;
using System.Collections.Generic;
using System.Text;

namespace MUNitySchema.Models.Simulation
{
    /// <summary>
    /// Different States the Petition can have.
    /// </summary>
    public enum EPetitionStates
    {
        /// <summary>
        /// Known Status
        /// </summary>
        Unkown,
        /// <summary>
        /// Is currently active
        /// </summary>
        Active,
        /// <summary>
        /// Is done
        /// </summary>
        Done,
        /// <summary>
        /// is in a lower priority
        /// </summary>
        LowPrio,
        /// <summary>
        /// is currently in review for example the chairs are taking a look at it.
        /// </summary>
        InReview
    }
}
