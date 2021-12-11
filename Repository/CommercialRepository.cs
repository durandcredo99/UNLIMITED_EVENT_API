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
    public class CommercialRepository : RepositoryBase<Commercial>, ICommercialRepository
    {
        private ISortHelper<Commercial> _sortHelper;

        public CommercialRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Commercial> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Commercial>> GetCommercialsAsync(CommercialParameters commercialParameters)
        {
            var commercials = Enumerable.Empty<Commercial>().AsQueryable();

            ApplyFilters(ref commercials, commercialParameters);

            PerformSearch(ref commercials, commercialParameters.SearchTerm);

            var sortedCommercials = _sortHelper.ApplySort(commercials, commercialParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Commercial>.ToPagedList
                (
                    sortedCommercials,
                    commercialParameters.PageNumber,
                    commercialParameters.PageSize)
                );
        }

        public async Task<Commercial> GetCommercialByIdAsync(Guid id)
        {
            return await FindByCondition(commercial => commercial.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CommercialExistAsync(Commercial commercial)
        {
            return await FindByCondition(x => x.Title == commercial.Title)
                .AnyAsync();
        }

        public async Task CreateCommercialAsync(Commercial commercial)
        {
            await CreateAsync(commercial);
        }

        public async Task UpdateCommercialAsync(Commercial commercial)
        {
            await UpdateAsync(commercial);
        }

        public async Task UpdateCommercialAsync(IEnumerable<Commercial> commercials)
        {
            await UpdateAsync(commercials);
        }

        public async Task DeleteCommercialAsync(Commercial commercial)
        {
            await DeleteAsync(commercial);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Commercial> commercials, CommercialParameters commercialParameters)
        {
            commercials = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(commercialParameters.AppUserId))
            {
                commercials = commercials.Where(x => x.AppUserId == commercialParameters.AppUserId);
            }

            if (commercialParameters.MinBirthday != null)
            {
                commercials = commercials.Where(x => x.Birthday >= commercialParameters.MinBirthday);
            }

            if (commercialParameters.MaxBirthday != null)
            {
                commercials = commercials.Where(x => x.Birthday < commercialParameters.MaxBirthday);
            }

            if (commercialParameters.MinCreateAt != null)
            {
                commercials = commercials.Where(x => x.CreateAt >= commercialParameters.MinCreateAt);
            }

            if (commercialParameters.MaxCreateAt != null)
            {
                commercials = commercials.Where(x => x.CreateAt < commercialParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Commercial> commercials, string searchTerm)
        {
            if (!commercials.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            commercials = commercials.Where(x => x.Title.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
