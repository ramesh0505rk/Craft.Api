using Craft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Infrastructure.Interfaces
{
    public interface IUserQueryRepository
    {
        Task<List<GetUserListResponse>> GetUserList(GetUserListRequest request, CancellationToken cancellationToken);
        Task<GetUserPreferencesResponse> GetUserPreference(GetUserPreferencesRequest request, CancellationToken cancellationToken);
    }
}
