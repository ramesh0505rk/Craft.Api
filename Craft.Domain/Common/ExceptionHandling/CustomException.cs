using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Domain.Common.ExceptionHandling
{
	public class BadRequestCustomException : Exception
	{
		public List<string> Errors { get; }
		public BadRequestCustomException(List<string> errors) : base("Validation Failed")
		{
			Errors = errors;
		}
	}

	public class UnAuthorizedCustomException : Exception
	{
		public List<string> Errors { get; }
		public UnAuthorizedCustomException(List<string> errors) : base("UnAuthorized")
		{
			Errors = errors;
		}
	}
}
