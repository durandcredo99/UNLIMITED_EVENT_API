
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICommercialRepository
    {
        Task<PagedList<Commercial>> GetCommercialsAsync(CommercialParameters commercialParameters);

        Task<Commercial> GetCommercialByIdAsync(Guid id);
        Task<bool> CommercialExistAsync(Commercial commercial);

        Task CreateCommercialAsync(Commercial commercial);
        Task UpdateCommercialAsync(Commercial commercial);
        Task UpdateCommercialAsync(IEnumerable<Commercial> commercials);
        Task DeleteCommercialAsync(Commercial commercial);
    }
}
