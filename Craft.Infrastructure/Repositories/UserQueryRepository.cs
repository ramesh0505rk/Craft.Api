using Craft.Domain.Entities;
using Craft.Infrastructure.Interfaces;
using Craft.Infrastructure.Presistence;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;

namespace Craft.Infrastructure.Repositories
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<UserQueryRepository> _logger;
        public UserQueryRepository(IDbConnectionFactory dbConnectionFactory, ILogger<UserQueryRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }
        public async Task<List<GetUserListResponse>> GetUserList(GetUserListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in UserQueryRepository.GetUserList: Input Parameters: {Request}", JsonConvert.SerializeObject(request));
                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var parameters = new DynamicParameters();
                parameters.Add("@Page", request.Page);
                parameters.Add("@PageSize", request.PageSize);
                parameters.Add("@SearchString", request.SearchString);

                var result = await connection.QueryAsync<GetUserListResponse>(
                    DBQueries.GetUserList,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
            catch (Exception ex)
            {
                var inputParams = new { request };
                _logger.LogError(ex, "Error thrown in UserQueryRepository.GetUserList. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }
    }
}
