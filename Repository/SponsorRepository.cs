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
    public class SponsorRepository : RepositoryBase<Sponsor>, ISponsorRepository
    {
        private ISortHelper<Sponsor> _sortHelper;

        public SponsorRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Sponsor> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Sponsor>> GetSponsorsAsync(SponsorParameters sponsorParameters)
        {
            var sponsors = Enumerable.Empty<Sponsor>().AsQueryable();

            ApplyFilters(ref sponsors, sponsorParameters);

            PerformSearch(ref sponsors, sponsorParameters.SearchTerm);

            var sortedSponsors = _sortHelper.ApplySort(sponsors, sponsorParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Sponsor>.ToPagedList
                (
                    sortedSponsors,
                    sponsorParameters.PageNumber,
                    sponsorParameters.PageSize)
                );
        }

        public async Task<Sponsor> GetSponsorByIdAsync(Guid id)
        {
            return await FindByCondition(sponsor => sponsor.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SponsorExistAsync(Sponsor sponsor)
        {
            return await FindByCondition(x => x.Name == sponsor.Name)
                .AnyAsync();
        }

        public async Task CreateSponsorAsync(Sponsor sponsor)
        {
            await CreateAsync(sponsor);
        }

        public async Task UpdateSponsorAsync(Sponsor sponsor)
        {
            await UpdateAsync(sponsor);
        }

        public async Task UpdateSponsorAsync(IEnumerable<Sponsor> sponsors)
        {
            await UpdateAsync(sponsors);
        }

        public async Task DeleteSponsorAsync(Sponsor sponsor)
        {
            await DeleteAsync(sponsor);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Sponsor> sponsors, SponsorParameters sponsorParameters)
        {
            sponsors = FindAll();

            if (!string.IsNullOrWhiteSpace(sponsorParameters.AddBy))
            {
                sponsors = sponsors.Where(x => x.AppUserId == sponsorParameters.AddBy);
            }

        }

        private void PerformSearch(ref IQueryable<Sponsor> sponsors, string searchTerm)
        {
            if (!sponsors.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            sponsors = sponsors.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
