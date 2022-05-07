
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEventRepository
    {
        Task<PagedList<Event>> GetEventsAsync(EventQueryParameters eventParameters);

        Task<Event> GetPlaceByIdAsync(Guid id);
        Task<Event> GetEventDetailsAsync(Guid id);
        Task<bool> EventExistAsync(Event _event);

        Task CreateEventAsync(Event _event);
        Task UpdateEventAsync(Event _event);
        //Task UpdateEventAsync(IEnumerable<Event> events);
        Task DeleteEventAsync(Event _event);
        Task<int> CountEventsAsync();

    }
}
