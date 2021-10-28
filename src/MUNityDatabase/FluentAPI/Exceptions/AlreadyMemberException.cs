using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI
{

    [Serializable]
    public class AlreadyMemberException : Exception
    {
        public AlreadyMemberException() { }
        public AlreadyMemberException(string message) : base(message) { }
        public AlreadyMemberException(string message, Exception inner) : base(message, inner) { }
        protected AlreadyMemberException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
