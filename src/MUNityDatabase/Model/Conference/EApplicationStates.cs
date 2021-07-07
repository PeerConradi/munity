namespace MUNity.Database.Models.Conference
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
        /// If you just want to apply to be a specific delegation but the committee
        /// doesn't matter. Look into the ApplicationValue to find out what Delegation is meant by this
        /// </summary>
        DelegationApplication,

        /// <summary>
        /// If you want to apply to be inside a specific committee but the delegation doesn't matter
        /// Look into the ApplicationValue to find out what Committee you can apply on
        /// </summary>
        CommitteeApplication,

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