using Craft.Domain.Common.ExceptionHandling;
using Craft.Domain.Common.Response;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace Craft.Api.Middleware
{
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;

		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (ShouldEnableBuffering(context))
			{
				context.Request.EnableBuffering();
			}

			try
			{
				await _next(context);
				await HandlePipelineErrorsAsync(context);
			}
			catch (Exception ex)
			{
				await LogAndWriteErrorAsync(context, ex);
			}
		}

		private bool ShouldEnableBuffering(HttpContext context)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(context.Request.ContentType))
					return false;

				if ((context.Request.Method == HttpMethods.Post ||
					 context.Request.Method == HttpMethods.Put ||
					 context.Request.Method == HttpMethods.Patch)
					&& (context.Request.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase)
						|| context.Request.ContentType.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)))
					return true;

				return false;
			}
			catch
			{
				return false;
			}
		}

		private async Task HandlePipelineErrorsAsync(HttpContext context)
		{
			var statusMap = new Dictionary<int, HttpStatusCode>
			{
				{ StatusCodes.Status401Unauthorized,HttpStatusCode.Unauthorized },
				{ StatusCodes.Status403Forbidden , HttpStatusCode.Forbidden },
				{ StatusCodes.Status500InternalServerError, HttpStatusCode.InternalServerError }
			};

			if (statusMap.TryGetValue(context.Response.StatusCode, out var statusCode))
			{
				_logger.LogError("----- {StatusCode} {StatusDescription} Response Returned by Pipeline -----\nPath: {Path}",
				(int)statusCode, statusCode, context.Request.Path);
			}
		}

		private async Task LogAndWriteErrorAsync(HttpContext context, Exception ex)
		{
			var (statusCode, customErrors) = ex switch
			{
				BadRequestCustomException badRequestEx => (HttpStatusCode.BadRequest, badRequestEx.Errors),
				UnAuthorizedCustomException unAuthorizedEx => (HttpStatusCode.Unauthorized, unAuthorizedEx.Errors),
				BadHttpRequestException => (HttpStatusCode.BadRequest, null),
				UnauthorizedAccessException => (HttpStatusCode.Unauthorized, null),
				_ => (HttpStatusCode.BadRequest, null)
			};

			await LogRequestAsync(context, ex);
			await WriteErrorResponseAsync(context, statusCode, customErrors);
		}

		private async Task WriteErrorResponseAsync(HttpContext context, HttpStatusCode statusCode, List<string>? customMessages = null)
		{
			if (context.Response.HasStarted)
			{
				_logger.LogWarning("The response has already started. Skipping error response.");
				return;
			}

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;

			var errorDetails = customMessages?.Count > 0 ? string.Join(", ", customMessages) : string.Empty;
			var errorResponse = new ApiErrorResponse
			{
				Errors = ApiErrorHandler.GetDefaultErrorMessage(statusCode, errorDetails)
			};

			var options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var result = System.Text.Json.JsonSerializer.Serialize(errorResponse, options);
			await context.Response.WriteAsync(result);

		}

		private async Task LogRequestAsync(HttpContext context, Exception? ex = null)
		{
			var routeParams = context.Request.RouteValues
				.Select(r => $"{r.Key}={r.Value}")
				.ToList();

			var queryParams = context.Request.Query
				.Select(q => $"{q.Key}={q.Value}")
				.ToList();

			var requestBodyType = string.Empty;
			var requestBodyContent = string.Empty;

			try
			{
				if (context.Request.HasFormContentType)
				{
					// Handle Form Data
					requestBodyType = "Form Data";
					var formDictionary = context.Request.Form.ToDictionary(f => f.Key, f => f.Value.ToString());

					requestBodyContent = formDictionary.Count > 0
						? JsonConvert.SerializeObject(formDictionary, Formatting.Indented)
						: "[Empty Body]";

				}
				else if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
				{
					requestBodyType = "JSON Body";
					var jsonBody = await ReadRequestBodyAsync(context);
					if (string.IsNullOrWhiteSpace(jsonBody))
					{
						requestBodyContent = "[Empty Body]";
					}
					else
					{
						requestBodyContent = jsonBody;
					}
				}
				else
				{
					requestBodyType = "Unsupported or Empty Body";
					requestBodyContent = $"[No supported body content. Content-Type: {context.Request.ContentType ?? "None"}]";
				}
			}
			catch (Exception formEx)
			{
				requestBodyType = "Error Reading Body/Form";
				requestBodyContent = "[Error reading form or body]";
				_logger.LogWarning(formEx, "Error occurred while reading the request body/form.");
			}

			var logMessage = "----- Incoming Request -----\n" +
							 "Method: {Method}\n" +
							 "Path: {Path}\n" +
							 "Route: {Route}\n" +
							 "Query: {Query}\n" +
							 "{RequestBodyType}: {RequestBodyContent}\n" +
							 "---------------------------";

			if (ex != null)
			{
				// Log as Error with Exception
				_logger.LogError(ex, logMessage, context.Request.Method, context.Request.Path, string.Join(", ", routeParams), string.Join(", ", queryParams), requestBodyType, requestBodyContent);
			}
			else
			{
				_logger.LogError(logMessage, context.Request.Method, context.Request.Path, string.Join(", ", routeParams), string.Join(", ", queryParams), requestBodyType, requestBodyContent);
			}
		}

		private static async Task<string> ReadRequestBodyAsync(HttpContext context)
		{
			context.Request.Body.Position = 0;

			using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
			var body = await reader.ReadToEndAsync();

			context.Request.Body.Position = 0;

			if (body.Length > 5000)
			{
				body = body.Substring(0, 5000) + "... [Truncated]";
			}

			return string.IsNullOrWhiteSpace(body) ? "[Empty Body]" : body;
		}
	}
}
