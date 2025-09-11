using Craft.Application.ResponseDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Commands.Requests
{
    public class CreateProjectCommand : IRequest<CreateProjectDTO>
    {
        public string ProjectName { get; set; }
        public Guid SuperAdmin { get; set; }
        public Guid ProjectOwner { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
