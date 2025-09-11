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
using System.Threading.Tasks;

namespace Craft.Infrastructure.Repositories
{
    public class ProjectCommandRepository : IProjectCommandRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<ProjectCommandRepository> _logger;

        public ProjectCommandRepository(IDbConnectionFactory dbConnectionFactory, ILogger<ProjectCommandRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<int> CreateProject(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in ProjectCommandRepository.CreateProject: Input Parameters: {Request}", JsonConvert.SerializeObject(request));
                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                Guid projectId = Guid.NewGuid();


                var parameters = new DynamicParameters();
                parameters.Add("@ProjectId", projectId);
                parameters.Add("@ProjectName", request.ProjectName);
                parameters.Add("@SuperAdmin", request.SuperAdmin);
                parameters.Add("@ProjectOwner", request.ProjectOwner);
                parameters.Add("@CreatedDate", DateTime.UtcNow);
                parameters.Add("@CreatedBy", request.CreatedBy);
                parameters.Add("@ModifiedDate", DateTime.UtcNow);
                parameters.Add("@ModifiedBy", request.CreatedBy);

                var result = await connection.ExecuteScalarAsync<int>(
                    DBQueries.CreateProject,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown in ProjectCommandRepository.CreateProject. Input parameters: {Request}", JsonConvert.SerializeObject(request));
                throw;
            }
        }
    }
}
