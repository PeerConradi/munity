using System;

namespace MUNity.Database.FluentAPI
{
    [Serializable]
    public class DelegationNotFoundException : Exception
    {
        public DelegationNotFoundException() { }
        public DelegationNotFoundException(string message) : base(message) { }
        public DelegationNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected DelegationNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
