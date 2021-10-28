using System;

namespace MUNity.Database.FluentAPI
{
    [Serializable]
    public class ConferenceRoleNotFoundException : Exception
    {
        public ConferenceRoleNotFoundException() { }
        public ConferenceRoleNotFoundException(string message) : base(message) { }
        public ConferenceRoleNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ConferenceRoleNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
