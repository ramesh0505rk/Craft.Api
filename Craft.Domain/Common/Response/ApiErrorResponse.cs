using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Craft.Domain.Common.Response
{
    public class ApiErrorResponse
    {
        [JsonPropertyName("errors")]
        public ApiError Errors { get; set; }
    }

    public class ApiError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("details")]
        public string Details { get; set; }
    }
}
