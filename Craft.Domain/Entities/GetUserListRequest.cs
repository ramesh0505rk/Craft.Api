using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Domain.Entities
{
    public class GetUserListRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? SearchString { get; set; }
    }
}
