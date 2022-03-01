
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEventYearRepository
    {
        Task<PagedList<EventYear>> GetEventYearsAsync(EventYearParameters rateParameters);

        Task<EventYear> GetEventYearByIdAsync(Guid id);
        Task<bool> EventYearExistAsync(EventYear rate);

        Task CreateEventYearAsync(EventYear rate);
        Task UpdateEventYearAsync(EventYear rate);
        Task UpdateEventYearAsync(IEnumerable<EventYear> rates);
        Task DeleteEventYearAsync(EventYear rate);
    }
}
