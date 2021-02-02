using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Exceptions.Resolution
{

    /// <summary>
    /// Throw this exception when an operative section is not found inside the OperativeSection.
    /// </summary>
    [Serializable]
    public class OperativeParagraphNotFoundException : Exception
    {
        /// <summary>
        /// Base exception of an operative paragraph could not be found
        /// </summary>
        public OperativeParagraphNotFoundException() { }

        /// <summary>
        /// Operative paragraph cannot be found inside the operative section with a message
        /// </summary>
        /// <param name="message"></param>
        public OperativeParagraphNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Paragraph cannot be found with a message and an inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public OperativeParagraphNotFoundException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// General info.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected OperativeParagraphNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
