using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.ResponseDTOs
{
    public class ValidateOTPDTO : BaseDTO
    {
        public bool IsValid { get; set; }
    }
}
