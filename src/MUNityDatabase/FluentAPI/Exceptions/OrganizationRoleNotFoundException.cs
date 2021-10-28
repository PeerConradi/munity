using System;

namespace MUNity.Database.FluentAPI
{
    [System.Serializable]
    public class OrganizationRoleNotFoundException : Exception
    {
        public OrganizationRoleNotFoundException() { }
        public OrganizationRoleNotFoundException(string message) : base(message) { }
        public OrganizationRoleNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected OrganizationRoleNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


}