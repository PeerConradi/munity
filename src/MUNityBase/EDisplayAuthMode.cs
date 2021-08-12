namespace MUNityBase
{
    public enum EDisplayAuthMode
    {
        /// <summary>
        /// Everyone can see this information
        /// </summary>
        Everyone,

        /// <summary>
        /// Logged in users can see this information
        /// </summary>
        Users,

        /// <summary>
        /// only users that are added as friends can see this information
        /// </summary>
        Friends,

        /// <summary>
        /// only I can see this information.
        /// </summary>
        JustMe
    }
}
