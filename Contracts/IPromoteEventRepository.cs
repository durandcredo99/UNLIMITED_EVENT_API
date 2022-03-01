
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPromoteEventRepository
    {
        Task<PagedList<PromoteEvent>> GetPromoteEventsAsync(PromoteEventParameters promoteEventParameters);

        Task<PromoteEvent> GetPromoteEventByIdAsync(Guid id);
        Task<bool> PromoteEventExistAsync(PromoteEvent promoteEvent);

        Task<PromoteEvent> GetPromoteEventByPromoteIdAsync(string id);

        Task CreatePromoteEventAsync(PromoteEvent promoteEvent);
        Task UpdatePromoteEventAsync(PromoteEvent promoteEvent);
        Task UpdatePromoteEventAsync(IEnumerable<PromoteEvent> promoteEvents);
        Task DeletePromoteEventAsync(PromoteEvent promoteEvent);
    }
}
