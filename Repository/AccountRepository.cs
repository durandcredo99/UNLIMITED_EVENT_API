using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : RepositoryBase<AppUser>, IAccountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<Workstation> _roleManager;
        private readonly HttpContext _httpContext;
        //private readonly string _baseURL;


        public AccountRepository(RepositoryContext repositoryContext, UserManager<AppUser> userManager, RoleManager<Workstation> roleManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(repositoryContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContext = httpContextAccessor.HttpContext;
            //_baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        public async Task<PagedList<AppUser>> GetUsersAsync(QueryStringParameters paginationParameters)
        {
            return await Task.Run(() =>
                        PagedList<AppUser>.ToPagedList(FindAll(),
                            paginationParameters.PageNumber,
                            paginationParameters.PageSize)
                        );
        }


        public async Task<Authentication> RegisterUserAsync(AppUser user, string password)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var generatedToken = await GenerateEmailConfirmationTokenAsync(user);
                var encodeToken = await EncodeTokenAsync(generatedToken);

                return new Authentication
                {
                    Token = encodeToken,
                    IsSuccess = true,
                };
            }

            return new Authentication
            {
                Message = "AppUser is not created",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }


        public async Task<Authentication> UpdateUserAsync(AppUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UpdatedAt = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var generatedToken = await GenerateEmailConfirmationTokenAsync(user);
                var encodeToken = await EncodeTokenAsync(generatedToken);

                return new Authentication
                {
                    Token = encodeToken,
                    IsSuccess = true,
                };
            }

            return new Authentication
            {
                Message = "AppUser is not created",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }


        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        }


        public async Task<string> EncodeTokenAsync(string token)
        {
            var encodedEmailToken = await Task.Run(()=>Encoding.UTF8.GetBytes(token));
            return await Task.Run(()=> WebEncoders.Base64UrlEncode(encodedEmailToken));
        }


        public async Task<string> DecodeTokenAsync(string encodedToken)
        {
            var decodedToken = await Task.Run(()=> WebEncoders.Base64UrlDecode(encodedToken));
            return await Task.Run(()=> Encoding.UTF8.GetString(decodedToken));
        }


        public async Task<Authentication> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new Authentication
                {
                    IsSuccess = false,
                    Message = "User not found !"
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, await DecodeTokenAsync(token));

            var userInfo = user.ToDictionary();

            if (result.Succeeded)
            {
                return new Authentication
                {
                    IsSuccess = true,
                    UserInfo = userInfo,
                    Message = "Email confirmed successfuly!"
                };
            }

            return new Authentication
            {
                IsSuccess = false,
                Message = "Email confirmation failed",
                ErrorDetails = result.Errors.Select(ex=>ex.Description)
            };
        }


        public async Task<Authentication> SignInAsync(LoginModel loginRequest, string password)
        {
            if (loginRequest == null) throw new ArgumentNullException(nameof(loginRequest));

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                return new Authentication
                {
                    Message = "There is no user with that email adresse",
                    IsSuccess = false,
                };
            }

            var resultSucceeded = await _userManager.CheckPasswordAsync(user, password);

            if (!resultSucceeded)
            {
                return new Authentication
                {
                    Message = "Invalid password !",
                    IsSuccess = false,
                };
            }

            var roleName = (await _userManager.GetRolesAsync(user))[0];
            var role = await _roleManager.FindByNameAsync(roleName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.Firstname),
                new Claim(ClaimTypes.Gender, user.Gender),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrEmpty(user.ImgLink)) claims.Add(new Claim(ClaimTypes.Uri, user.ImgLink));

            claims.AddRange(await _roleManager.GetClaimsAsync(role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var userInfo = user.ToDictionary();

            return new Authentication
            {
                UserInfo = userInfo,
                AppUser = user,
                Token = tokenString,
                IsSuccess = true,
                ExpireDate = token.ValidTo,
            };
        }

        public async Task SignOutAsync()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


        public async Task<Authentication> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new Authentication
                {
                    IsSuccess = false,
                    Message = "No user associated with this email",
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = await EncodeTokenAsync(token);

            var userInfo = user.ToDictionary();

            return new Authentication
            {
                Token = encodedToken,
                IsSuccess = true,
                UserInfo = userInfo,
                Message = "Password reset url has been sent to your email successfully",
            };

        }


        public async Task<Authentication> ResetPasswordAsync(ResetPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new Authentication
                {
                    IsSuccess = false,
                    Message = "No user associated with this email",
                };
            }


            var formerToken = await DecodeTokenAsync(model.Token);

            var result = await _userManager.ResetPasswordAsync(user, formerToken, model.NewPassword);
            if (result.Succeeded)
            {
                return new Authentication
                {
                    IsSuccess = true,
                    Message = "Password reset was successful!",
                };
            }

            return new Authentication
            {
                IsSuccess = false,
                Message = "Something went wrong",
                ErrorDetails = result.Errors.Select(e=>e.Description),
            };
        }


        public async Task<Authentication> AddToWorkstationAsync(AppUser user, Workstation role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return new Authentication
                {
                    IsSuccess = true,
                };
            }

            return new Authentication
            {
                Message = "Workstation not assigned",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }


        public async Task<Authentication> RemoveFromWorkstationAsync(AppUser user, Workstation role)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return new Authentication
                {
                    IsSuccess = true,
                };
            }

            return new Authentication
            {
                Message = "Workstation not removed",
                IsSuccess = false,
                ErrorDetails = result.Errors.Select(errorDescription => errorDescription.Description)
            };
        }


        public async Task<ICollection<string>> GetUsersWorkstationsAsync(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }


        public async Task<int> CountUsersAsync()
        {
            return await FindAll().CountAsync();
        }


        public async Task<string> GetUserId(ClaimsPrincipal user)
        {
            return await (Task.Run(() => _userManager.GetUserId(user)));
            //return await _userManager.GetUserIdAsync(user);
        }
    }

    public static class AppUserExtension
    {
        public static Dictionary<string, string> ToDictionary(this AppUser appUser)
        {
            return new Dictionary<string, string>
                {
                    { "ImgUrl", appUser.ImgLink },
                    { "Name", appUser.Name +" "+appUser.Firstname},
                    { "Email", appUser.Email },
                    { "PhoneNumber", appUser.PhoneNumber },
                    { "CreatedAt", appUser.CreatedAt.ToString()},
                    { "UpdatedAt", appUser.UpdatedAt.ToString()},
                    { "DisabledAt", appUser.DisabledAt.ToString()},
                };
        }
    }
}
