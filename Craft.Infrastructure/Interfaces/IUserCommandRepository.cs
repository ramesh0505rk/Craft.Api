using Craft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Infrastructure.Interfaces
{
    public interface IUserCommandRepository
    {
        Task<Guid?> ValidateUser(string userName, string password, CancellationToken cancellationToken);
        Task<User> GetUserDetails(Guid? userId, CancellationToken cancellationToken);
        Task<CheckUserExists> CheckUserExists(string userName, string userEmail, CancellationToken cancellationToken);
        Task<User> InsertUser(SignUpRequest request, CancellationToken cancellationToken);
        Task<int> RequestOTP(string userEmail, CancellationToken cancellationToken);
        Task<int> ValidateOTP(string userEmail, string otp, CancellationToken cancellationToken);
        Task<int> ResetPassword(string userEmail, string newPassword, string otp, CancellationToken cancellationToken);
    }
}
