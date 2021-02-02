using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Exceptions.Resolution
{

    /// <summary>
    /// Throw this Exception when the given implementation of the AbstractAmendment is not capable to work with 
    /// in a given case.
    /// </summary>
    [Serializable]
    public class UnsupportedAmendmentTypeException : Exception
    {
        /// <summary>
        /// Exception that the action cannot be performed with this type of amendment.
        /// </summary>
        public UnsupportedAmendmentTypeException() { }

        /// <summary>
        /// Exception that the action cannot be performed with this type of amendment with some extra information.
        /// </summary>
        /// <param name="message"></param>
        public UnsupportedAmendmentTypeException(string message) : base(message) { }

        /// <summary>
        /// Exception of something else went wrong and the inner Exception of what went wrong.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public UnsupportedAmendmentTypeException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// General info.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnsupportedAmendmentTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
