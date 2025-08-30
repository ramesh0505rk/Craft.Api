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
using System.Text.Json;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Commands.Handlers
{
    public class ValidateOTPCommandHandler : IRequestHandler<ValidateOTPCommand, ValidateOTPDTO>
    {
        private readonly ILogger<ValidateOTPCommandHandler> _logger;
        private readonly IUserCommandRepository _userCommandRepository;
        public ValidateOTPCommandHandler(ILogger<ValidateOTPCommandHandler> logger, IUserCommandRepository userCommandRepository)
        {
            _logger = logger;
            _userCommandRepository = userCommandRepository;
        }
        public async Task<ValidateOTPDTO> Handle(ValidateOTPCommand request, CancellationToken cancellationToken)
        {
            var isValid = await ValidateOTP(request.UserEmail, request.OTP, cancellationToken);
            return CreateSuccessResponse(isValid);
        }

        public async Task<bool> ValidateOTP(string userEmail, string otp, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in ValidateOTPCommandHandler.ValidateOTP: Input Parameters: UserEmail = {userEmail}", userEmail);
                var isValid = await _userCommandRepository.ValidateOTP(userEmail, otp, cancellationToken);

                if (isValid == 0)
                {
                    _logger.LogError("Invalid OTP for UserEmail: {UserEmail}", userEmail);
                    throw new BadRequestCustomException(new List<string> { "Invalid OTP." });
                }

                return true;
            }
            catch (Exception ex)
            {
                var inputParams = new { userEmail, otp };
                _logger.LogError(ex, "Error thrown in ValidateOTPCommandHandler.ValidateOTP. Input parameters: {InputParams}", JsonSerializer.Serialize(inputParams));
                throw;
            }
        }

        public ValidateOTPDTO CreateSuccessResponse(bool isValId)
        {
            return new ValidateOTPDTO
            {
                IsValid = isValId,
                RequestId = Guid.NewGuid().ToString(),
                ResponseMessage = "OTP is valid.",
            };
        }
    }
}
