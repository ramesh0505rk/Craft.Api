using AutoMapper;
using Craft.Application.Operations.Commands.Requests;
using Craft.Application.ResponseDTOs;
using Craft.Domain.Common.ExceptionHandling;
using Craft.Domain.Entities;
using Craft.Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Craft.Application.Operations.Commands.Handlers
{
	public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInDTO>
	{
		private readonly IUserCommandRepository _userCommandRepository;
		private readonly IConfiguration _configuration;
		private readonly ILogger<SignInCommandHandler> _logger;
		private readonly IMapper _mapper;

		public SignInCommandHandler(IUserCommandRepository userCommandRepository, IConfiguration configuration, ILogger<SignInCommandHandler> logger, IMapper mapper)
		{
			_userCommandRepository = userCommandRepository;
			_configuration = configuration;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<SignInDTO> Handle(SignInCommand request, CancellationToken cancellationToken)
		{
			var userId = await ValidateUser(request, cancellationToken);
			var user = await GetUserDetails(userId, cancellationToken);

			var accessToken = GenerateToken(user);
			return CreateSuccessResponse(accessToken);
		}

		private async Task<Guid?> ValidateUser(SignInCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var userId = await _userCommandRepository.ValidateUser(request.UserName, request.Password, cancellationToken);
				if (userId == null)
				{
					_logger.LogError("Invalid credentials provided for user: {UserName}", request.UserName);
					throw new BadRequestCustomException(["Invalid username or password."]);
				}
				return userId;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error thrown in SignInCommandHandler.ValidateUser. Input parameters: {InputParams}", request);
				throw;
			}
		}

		private async Task<User> GetUserDetails(Guid? userId, CancellationToken cancellationToken)
		{
			try
			{
				var userDetails = await _userCommandRepository.GetUserDetails(userId, cancellationToken);
				return userDetails;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error thrown in SignInCommandHandler.GetUserDetails. Input parameters: {InputParams}", userId);
				throw;
			}
		}

		private string GenerateToken(User user)
		{
			var jwtSettings = _configuration.GetSection("Jwt");
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim("UserID",user.UserID+""),
				new Claim("UserName",user.UserName),
				new Claim("FirstName",user.FirstName),
				new Claim("LastName",user.LastName),
				new Claim("UserEmail",user.UserEmail)
			};

			var token = new JwtSecurityToken(jwtSettings["Issuer"],
				jwtSettings["Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpireInMinutes"])),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private SignInDTO CreateSuccessResponse(string accessToken)
		{
			return new SignInDTO
			{
				AccessToken = accessToken,
				RequestId = Guid.NewGuid().ToString(),
				ResponseMessage = "Sign-in was successful."
			};
		}
	}
}
