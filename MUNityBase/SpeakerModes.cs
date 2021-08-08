namespace MUNityBase
{
    /// <summary>
    /// The Mode with what the Speaker is handled
    /// </summary>
    public enum SpeakerModes
    {
        /// <summary>
        /// Is the Speaker on the List of Speakers
        /// </summary>
        WaitToSpeak,
        /// <summary>
        /// Is the Speaker on the List of Questions
        /// </summary>
        WaitForQuesiton,
        /// <summary>
        /// Is The speaker Currently speaking
        /// </summary>
        CurrentlySpeaking,
        /// <summary>
        /// Is the speaker currently asking a question
        /// </summary>
        CurrentQuestion
    }
}
