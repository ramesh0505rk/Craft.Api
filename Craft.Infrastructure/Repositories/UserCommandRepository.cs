using Azure.Core;
using Craft.Domain.Entities;
using Craft.Infrastructure.Interfaces;
using Craft.Infrastructure.Presistence;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Craft.Infrastructure.Repositories
{
    public class UserCommandRepository : IUserCommandRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<UserCommandRepository> _logger;

        public UserCommandRepository(IDbConnectionFactory dbConnectionFactory, ILogger<UserCommandRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<Guid?> ValidateUser(string userName, string password, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in UserCommandRepository.ValidateUser: Input Parameters: UserName = {userName}", userName);
                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);
                parameters.Add("@Password", password);

                var result = await connection.QueryFirstAsync<Guid?>(
                    DBQueries.ValidateUser,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;

            }
            catch (Exception ex)
            {
                var inputParams = new { userName };
                _logger.LogError(ex, "Error thrown in UserCommandRepository.ValidateUser. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

        public async Task<User> GetUserDetails(Guid? userId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in UserCommandRepository.GetUserDetails: Input Parameters: UserId = {userId}", userId);
                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var result = await connection.QueryFirstOrDefaultAsync<User>(
                    DBQueries.GetUserDetails,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                var inputParams = new { userId };
                _logger.LogError(ex, "Error thrown in UserCommandRepository.GetUserDetails. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

        public async Task<CheckUserExists> CheckUserExists(string userName, string userEmail, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in UserCommandRepository.CheckUserExists: Input Parameters: UserName = {userName}, UserEmail = {userEmail}", userName, userEmail);
                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);
                parameters.Add("@UserEmail", userEmail);

                var result = await connection.QueryFirstOrDefaultAsync<CheckUserExists>(
                    DBQueries.CheckUserExists,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                var inputParams = new { userName, userEmail };
                _logger.LogError(ex, "Error thrown in UserCommandRepository.CheckUserExists. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

        public async Task<User> InsertUser(SignUpRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in UserCommandRepository.InsertUser: Input Parameters: {InputParams}", JsonConvert.SerializeObject(request));
                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@UserID", Guid.NewGuid());
                parameters.Add("@UserName", request.UserName);
                parameters.Add("@Password", request.Password);
                parameters.Add("@FirstName", request.FirstName);
                parameters.Add("@LastName", request.LastName);
                parameters.Add("@UserEmail", request.UserEmail);

                var result = await connection.QueryFirstOrDefaultAsync<User>(
                    DBQueries.CreateUser,
                    parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown in UserCommandRepository.InsertUser. Input parameters: {InputParams}", JsonConvert.SerializeObject(request));
                throw;
            }
        }

        public async Task<int> RequestOTP(string userEmail, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in UserCommandRepository.RequestOTP: Input Parameters: UserEmail = {userEmail}", userEmail);
                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@UserEmail", userEmail);

                var result = await connection.ExecuteScalarAsync<int>(
                    DBQueries.RequestOTP,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                var inputParams = new { userEmail };
                _logger.LogError(ex, "Error thrown in UserCommandRepository.RequestOTP. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

    }
}
