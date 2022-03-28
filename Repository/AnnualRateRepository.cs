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
    public class AnnualRateRepository : RepositoryBase<AnnualRate>, IAnnualRateRepository
    {
        private ISortHelper<AnnualRate> _sortHelper;

        public AnnualRateRepository(
            RepositoryContext repositoryContext,
            ISortHelper<AnnualRate> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<AnnualRate>> GetAnnualRatesAsync(AnnualRateParameters annualRateParameters)
        {
            var annualRate = Enumerable.Empty<AnnualRate>().AsQueryable();

            ApplyFilters(ref annualRate, annualRateParameters);

            PerformSearch(ref annualRate, annualRateParameters.SearchTerm);

            var sortedAnnualRates = _sortHelper.ApplySort(annualRate, annualRateParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<AnnualRate>.ToPagedList
                (
                    sortedAnnualRates,
                    annualRateParameters.PageNumber,
                    annualRateParameters.PageSize)
                );
        }

        public async Task<AnnualRate> GetAnnualRateByIdAsync(Guid id)
        {
            return await FindByCondition(annualRate => annualRate.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AnnualRateExistAsync(AnnualRate annualRate)
        {
            return await FindByCondition(x => x.StartingDate == annualRate.StartingDate)
                .AnyAsync();
        }

        public async Task<AnnualRate> GetOpenAnnualRateAsync()
        {
            /*
            return await FindByCondition(annualRate => annualRate.IsOpen)
                .FirstOrDefaultAsync();
            */

            return await FindAll().FirstOrDefaultAsync();
        }

        public async Task CreateAnnualRateAsync(AnnualRate annualRate)
        {
            await CreateAsync(annualRate);
        }

        public async Task UpdateAnnualRateAsync(AnnualRate annualRate)
        {
            await UpdateAsync(annualRate);
        }

        public async Task UpdateAnnualRateAsync(IEnumerable<AnnualRate> annualRate)
        {
            await UpdateAsync(annualRate);
        }

        public async Task DeleteAnnualRateAsync(AnnualRate annualRate)
        {
            await DeleteAsync(annualRate);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<AnnualRate> annualRate, AnnualRateParameters annualRateParameters)
        {
            annualRate = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(annualRateParameters.AppUserId))
            {
                annualRate = annualRate.Where(x => x.AppUserId == annualRateParameters.AppUserId);
            }

            if (annualRateParameters.MinBirthday != null)
            {
                annualRate = annualRate.Where(x => x.Birthday >= annualRateParameters.MinBirthday);
            }

            if (annualRateParameters.MaxBirthday != null)
            {
                annualRate = annualRate.Where(x => x.Birthday < annualRateParameters.MaxBirthday);
            }

            if (annualRateParameters.MinCreateAt != null)
            {
                annualRate = annualRate.Where(x => x.CreateAt >= annualRateParameters.MinCreateAt);
            }

            if (annualRateParameters.MaxCreateAt != null)
            {
                annualRate = annualRate.Where(x => x.CreateAt < annualRateParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<AnnualRate> annualRate, string searchTerm)
        {
            if (!annualRate.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            //annualRate = annualRate.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
