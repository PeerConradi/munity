namespace MUNityBase
{
    /// <summary>
    /// The different modes the lobby phase can have.
    /// </summary>
    public enum LobbyModes
    {
        /// <summary>
        /// Allows to Join the game with a role token and will then give
        /// you the role.
        /// </summary>
        WithRoleKey,
        /// <summary>
        /// You can join the lobby and pick a role
        /// </summary>
        PickRole,
        /// <summary>
        /// Allow everyone inside the Lobby to create a role
        /// </summary>
        CreateAnyRole,
        /// <summary>
        /// the lobby is closed you cannot join.
        /// </summary>
        Closed
    }
}
