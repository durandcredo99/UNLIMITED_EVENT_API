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
    public class BannerRepository : RepositoryBase<Banner>, IBannerRepository
    {
        private ISortHelper<Banner> _sortHelper;

        public BannerRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<Banner> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Banner>> GetBannersAsync(BannerParameters bannerParameters)
        {
            var banners = Enumerable.Empty<Banner>().AsQueryable();

            ApplyFilters(ref banners, bannerParameters);

            var sortedBanners = _sortHelper.ApplySort(banners, bannerParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Banner>.ToPagedList
                (
                    sortedBanners,
                    bannerParameters.PageNumber,
                    bannerParameters.PageSize)
                );
        }

        public async Task<Banner> GetBannerByIdAsync(Guid Id)
        {
            return await FindByCondition(banner => banner.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> BannerExistAsync(Banner banner)
        {
            return await FindByCondition(x => x.No == banner.No)
                .AnyAsync();
        }

        public async Task<int> GetBannersCountAsync()
        {
            return await FindAll().CountAsync();
        }

        public async Task CreateBannerAsync(Banner banner)
        {
            await CreateAsync(banner);
        }

        public async Task UpdateBannerAsync(Banner banner)
        {
            await UpdateAsync(banner);
        }

        public async Task DeleteBannerAsync(Banner banner)
        {
            await DeleteAsync(banner);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Banner> banners, BannerParameters bannerParameters)
        {
            banners = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(bannerParameters.AppUserId))
            {
                banners = banners.Where(x => x.AppUserId == bannerParameters.AppUserId);
            }

            if (bannerParameters.MinBirthday != null)
            {
                banners = banners.Where(x => x.Birthday >= bannerParameters.MinBirthday);
            }

            if (bannerParameters.MaxBirthday != null)
            {
                banners = banners.Where(x => x.Birthday < bannerParameters.MaxBirthday);
            }

            if (bannerParameters.MinCreateAt != null)
            {
                banners = banners.Where(x => x.CreateAt >= bannerParameters.MinCreateAt);
            }

            if (bannerParameters.MaxCreateAt != null)
            {
                banners = banners.Where(x => x.CreateAt < bannerParameters.MaxCreateAt);
            }
            */
        }

        #endregion

    }
}
