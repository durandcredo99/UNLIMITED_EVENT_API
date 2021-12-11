using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepository(RepositoryContext repositoryContext, UserManager<AppUser> userManager) : base(repositoryContext)
        {
            _userManager = userManager;
        }


        public async Task<PagedList<AppUser>> GetAppUsersAsync(AppUserParameters paginationParameters)
        {
            return await Task.Run(() =>
                PagedList<AppUser>.ToPagedList(_userManager.Users,
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize)
                );
        }

        public async Task<AppUser> GetAppUserByIdAsync(string id)
        {
            return await _userManager.Users.Where(appUser => appUser.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AppUserExistAsync(AppUser appUser)
        {
            return await _userManager.Users.Where(x => x.Name == appUser.Name)
                .AnyAsync();
        }

        public async Task UpdateAppUserAsync(AppUser appUser)
        {
            await _userManager.UpdateAsync(appUser);
        }

        public async Task DeleteAppUserAsync(AppUser appUser)
        {
            await _userManager.DeleteAsync(appUser);
        }
    }
}
