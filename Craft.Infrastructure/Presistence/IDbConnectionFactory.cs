using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Infrastructure.Presistence
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> GetOpenConnection(CancellationToken cancellationToken);
    }
}
