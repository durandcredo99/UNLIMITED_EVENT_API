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

        public async Task<PagedList<Event>> GetEventsAsync(EventQueryParameters eventParameters)
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
                .Include(x => x.Places)
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
        public async Task<int> CountEventsAsync()
        {
            return await FindAll().CountAsync();
        }

        public async Task DeleteEventAsync(Event _event)
        {
            await DeleteAsync(_event);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Event> events, EventQueryParameters eventQueryParameters)
        {
            events = FindAll()
                .Include(x => x.Places).ThenInclude(x => x.Order)
                .Include(x => x.Category)
                .Include(x => x.Sponsor)
                .Include(x => x.AppUser);

            if (eventQueryParameters.FromDate != null)
            {
                events = events.Where(x => x.Date >= eventQueryParameters.FromDate);
            }

            if (eventQueryParameters.ToDate != null)
            {
                events = events.Where(x => x.Date <= eventQueryParameters.ToDate);
            }

            if (eventQueryParameters.OfCategoryId != null && eventQueryParameters.OfCategoryId != new Guid())
            {
                events = events.Where(x => x.CategoryId == eventQueryParameters.OfCategoryId);
            }

            if (!string.IsNullOrWhiteSpace(eventQueryParameters.OrganizedBy))
            {
                events = events.Where(x => x.AppUserId == eventQueryParameters.OrganizedBy);
            }

            //if (eventQueryParameters.PublicOnly)
            //{
            //    events = events.Where(x => x.IsPublic == "Public");
            //}
        }

        private void PerformSearch(ref IQueryable<Event> events, string searchTerm)
        {
            if (!events.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            events = events.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
