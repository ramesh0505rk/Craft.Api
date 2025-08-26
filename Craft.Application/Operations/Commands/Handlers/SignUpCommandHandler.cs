using AutoMapper;
using Craft.Application.Operations.Commands.Requests;
using Craft.Application.ResponseDTOs;
using Craft.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Commands.Handlers
{
	public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpDTO>
	{
		private readonly IUserCommandRepository _userCommandRepository;
		private readonly ILogger<SignUpCommandHandler> _logger;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;

		public SignUpCommandHandler(IUserCommandRepository userCommandRepository, ILogger<SignUpCommandHandler> logger, IConfiguration configuration, IMapper mapper)
		{
			_userCommandRepository = userCommandRepository;
			_logger = logger;
			_configuration = configuration;
			_mapper = mapper;
		}

		//public async Task<SignUpDTO> Handle(SignUpCommand request, CancellationToken cancellationToken)
		//{

		//}
	}
}
