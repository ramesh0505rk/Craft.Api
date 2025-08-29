using AutoMapper;
using Craft.Application.Operations.Commands.Requests;
using Craft.Application.ResponseDTOs;
using Craft.Domain.Common.ExceptionHandling;
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
            var otp = await RequestOTP(request.UserEmail, cancellationToken);

            return CreateSuccessResponse(otp);
        }

        public async Task<string> RequestOTP(string UserEmail, CancellationToken cancellationToken)
        {
            var otp = await _userCommandRepository.RequestOTP(UserEmail, cancellationToken);
            if (otp == 0)
            {
                _logger.LogError("Error in generating OTP for email: {UserEmail}", UserEmail);
                throw new BadRequestCustomException(new List<string> { "Error in generating OTP. Invalid email." });
            }
            return otp.ToString();
        }

        public RequestOTPDTO CreateSuccessResponse(string otp)
        {
            return new RequestOTPDTO
            {
                OTP = otp,
                RequestId = Guid.NewGuid().ToString(),
                ResponseMessage = "OTP generated successfully."
            };
        }
    }
}
