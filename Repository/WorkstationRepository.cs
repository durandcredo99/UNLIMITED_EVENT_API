using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WorkstationRepository : RepositoryBase<Workstation>, IWorkstationRepository
    {
        private ISortHelper<Workstation> _sortHelper;
        private readonly RoleManager<Workstation> _roleManager;


        public WorkstationRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Workstation> sortHelper,
            RoleManager<Workstation> roleManager
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _roleManager = roleManager;
        }


        public async Task<PagedList<Workstation>> GetWorkstationsAsync(WorkstationParameters workstationParameters)
        {
            var workstations = Enumerable.Empty<Workstation>().AsQueryable();

            ApplyFilters(ref workstations, workstationParameters);

            PerformSearch(ref workstations, workstationParameters.SearchTerm);

            var sortedWorkstations = _sortHelper.ApplySort(workstations, workstationParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Workstation>.ToPagedList
                (
                    sortedWorkstations,
                    workstationParameters.PageNumber,
                    workstationParameters.PageSize)
                );
        }


        public async Task<Workstation> GetWorkstationByIdAsync(string id)
        {
            return await _roleManager.Roles.Where(workstation => workstation.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<Workstation> GetWorkstationByNameAsync(string workstationName)
        {
            return await _roleManager.Roles.Where(workstation => workstation.Name.Equals(workstationName))
                .FirstOrDefaultAsync();
        }


        public async Task<IList<Claim>> GetClaimsAsync(Workstation workstation)
        {
            return await _roleManager.GetClaimsAsync(workstation);
        }



        public async Task<bool> AddClaimsSucceededAsync(Workstation workstation, IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                var result = await _roleManager.AddClaimAsync(workstation, claim);
                if (!result.Succeeded) return false;
            }
            return true;
        }



        public async Task<bool> RemoveClaimsSucceededAsync(Workstation workstation, IList<Claim> claims)
        {
            foreach (var claim in claims)
            {
                var result = await _roleManager.RemoveClaimAsync(workstation, claim);
                if (!result.Succeeded) return false;
            }
            return true;
        }


        public async Task<bool> WorkstationExistAsync(Workstation workstation)
        {
            return await FindByCondition(x => x.Name == workstation.Name)
                .AnyAsync();
        }


        public async Task CreateWorkstationAsync(Workstation workstation)
        {
            await _roleManager.CreateAsync(workstation);
        }

        public async Task UpdateWorkstationAsync(Workstation workstation)
        {
            await _roleManager.UpdateAsync(workstation);
        }

        public async Task DeleteWorkstationAsync(Workstation workstation)
        {
            await _roleManager.DeleteAsync(workstation);
        }


        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Workstation> workstations, WorkstationParameters workstationParameters)
        {
            workstations = _roleManager.Roles;

            /*
            if (!string.IsNullOrWhiteSpace(workstationParameters.ManagedByAppUserId))
            {
                workstations = workstations.Where(x => x.AppUserId == workstationParameters.ManagedByAppUserId);
            }

            if (workstationParameters.showValidatedOnesOnly)
            {
                workstations = workstations.Where(x => x.ValiddatedAt !=null);
            }

            if (workstationParameters.MaxBirthday != null)
            {
                workstations = workstations.Where(x => x.Birthday < workstationParameters.MaxBirthday);
            }

            if (workstationParameters.MinCreateAt != null)
            {
                workstations = workstations.Where(x => x.CreateAt >= workstationParameters.MinCreateAt);
            }

            if (workstationParameters.MaxCreateAt != null)
            {
                workstations = workstations.Where(x => x.CreateAt < workstationParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Workstation> workstations, string searchTerm)
        {
            if (!workstations.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            workstations = workstations.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
