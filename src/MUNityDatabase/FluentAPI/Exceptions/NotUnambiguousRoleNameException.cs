using System;

namespace MUNity.Database.FluentAPI;

[Serializable]
public class NotUnambiguousRoleNameException : Exception
{
    public NotUnambiguousRoleNameException() { }
    public NotUnambiguousRoleNameException(string message) : base(message) { }
    public NotUnambiguousRoleNameException(string message, Exception inner) : base(message, inner) { }
    protected NotUnambiguousRoleNameException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
