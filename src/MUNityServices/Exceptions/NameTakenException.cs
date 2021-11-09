using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services.Exceptions;


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
