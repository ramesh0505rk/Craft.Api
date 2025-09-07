using AutoMapper;
using Craft.Application.Operations.Queries.Requests;
using Craft.Application.ResponseDTOs;
using Craft.Domain.Common.ExceptionHandling;
using Craft.Domain.Entities;
using Craft.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Queries.Handlers
{
    public class GetUserPreferencesQueryHandler : IRequestHandler<GetUserPreferencesQuery, GetUserPreferencesDTO>
    {
        private readonly ILogger<GetUserPreferencesQueryHandler> _logger;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IMapper _mapper;

        public GetUserPreferencesQueryHandler(ILogger<GetUserPreferencesQueryHandler> logger, IUserQueryRepository userQueryRepository, IMapper mapper)
        {
            _logger = logger;
            _userQueryRepository = userQueryRepository;
            _mapper = mapper;
        }

        public async Task<GetUserPreferencesDTO> Handle(GetUserPreferencesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in ValidateOTPCommandHandler.ValidateOTP: Input Parameters: {Request}", JsonConvert.SerializeObject(request));
                var req = _mapper.Map<GetUserPreferencesRequest>(request);
                var result = await _userQueryRepository.GetUserPreference(req, cancellationToken);

                if (result == null)
                {
                    _logger.LogError("No preferences found for UserID: {UserID}", request.UserID);
                    throw new BadRequestCustomException(new List<string> { "No user preferences found for the given UserID." });
                }

                var response = _mapper.Map<GetUserPreferencesDTO>(result);

                return CreateSuccessResponse(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserPreferencesQueryHandler.Handle");
                throw;
            }
        }

        public GetUserPreferencesDTO CreateSuccessResponse(GetUserPreferencesDTO response)
        {
            return new GetUserPreferencesDTO
            {
                DarkTheme = response.DarkTheme,
                LightTheme = response.LightTheme,
                EnableNotification = response.EnableNotification,
                RequestId = Guid.NewGuid().ToString(),
                ResponseMessage = "User preferences retrieved successfully."
            };
        }
    }
}
