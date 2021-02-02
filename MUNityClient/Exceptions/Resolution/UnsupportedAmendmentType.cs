using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Exceptions.Resolution
{

    [Serializable]
    public class UnsupportedAmendmentTypeException : Exception
    {
        public UnsupportedAmendmentTypeException() { }
        public UnsupportedAmendmentTypeException(string message) : base(message) { }
        public UnsupportedAmendmentTypeException(string message, Exception inner) : base(message, inner) { }
        protected UnsupportedAmendmentTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
