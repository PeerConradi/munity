namespace MUNity.Base
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
        /// With a RoleApplication
        /// </summary>
        DirectApplication,

        /// <summary>
        /// Allows users to create a Delegation Applications for whatever Delegations this role is in.
        /// </summary>
        DelegationApplication,

        /// <summary>
        /// Looks inside the ApplicationValue of an AbstractConferenceRole
        /// </summary>
        Custom
    }
}
