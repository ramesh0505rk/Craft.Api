using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Domain.Entities
{
    public class CheckUserExists
    {
        public bool IsUserNameExists { get; set; }
        public bool IsUserEmailExists { get; set; }
    }
}
