using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Domain.Entities
{
	public class SignInRequest
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
