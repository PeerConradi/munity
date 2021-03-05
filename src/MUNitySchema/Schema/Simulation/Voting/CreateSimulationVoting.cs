using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Ein Anzeigetext wird benötigt.")]
        [MaxLength(200)]
        public string Text { get; set; }

        /// <summary>
        /// Is Anstention an allowed option to vote.
        /// </summary>
        public bool AllowAbstention { get; set; }

        /// <summary>
        /// The Mode of the voting. Who is allowed to vote.
        /// </summary>
        public EVotingMode Mode { get; set; } = EVotingMode.JustDelegates;

        /// <summary>
        /// The time when the voting should be done. Set this to null if you want the voting to be
        /// stoped manual.
        /// </summary>
        public DateTime? CloseTime { get; set; }
    }
}
