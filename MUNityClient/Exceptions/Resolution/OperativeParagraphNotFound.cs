using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Exceptions.Resolution
{

    [Serializable]
    public class OperativeParagraphNotFoundException : Exception
    {
        public OperativeParagraphNotFoundException() { }
        public OperativeParagraphNotFoundException(string message) : base(message) { }
        public OperativeParagraphNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected OperativeParagraphNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
