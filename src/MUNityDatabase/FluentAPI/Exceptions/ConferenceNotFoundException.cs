namespace MUNity.Database.FluentAPI
{
    [System.Serializable]
    public class ConferenceNotFoundException : System.Exception
    {
        public ConferenceNotFoundException() { }
        public ConferenceNotFoundException(string message) : base(message) { }
        public ConferenceNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected ConferenceNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}