using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Infrastructure
{
    public class DBQueries
    {
        public static string CreateUser = "Craft.dbo.Usp_CreateUser";
        public const string GetUserList = "Usp_GetUserList";
        public const string ValidateUser = "Usp_ValidateUserWithCreds";
        public const string GetUserDetails = "Usp_GetUserDetails";
        public const string CheckUserExists = "Usp_CheckUserExists";
        public const string RequestOTP = "Usp_RequestOTP";
        public const string ValidateOTP = "Usp_ValidateOtp";
        public const string ResetPassword = "Usp_ResetUserPassword";
        public const string GetUserPreferences = "SELECT * FROM ufn_GetUserPreferencesByUserID(@UserID)";
    }
}
