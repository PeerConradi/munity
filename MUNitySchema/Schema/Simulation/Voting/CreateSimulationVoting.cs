using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    /// <summary>
    /// Body to create a new voting.
    /// </summary>
    public class CreateSimulationVoting : SimulationRequest
    {
        /// <summary>
        /// The display Text of the voting.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Is Anstention an allowed option to vote.
        /// </summary>
        public bool AllowAbstention { get; set; }

        /// <summary>
        /// The Mode of the voting. Who is allowed to vote.
        /// </summary>
        public EVotingMode Mode { get; set; }

        /// <summary>
        /// The time when the voting should be done. Set this to null if you want the voting to be
        /// stoped manual.
        /// </summary>
        public DateTime? CloseTime { get; set; }
    }
}
