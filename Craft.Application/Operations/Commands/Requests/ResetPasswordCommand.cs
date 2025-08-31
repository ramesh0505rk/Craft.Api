using Craft.Application.ResponseDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Commands.Requests
{
    public class ResetPasswordCommand : IRequest<ResetPasswordDTO>
    {
        public string UserEmail { get; set; }
        public string NewPassword { get; set; }
        public string Otp { get; set; }
    }
}
