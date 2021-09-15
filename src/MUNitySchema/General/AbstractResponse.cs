using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema
{
    public abstract class AbstractResponse
    {
        public bool HasError { get; set; }

        public List<ResponseError> Errors { get; set; } = new List<ResponseError>();
    }

    public class ResponseError
    {
        public ErrorTypes ErrorType { get; set; }

        public string ParameterName { get; set; }

        public string Description { get; set; }
    }

    public enum ErrorTypes
    {
        NoPermission,
        InvalidData,
        MissingData,
        Warning
    }
}
