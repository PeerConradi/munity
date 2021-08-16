namespace MUNityBase
{
    /// <summary>
    /// Indicated a way a user can apply to a role.
    /// </summary>
    public enum EApplicationStates
    {
        /// <summary>
        /// No one can apply to this role
        /// </summary>
        Closed,

        /// <summary>
        /// Every user can directly apply for this particular role.
        /// </summary>
        DirectApplication,

        /// <summary>
        /// CategoryApplication means you apply for any Role inside the RoleGroup
        /// for example you just apply to be part of a PressRole.
        /// </summary>
        CategoryApplication,

        /// <summary>
        /// A registration means you can register for this role with a given key.
        /// </summary>
        Registration,

        /// <summary>
        /// Allows users to create a Delegation Applications for whatever Delegations this role is in.
        /// All other DelegateRoles inside the delegation this one is in is also meant by the DelegationApplication.
        /// </summary>
        DelegationApplication,

        /// <summary>
        /// Allows applications to this role only with Grouped Applications.
        /// </summary>
        OnlyDelegationGroup,

        /// <summary>
        /// Closed to public means only a list of users can apply to this role
        /// </summary>
        ClosedToPublic,

        /// <summary>
        /// Looks inside the ApplicationValue of an AbstractConferenceRole
        /// </summary>
        Custom
    }
}
