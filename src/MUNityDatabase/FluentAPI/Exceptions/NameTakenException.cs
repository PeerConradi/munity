using System;

namespace MUNity.Database.FluentAPI;

[Serializable]
public class NameTakenException : Exception
{
    public NameTakenException() { }
    public NameTakenException(string message) : base(message) { }
    public NameTakenException(string message, Exception inner) : base(message, inner) { }
    protected NameTakenException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
