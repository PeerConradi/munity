using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Extensions
{
    public static class ResponseBuilderExtensions
    {
        public static void AddNoPermissionError(this AbstractResponse response, string description = "")
        {
            response.HasError = true;
            response.Errors.Add(new ResponseError()
            {
                ErrorType = ErrorTypes.NoPermission,
                Description = description
            });
        }

        public static void AddInvalidDataError(this AbstractResponse response, string description, string parameter = "")
        {
            response.HasError = true;
            response.Errors.Add(new ResponseError()
            {
                ErrorType = ErrorTypes.InvalidData,
                Description = description,
                ParameterName = parameter
            });
        }

        public static void AddMissingDataError(this AbstractResponse response, string description, string parameter)
        {
            response.HasError = true;
            response.Errors.Add(new ResponseError()
            {
                ErrorType = ErrorTypes.MissingData,
                Description = description,
                ParameterName = parameter
            });
        }

        public static void AddNotFoundError(this AbstractResponse response, string parameterName)
        {
            response.HasError = true;
            response.Errors.Add(new ResponseError()
            {
                ErrorType = ErrorTypes.InvalidData,
                Description = $"The data for {parameterName} was not found.",
                ParameterName = parameterName
            });
        }

        public static void AddConferenceNotFoundError(this AbstractResponse response) =>
            response.AddInvalidDataError("The needed Conference was not found");

        public static void AddCommitteeNotFoundError(this AbstractResponse response) =>
            response.AddInvalidDataError("The needed Committee was not found");


        public static void AddWarning(this AbstractResponse response, string description, string parameter = null)
        {
            response.Errors.Add(new ResponseError()
            {
                ErrorType = ErrorTypes.MissingData,
                Description = description,
                ParameterName = parameter
            });
        }
    }
}
