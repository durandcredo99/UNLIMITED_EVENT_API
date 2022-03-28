using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PromoteRepository : RepositoryBase<Promote>, IPromoteRepository
    {
        private ISortHelper<Promote> _sortHelper;

        public PromoteRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Promote> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Promote>> GetPromotesAsync(PromoteParameters promoteParameters)
        {
            var promote = Enumerable.Empty<Promote>().AsQueryable();

            ApplyFilters(ref promote, promoteParameters);

            PerformSearch(ref promote, promoteParameters.SearchTerm);

            var sortedPromotes = _sortHelper.ApplySort(promote, promoteParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Promote>.ToPagedList
                (
                    sortedPromotes,
                    promoteParameters.PageNumber,
                    promoteParameters.PageSize)
                );
        }
        public async Task<int> GetNextNumberAsync(PromoteParameters promoteParameters)
        {
            var promotesCount = 0;

            var promotes = Enumerable.Empty<Promote>().AsQueryable();
            ApplyFilters(ref promotes, promoteParameters);

            if (promotes.Any())
            {
                promotesCount = await promotes.MaxAsync(x => x.Position);
            }

            promotesCount++;

            return promotesCount;
        }

        public async Task<Promote> GetPromoteByIdAsync(Guid id)
        {
            return await FindByCondition(promote => promote.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PromoteExistAsync(Promote promote)
        {
            return await FindByCondition(x => x.Position == promote.Position)
                .AnyAsync();
        }

        public async Task CreatePromoteAsync(Promote promote)
        {
            await CreateAsync(promote);
        }

        public async Task UpdatePromoteAsync(Promote promote)
        {
            await UpdateAsync(promote);
        }

        public async Task UpdatePromoteAsync(IEnumerable<Promote> promote)
        {
            await UpdateAsync(promote);
        }

        public async Task DeletePromoteAsync(Promote promote)
        {
            await DeleteAsync(promote);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Promote> promote, PromoteParameters promoteParameters)
        {
            promote = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(promoteParameters.AppUserId))
            {
                promote = promote.Where(x => x.AppUserId == promoteParameters.AppUserId);
            }

            if (promoteParameters.MinBirthday != null)
            {
                promote = promote.Where(x => x.Birthday >= promoteParameters.MinBirthday);
            }

            if (promoteParameters.MaxBirthday != null)
            {
                promote = promote.Where(x => x.Birthday < promoteParameters.MaxBirthday);
            }

            if (promoteParameters.MinCreateAt != null)
            {
                promote = promote.Where(x => x.CreateAt >= promoteParameters.MinCreateAt);
            }

            if (promoteParameters.MaxCreateAt != null)
            {
                promote = promote.Where(x => x.CreateAt < promoteParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Promote> promote, string searchTerm)
        {
            if (!promote.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            //promote = promote.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
