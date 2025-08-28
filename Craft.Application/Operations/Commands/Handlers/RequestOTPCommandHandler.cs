using AutoMapper;
using Craft.Application.Operations.Commands.Requests;
using Craft.Application.ResponseDTOs;
using Craft.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Commands.Handlers
{
    public class RequestOTPCommandHandler : IRequestHandler<RequestOTPCommand, RequestOTPDTO>
    {
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly ILogger<RequestOTPCommandHandler> _logger;

        public RequestOTPCommandHandler(IUserCommandRepository userCommandRepository, ILogger<RequestOTPCommandHandler> logger)
        {
            _userCommandRepository = userCommandRepository;
            _logger = logger;
        }

        public async Task<RequestOTPDTO> Handle(RequestOTPCommand request, CancellationToken cancellationToken)
        {
            var otp = await RequestOTP(request.UserEmail,cancellationToken);
        }

        public async Task<string> RequestOTP(string UserEmail, CancellationToken cancellationToken)
        {
            //var otp = 
        }
    }
}
