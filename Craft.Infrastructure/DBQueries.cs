using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Infrastructure
{
    public class DBQueries
    {
        public const string GetUserList = "Usp_GetUserList";
        public const string ValidateUser = "Usp_ValidateUserWithCreds";
        public const string GetUserDetails = "Usp_GetUserDetails";
	}
}
