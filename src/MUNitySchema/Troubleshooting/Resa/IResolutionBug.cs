using MUNity.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Troubleshooting.Resa
{

    /// <summary>
    /// Interface to declare a bug inside a resolution.
    /// </summary>
    public interface IResolutionBug
    {
        /// <summary>
        /// The description that should be displayed when a bug is found
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Search for the implemented type of bug inside the given resolution.
        /// Should return true if the bug is found
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        bool Detect(Resolution resolution);

        /// <summary>
        /// Try to fix the bug. If it was successful return true
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        bool Fix(Resolution resolution);
    }
}
