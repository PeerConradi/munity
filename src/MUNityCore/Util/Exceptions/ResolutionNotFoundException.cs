using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Util.Exceptions
{
    [System.Serializable]
    public class ResolutionNotFoundException : Exception
    { 
        public ResolutionNotFoundException() { }
        public ResolutionNotFoundException(string message) : base(message) { }
        public ResolutionNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ResolutionNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
