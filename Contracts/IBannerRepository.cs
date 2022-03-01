
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IBannerRepository
    {
        Task<PagedList<Banner>> GetBannersAsync(BannerParameters bannerParameters);

        Task<Banner> GetBannerByIdAsync(Guid Id);
        Task<int> GetBannersCountAsync();
        Task<bool> BannerExistAsync(Banner banner);

        Task CreateBannerAsync(Banner banner);
        Task UpdateBannerAsync(Banner banner);
        Task DeleteBannerAsync(Banner banner);
    }
}
