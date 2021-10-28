using System;

namespace MUNity.Database.FluentAPI
{
    [Serializable]
    public class CommitteeNotFoundException : Exception
    {
        public CommitteeNotFoundException() { }
        public CommitteeNotFoundException(string message) : base(message) { }
        public CommitteeNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected CommitteeNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
