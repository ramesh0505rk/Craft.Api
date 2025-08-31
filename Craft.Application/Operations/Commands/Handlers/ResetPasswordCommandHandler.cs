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
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordDTO>
    {
        private readonly ILogger<ResetPasswordCommandHandler> _logger;
        private readonly IUserCommandRepository _userCommandRepository;

        public ResetPasswordCommandHandler(ILogger<ResetPasswordCommandHandler> logger, IUserCommandRepository userCommandRepository)
        {
            _logger = logger;
            _userCommandRepository = userCommandRepository;
        }

        public async Task<ResetPasswordDTO> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var isReset = await ResetPassword(request.UserEmail, request.NewPassword, request.Otp, cancellationToken);
            return CreateSuccessResponse(isReset);
        }

        public async Task<bool> ResetPassword(string userEmail, string newPassword, string otp, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in ResetPasswordCommandHandler.ResetPassword: Input Parameters: UserEmail = {userEmail}", userEmail);
                var isReset = await _userCommandRepository.ResetPassword(userEmail, newPassword, otp, cancellationToken);
                if (isReset == 0)
                {
                    _logger.LogError("Failed to reset password for UserEmail: {UserEmail}", userEmail);
                    throw new BadRequestCustomException(["Failed to reset password."]);
                }
                return true;
            }
            catch (Exception ex)
            {
                var inputParams = new { userEmail, newPassword, otp };
                _logger.LogError(ex, "Error thrown in ResetPasswordCommandHandler.ResetPassword. Input parameters: {InputParams}", System.Text.Json.JsonSerializer.Serialize(inputParams));
                throw;
            }
        }

        public ResetPasswordDTO CreateSuccessResponse(bool isReset)
        {
            return new ResetPasswordDTO
            {
                IsReset = isReset,
                RequestId = Guid.NewGuid().ToString(),
                ResponseMessage = "Password has been reset successfully."
            };
        }
    }
}
