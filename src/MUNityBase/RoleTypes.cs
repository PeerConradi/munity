namespace MUNityBase
{
    /// <summary>
    /// The roles a user can take when inside a simulation / virtual committee.
    /// </summary>
    public enum RoleTypes
    {
        /// <summary>
        /// A spectator role that cannot interact with the simulation but will be displayed as spectator.
        /// For example press, spectators another committee that is visiting this committee etc.
        /// </summary>
        Spectator,
        /// <summary>
        /// The chairman role that can edit the list of speakers and resolution and can interact with requests of other roles.
        /// </summary>
        Chairman,
        /// <summary>
        /// a delegate role that is part of the committee.
        /// </summary>
        Delegate,
        /// <summary>
        /// A moderator role that can interact with all other users in this simulation by having higher powers than any other role
        /// and is allowed to kick users.
        /// </summary>
        Moderator,
        /// <summary>
        /// A non government organization that has some ability to interact with the simulation but not as much as a delegate (cant vote etc.)
        /// </summary>
        Ngo,
        None
    }
}
