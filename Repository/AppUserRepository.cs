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

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<AppUser> appUsers, AppUserParameters appUserParameters)
        {
            appUsers = FindAll()
                .Include(x => x.Events);


            if (!string.IsNullOrEmpty(appUserParameters.WithRoleName))
            {
                var taskAppUsers = Task.Run(async () => await _userManager.GetUsersInRoleAsync(appUserParameters.WithRoleName));
                appUsers = taskAppUsers.Result.AsQueryable();
            }

            if (appUserParameters.DisplayOrganisatorOnly)
            {
                var taskAppUsers = Task.Run(async () => await _userManager.GetUsersInRoleAsync("Organisateur"));
                appUsers = taskAppUsers.Result.AsQueryable();
            }

            //if (appUserParameters.OfFormationLevelId != new Guid())
            //{
            //    appUsers = appUsers.Where(x => x.Subscriptions.Any(x => x.FormationLevelId == appUserParameters.OfFormationLevelId));
            //}

            //if (appUserParameters.OfFormationId != new Guid())
            //{
            //    appUsers = appUsers.Where(x => x.Subscriptions.Any(x => x.FormationLevel.FormationId == appUserParameters.OfFormationId));
            //}

            //if (appUserParameters.FromUniversityId != new Guid())
            //{
            //    appUsers = appUsers.Where(x => x.Subscriptions.Any(x => x.FormationLevel.Formation.UniversityId == appUserParameters.FromUniversityId));
            //}

            //if (!string.IsNullOrEmpty(appUserParameters.ManagedByAppUserId))
            //{
            //    appUsers = appUsers.Where(x => x.Subscriptions.Any(x => x.FormationLevel.Formation.University.AppUserId == appUserParameters.ManagedByAppUserId));
            //}

            //if (appUserParameters.DisplayStudentOnly)
            //{
            //    var taskAppUsers = Task.Run(async () => await _userManager.GetUsersInRoleAsync("Etudiant"));
            //    appUsers = taskAppUsers.Result.AsQueryable();
            //}

            //if (!string.IsNullOrEmpty(appUserParameters.WhitchPaidFrom))
            //{
            //    var paidFrom = DateTime.Parse(appUserParameters.WhitchPaidFrom);
            //    appUsers = appUsers.Where(x => x.Payments.Any(x => x.PaidAt >= paidFrom));
            //}

            //if (!string.IsNullOrEmpty(appUserParameters.WhitchPaidTo))
            //{
            //    var paidTo = DateTime.Parse(appUserParameters.WhitchPaidTo);
            //    appUsers = appUsers.Where(x => x.Payments.Any(x => x.PaidAt <= paidTo));
            //}
        }

        private void PerformSearch(ref IQueryable<AppUser> appUsers, string searchTerm)
        {
            if (!appUsers.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            appUsers = appUsers.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion
    }
}
