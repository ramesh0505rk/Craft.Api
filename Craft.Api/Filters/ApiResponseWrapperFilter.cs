using Craft.Domain.Common.ExceptionHandling;
using Craft.Domain.Common.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Craft.Api.Filters
{
    public class ApiResponseWrapperFilter : IActionFilter
    {
        private readonly ILogger<ApiResponseWrapperFilter> _logger;
        public ApiResponseWrapperFilter(ILogger<ApiResponseWrapperFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Extract validation errors
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value.Errors.Select(er =>
                    {
                        var key = e.Key.StartsWith("$.") ? e.Key[2..] : e.Key;
                        // Clean default deserialization noise like "Path:", "LineNumber:", etc.
                        var msg = CleanErrorMessage(er.ErrorMessage);

                        return $"{key}: {msg}";
                    }))
                    .Distinct() // Ensures duplicate messages are removed
                    .ToList();

                // Build custom response
                var errorDetails = errors?.Count > 0 ? string.Join(", ", errors) : string.Empty;
                var errorResponse = new ApiErrorResponse
                {
                    Errors = ApiErrorHandler.GetDefaultErrorMessage(HttpStatusCode.BadRequest, errorDetails)
                };

                // Log errors
                _logger.LogError("----- System generated 400 BadRequest Model Validation Error -----\n" +
                                "Method: {Method}\n" +
                                "Path: {Path}\n" +
                                "Validation Errors: {@Errors}\n" +
                                "---------------------------------",
                                context.HttpContext.Request.Method, context.HttpContext.Request.Path, errorResponse
                                );

                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Code to execute after the action executes
        }

        private string CleanErrorMessage(string rawMessage)
        {
            if (string.IsNullOrEmpty(rawMessage))
                return string.Empty;

            var keywordsToTrim = new[] { "Path:", "LineNumber:", "BytePositionInLine:" };

            foreach (var keyword in keywordsToTrim)
            {
                var index = rawMessage.IndexOf(keyword);
                if (index >= 0)
                {
                    rawMessage = rawMessage[..index];
                    break; // Stop at first match
                }
            }
            // Final cleanup
            return rawMessage.Trim().TrimEnd('.') + ".";
        }
    }
}
