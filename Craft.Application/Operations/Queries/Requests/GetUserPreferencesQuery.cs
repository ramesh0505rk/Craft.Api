using Craft.Application.ResponseDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Queries.Requests
{
    public class GetUserPreferencesQuery : IRequest<GetUserPreferencesDTO>
    {
        public Guid UserID { get; set; }
    }
}
