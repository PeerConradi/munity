using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Util.Exceptions
{
    public class ConferenceNotFoundException : Exception
    {
        public ConferenceNotFoundException()
        {
        }

        public ConferenceNotFoundException(string message) : base(message)
        {
        }

        public ConferenceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConferenceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
