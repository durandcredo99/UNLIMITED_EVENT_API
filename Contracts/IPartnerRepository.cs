
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPartnerRepository
    {
        Task<PagedList<Partner>> GetPartnersAsync(PartnerParameters partnerParameters);
        Task<Partner> GetPartnerByIdAsync(Guid id);
        Task<bool> PartnerExistAsync(Partner partner);

        Task CreatePartnerAsync(Partner partner);
        Task UpdatePartnerAsync(Partner partner);
        Task UpdatePartnerAsync(IEnumerable<Partner> partners);
        Task DeletePartnerAsync(Partner partner);
    }
}
