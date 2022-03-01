using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
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
        Task<Authentication> UpdateUserAsync(AppUser appUser);
        Task<Authentication> ConfirmEmailAsync(string userId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<string> EncodeTokenAsync(string token);
        Task<string> DecodeTokenAsync(string encodedToken);
        Task<Authentication> SignInAsync(LoginModel loginRequest, string password);
        Task SignOutAsync();
        Task<AppUser> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<Authentication> ForgetPasswordAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string password);
        Task<bool> IsEmailConfirmedAsync(AppUser appUser);
        //Task<Authentication> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<string> GetUserId(ClaimsPrincipal user);

        Task<ICollection<string>> GetUsersWorkstationsAsync(AppUser user);
        Task<Authentication> AddToWorkstationAsync(AppUser user, Workstation workstation);
        Task<Authentication> RemoveFromWorkstationAsync(AppUser user, Workstation workstation);

        /*
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
            Task<AppUser> FindByEmailAsync(string email);
            Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
            Task<AuthenticationResponse> ForgetPasswordAsync(string email);
            Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string password);
            Task<bool> IsEmailConfirmedAsync(AppUser appUser);
            Task<string> GetUserId (ClaimsPrincipal user);

            Task<ICollection<string>> GetUsersWorkstationsAsync(AppUser user);
            Task<Authentication> AddToWorkstationAsync(AppUser user, Workstation workstation);
            Task<Authentication> RemoveFromWorkstationAsync(AppUser user, Workstation workstation);
         */
    }
}
