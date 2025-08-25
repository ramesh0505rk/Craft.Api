using Craft.Domain.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Domain.Common.ExceptionHandling
{
    public class ApiErrorHandler
    {
        public static ApiError GetDefaultErrorMessage(HttpStatusCode statusCode, string customMessage = null)
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => new ApiError { Message = "Bad Request", Details = string.IsNullOrWhiteSpace(customMessage) ? HttpErrorMessages.BadRequest : customMessage },
                HttpStatusCode.Unauthorized => new ApiError { Message = "Unauthorized", Details = string.IsNullOrWhiteSpace(customMessage) ? HttpErrorMessages.Unauthorized : customMessage },
                HttpStatusCode.Forbidden => new ApiError { Message = "Forbidden", Details = string.IsNullOrWhiteSpace(customMessage) ? HttpErrorMessages.Forbidden : customMessage },
                HttpStatusCode.InternalServerError => new ApiError { Message = "Internal Server Error", Details = string.IsNullOrWhiteSpace(customMessage) ? HttpErrorMessages.InternalServerError : customMessage },
                _ => new ApiError { Message = "Bad Request", Details = string.IsNullOrWhiteSpace(customMessage) ? HttpErrorMessages.UnexpectedError : customMessage }
            };
        }
    }
}
