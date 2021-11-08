namespace MUNity.Base
{
    public enum PetitionRulings
    {
        /// <summary>
        /// There is no type of ruling defined.
        /// </summary>
        Unknown,
        /// <summary>
        /// The Chairs make the decision.
        /// </summary>
        Chairs,
        /// <summary>
        /// Its voted and accepted if the result is 2/3 in favor.
        /// </summary>
        TwoThirds,
        /// <summary>
        /// It is decided when the majority (50% + 1) is in favor.
        /// </summary>
        simpleMajority,
        /// <summary>
        /// Special case for voting inside the SecurityCouncil
        /// </summary>
        TwoThirdsPlusPermanentMembers
    }
}
