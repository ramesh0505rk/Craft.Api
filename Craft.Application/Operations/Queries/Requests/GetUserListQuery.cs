using Craft.Application.ResponseDTOs;
using Craft.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Queries.Requests
{
	public class GetUserListQuery : IRequest<GetUserListDTO>
	{
		public int Page { get; set; }
		public int PageSize { get; set; }
		public string? SearchString { get; set; }
	}
}
