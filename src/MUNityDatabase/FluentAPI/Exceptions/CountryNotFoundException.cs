using System;

namespace MUNity.Database.FluentAPI;

[Serializable]
public class CountryNotFoundException : Exception
{
    public CountryNotFoundException() { }
    public CountryNotFoundException(string message) : base(message) { }
    public CountryNotFoundException(string message, Exception inner) : base(message, inner) { }
    protected CountryNotFoundException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
