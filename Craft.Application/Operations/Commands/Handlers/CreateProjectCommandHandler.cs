using AutoMapper;
using Craft.Application.Operations.Commands.Requests;
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

namespace Craft.Application.Operations.Commands.Handlers
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectDTO>
    {
        private readonly ILogger<CreateProjectCommandHandler> _logger;
        private readonly IProjectCommandRepository _projectCommandRepository;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(ILogger<CreateProjectCommandHandler> logger, IProjectCommandRepository projectCommandRepository, IMapper mapper)
        {
            _logger = logger;
            _projectCommandRepository = projectCommandRepository;
            _mapper = mapper;
        }

        public async Task<CreateProjectDTO> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            await CreateProject(request, cancellationToken);
            return CreateSuccessResponse(true);
        }

        public async Task CreateProject(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in CreateProjectCommandHandler.CreateProject: Input Parameters: {Request}", JsonConvert.SerializeObject(request));

                var req = _mapper.Map<CreateProjectRequest>(request);
                var isCreated = await _projectCommandRepository.CreateProject(req, cancellationToken);

                if (isCreated < 0)
                    throw new BadRequestCustomException(new List<string> { "Failed to create the project. Try again later." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown in CreateProjectCommandHandler.CreateProject. Input parameters: {Request}", JsonConvert.SerializeObject(request));
                throw;
            }
        }

        public CreateProjectDTO CreateSuccessResponse(bool isCreated)
        {
            return new CreateProjectDTO
            {
                IsCreated = isCreated,
                RequestId = Guid.NewGuid().ToString(),
                ResponseMessage = "Project has been created successfully."
            };
        }
    }
}
