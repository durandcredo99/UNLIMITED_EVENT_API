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
    public class PartnerRepository : RepositoryBase<Partner>, IPartnerRepository
    {
        private ISortHelper<Partner> _sortHelper;

        public PartnerRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Partner> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Partner>> GetPartnersAsync(PartnerParameters partnerParameters)
        {
            var partner = Enumerable.Empty<Partner>().AsQueryable();

            ApplyFilters(ref partner, partnerParameters);

            PerformSearch(ref partner, partnerParameters.SearchTerm);

            var sortedPartners = _sortHelper.ApplySort(partner, partnerParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Partner>.ToPagedList
                (
                    sortedPartners,
                    partnerParameters.PageNumber,
                    partnerParameters.PageSize)
                );
        }

        public async Task<Partner> GetPartnerByIdAsync(Guid id)
        {
            return await FindByCondition(partner => partner.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PartnerExistAsync(Partner partner)
        {
            return await FindByCondition(x => x.Name == partner.Name)
                .AnyAsync();
        }

        public async Task CreatePartnerAsync(Partner partner)
        {
            await CreateAsync(partner);
        }

        public async Task UpdatePartnerAsync(Partner partner)
        {
            await UpdateAsync(partner);
        }

        public async Task UpdatePartnerAsync(IEnumerable<Partner> partner)
        {
            await UpdateAsync(partner);
        }

        public async Task DeletePartnerAsync(Partner partner)
        {
            await DeleteAsync(partner);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Partner> partner, PartnerParameters partnerParameters)
        {
            partner = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(partnerParameters.AppUserId))
            {
                partner = partner.Where(x => x.AppUserId == partnerParameters.AppUserId);
            }

            if (partnerParameters.MinBirthday != null)
            {
                partner = partner.Where(x => x.Birthday >= partnerParameters.MinBirthday);
            }

            if (partnerParameters.MaxBirthday != null)
            {
                partner = partner.Where(x => x.Birthday < partnerParameters.MaxBirthday);
            }

            if (partnerParameters.MinCreateAt != null)
            {
                partner = partner.Where(x => x.CreateAt >= partnerParameters.MinCreateAt);
            }

            if (partnerParameters.MaxCreateAt != null)
            {
                partner = partner.Where(x => x.CreateAt < partnerParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Partner> partner, string searchTerm)
        {
            if (!partner.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            partner = partner.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
