using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.ResponseDTOs
{
	public class BaseDTO
	{
		/// <summary>
		/// Gets or sets the unique request identifier.
		/// </summary>
		[JsonProperty("requestId")]
		public string RequestId { get; set; }

		/// <summary>
		/// Gets or sets the response message.
		/// </summary>
		[JsonProperty("responseMessage")]
		public string ResponseMessage { get; set; }
	}
}
