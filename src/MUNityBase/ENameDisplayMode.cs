namespace MUNityBase
{
    /// <summary>
    /// The privacy settings of an user. This is privacy by default so everything should be kept as low
    /// as possible when creating the account.
    /// </summary>
    public enum ENameDisplayMode
    {
        /// <summary>
        /// Will show the Full Name: Max Mustermann
        /// </summary>
        FullName,

        /// <summary>
        /// Will display the complete forename and the first letter of the last name:
        /// Max M.
        /// </summary>
        FullForenameAndFirstLetterLastName,

        /// <summary>
        /// Will display the first letter of the forename and the complete lastname:
        /// M. Mustermann
        /// </summary>
        FirstLetterForenameFullLastName,

        /// <summary>
        /// Will display only the initals of the user:
        /// M. M.
        /// </summary>
        Initals
    }
}
