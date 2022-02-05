namespace MUNity.Base
{
    /// <summary>
    /// Different States the Petition can have.
    /// </summary>
    public enum EPetitionStates
    {
        /// <summary>
        /// Unkown/Unhandled Status
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
        /// is currently in review for example the chairs are taking a look at it, before adding it 
        /// to the offical list of petitions that are waiting.
        /// </summary>
        InReview,
        /// <summary>
        /// Is part of the list
        /// </summary>
        Queued,
        /// <summary>
        /// If the petition was removed
        /// </summary>
        Removed,
        /// <summary>
        /// When the petition was active and is now resolved.
        /// </summary>
        Resolved,
    }
}
