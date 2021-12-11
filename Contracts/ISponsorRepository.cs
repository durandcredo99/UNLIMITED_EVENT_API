
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISponsorRepository
    {
        Task<PagedList<Sponsor>> GetSponsorsAsync(SponsorParameters sponsorParameters);

        Task<Sponsor> GetSponsorByIdAsync(Guid id);
        Task<bool> SponsorExistAsync(Sponsor sponsor);

        Task CreateSponsorAsync(Sponsor sponsor);
        Task UpdateSponsorAsync(Sponsor sponsor);
        Task UpdateSponsorAsync(IEnumerable<Sponsor> sponsors);
        Task DeleteSponsorAsync(Sponsor sponsor);
    }
}
