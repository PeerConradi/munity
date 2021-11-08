namespace MUNity.Base
{
    /// <summary>
    /// Possible states that can be used within a list of Speakers.
    /// </summary>
    public enum ESpeakerListStatus
    {
        /// <summary>
        /// Noone is Speaking and the last status is unknown. Resume a speaker or Questions will 
        /// give them the full SpeakerTime/QuestionTime
        /// </summary>
        Stopped,
        /// <summary>
        /// The CurrentSpeaker is talking.
        /// </summary>
        Speaking,
        /// <summary>
        /// The Current Question is talking.
        /// </summary>
        Question,
        /// <summary>
        /// The CurrentSpeaker is Answering. Meaning he/she has the same time talking as the one asking the Question.
        /// </summary>
        Answer,
        /// <summary>
        /// The Speaker is paused and will continue with the time that he/she was paused at.
        /// </summary>
        SpeakerPaused,
        /// <summary>
        /// The Question is paused an will continue with the time that he/she was paused at.
        /// </summary>
        QuestionPaused,
        /// <summary>
        /// The answer has been paused and will continue with the time that he/she was paused at.
        /// </summary>
        AnswerPaused
    }
}
