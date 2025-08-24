using AutoMapper;
using Craft.Application.Operations.Queries.Requests;
using Craft.Domain.Entities;
using Craft.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Queries.Handlers
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<GetUserListResponse>>
    {
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly ILogger<GetUserListQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(IUserQueryRepository userQueryRepository, ILogger<GetUserListQueryHandler> logger, IMapper mapper)
        {
            _userQueryRepository = userQueryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<GetUserListResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Information in GetUserListQueryHandler: Input Parameters: {Request}", JsonConvert.SerializeObject(request));
        
            var req = _mapper.Map<GetUserListRequest>(request);
            var result = await _userQueryRepository.GetUserList(req, cancellationToken);
            return result;
        }
    }
}
