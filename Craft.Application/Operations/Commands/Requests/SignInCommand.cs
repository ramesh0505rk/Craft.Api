using Craft.Application.ResponseDTOs;
using Craft.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Commands.Requests
{
	public class SignInCommand : IRequest<SignInDTO>
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
