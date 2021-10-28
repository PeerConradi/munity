namespace MUNityBase
{
    public enum DelegationApplicationUserEntryStatuses
    {
        /// <summary>
        /// User is inside the application
        /// </summary>
        Joined,
        /// <summary>
        /// user wants to join the application
        /// </summary>
        RequestJoining,
        /// <summary>
        /// Was added by the creator of the application but needs to accept this
        /// invite.
        /// </summary>
        Invited
    }
}