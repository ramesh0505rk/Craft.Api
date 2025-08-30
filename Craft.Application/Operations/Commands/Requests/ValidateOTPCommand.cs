using Craft.Application.ResponseDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Commands.Requests
{
    public class ValidateOTPCommand : IRequest<ValidateOTPDTO>
    {
        public string UserEmail { get; set; }
        public string OTP { get; set; }
    }
}
