namespace MUNity.Base
{
    public enum EConferenceVisibilityMode
    {
        /// <summary>
        /// The conference is only visible to the creator and everyone that
        /// participate in it.
        /// </summary>
        Participants = 0,
        /// <summary>
        /// The conference is visible to every account that is registered inside
        /// the core.
        /// </summary>
        Users = 1,
        /// <summary>
        /// The conference is visible to the public. Everyone calling the API
        /// can access information about this conference.
        /// </summary>
        Public = 2
    }
}
