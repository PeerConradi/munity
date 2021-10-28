using System;

namespace MUNity.Database.FluentAPI
{
    [Serializable]
    public class ApplicationTypeNotAllowedException : Exception
    {
        public ApplicationTypeNotAllowedException() { }
        public ApplicationTypeNotAllowedException(string message) : base(message) { }
        public ApplicationTypeNotAllowedException(string message, Exception inner) : base(message, inner) { }
        protected ApplicationTypeNotAllowedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
