using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAccountRepository
    {
        Task<PagedList<AppUser>> GetUsersAsync(QueryStringParameters paginationParameters);
        Task<int> CountUsersAsync();
        Task<Authentication> RegisterUserAsync(AppUser appUser, string password);
        Task<Authentication> ConfirmEmailAsync(string userId, string token);
        Task<Authentication> UpdateUserAsync(AppUser appUser);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<string> EncodeTokenAsync(string token);
        Task<string> DecodeTokenAsync(string encodedToken);
        Task<Authentication> SignInAsync(LoginModel loginRequest, string password);
        Task SignOutAsync();
        Task<Authentication> ForgetPasswordAsync(string email);
        Task<Authentication> ResetPasswordAsync(ResetPassword model);
        Task<string> GetUserId (ClaimsPrincipal user);

        Task<ICollection<string>> GetUsersWorkstationsAsync(AppUser user);
        Task<Authentication> AddToWorkstationAsync(AppUser user, Workstation workstation);
        Task<Authentication> RemoveFromWorkstationAsync(AppUser user, Workstation workstation);
    }
}
