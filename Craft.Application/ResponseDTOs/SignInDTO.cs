using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.ResponseDTOs
{
	public class SignInDTO : BaseDTO
	{
		[JsonProperty("accessToken")]
		public string AccessToken { get; set; }
	}
}
