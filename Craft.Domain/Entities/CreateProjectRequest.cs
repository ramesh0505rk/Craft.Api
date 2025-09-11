using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Domain.Entities
{
    public class CreateProjectRequest
    {
        public string ProjectName { get; set; }
        public Guid SuperAdmin { get; set; }
        public Guid ProjectOwner { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
