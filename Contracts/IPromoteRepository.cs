
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPromoteRepository
    {
        Task<PagedList<Promote>> GetPromotesAsync(PromoteParameters promoteParameters);

        Task<Promote> GetPromoteByIdAsync(Guid id);
        Task<bool> PromoteExistAsync(Promote promote);
        Task CreatePromoteAsync(Promote promote);
        Task UpdatePromoteAsync(Promote promote);
        Task UpdatePromoteAsync(IEnumerable<Promote> promotes);
        Task DeletePromoteAsync(Promote promote);
    }
}
