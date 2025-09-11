using Craft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Infrastructure.Interfaces
{
    public interface IProjectCommandRepository
    {
        Task<int> CreateProject(CreateProjectRequest request, CancellationToken cancellationToken);
    }
}
