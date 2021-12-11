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
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        private ISortHelper<Event> _sortHelper;

        public EventRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Event> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Event>> GetEventsAsync(EventParameters eventParameters)
        {
            var events = Enumerable.Empty<Event>().AsQueryable();

            ApplyFilters(ref events, eventParameters);

            PerformSearch(ref events, eventParameters.SearchTerm);

            var sortedEvents = _sortHelper.ApplySort(events, eventParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Event>.ToPagedList
                (
                    sortedEvents,
                    eventParameters.PageNumber,
                    eventParameters.PageSize)
                );
        }

        public async Task<Event> GetEventByIdAsync(Guid id)
        {
            return await FindByCondition(_event => _event.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EventExistAsync(Event _event)
        {
            return await FindByCondition(x => x.Name == _event.Name)
                .AnyAsync();
        }

        public async Task CreateEventAsync(Event _event)
        {
            await CreateAsync(_event);
        }

        public async Task UpdateEventAsync(Event _event)
        {
            await UpdateAsync(_event);
        }

        public async Task UpdateEventAsync(IEnumerable<Event> events)
        {
            await UpdateAsync(events);
        }

        public async Task DeleteEventAsync(Event _event)
        {
            await DeleteAsync(_event);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Event> events, EventParameters eventParameters)
        {
            events = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(eventParameters.AppUserId))
            {
                events = events.Where(x => x.AppUserId == eventParameters.AppUserId);
            }

            if (eventParameters.MinBirthday != null)
            {
                events = events.Where(x => x.Birthday >= eventParameters.MinBirthday);
            }

            if (eventParameters.MaxBirthday != null)
            {
                events = events.Where(x => x.Birthday < eventParameters.MaxBirthday);
            }

            if (eventParameters.MinCreateAt != null)
            {
                events = events.Where(x => x.CreateAt >= eventParameters.MinCreateAt);
            }

            if (eventParameters.MaxCreateAt != null)
            {
                events = events.Where(x => x.CreateAt < eventParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Event> events, string searchTerm)
        {
            if (!events.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            events = events.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
