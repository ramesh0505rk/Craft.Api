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

        public async Task<SignUpDTO> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            await CheckUserExists(request, cancellationToken);
            var user = await InsertUser(request, cancellationToken);
            var accessToken = GenerateToken(user);

            return CreateSuccessResponse(accessToken);
        }

        private async Task CheckUserExists(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userExists = await _userCommandRepository.CheckUserExists(request.UserName, request.UserEmail, cancellationToken);
                if (userExists.IsUserNameExists)
                {
                    _logger.LogError("Username already exists: {UserName}", request.UserName);
                    throw new BadRequestCustomException(["Username already exists."]);
                }
                if (userExists.IsUserEmailExists)
                {
                    _logger.LogError("User with this Email already exists: {UserEmail}", request.UserEmail);
                    throw new BadRequestCustomException(["User with this Email already exists."]);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown in SignUpCommandHandler.CheckUserExists. Input parameters: {InputParams}", request);
                throw;
            }
        }

        private async Task<User> InsertUser(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var req = _mapper.Map<SignUpRequest>(request);
                var user = await _userCommandRepository.InsertUser(req, cancellationToken);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown in SignUpCommandHandler.InsertUser. Input parameters: {InputParams}", request);
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

        private SignUpDTO CreateSuccessResponse(string accessToken)
        {
            return new SignUpDTO
            {
                AccessToken = accessToken,
                RequestId = Guid.NewGuid().ToString(),
                ResponseMessage = "Sign-up was successful."
            };
        }
    }
}
