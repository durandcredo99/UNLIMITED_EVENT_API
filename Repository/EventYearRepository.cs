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
    public class EventYearRepository : RepositoryBase<EventYear>, IEventYearRepository
    {
        private ISortHelper<EventYear> _sortHelper;

        public EventYearRepository(
            RepositoryContext repositoryContext,
            ISortHelper<EventYear> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<EventYear>> GetEventYearsAsync(EventYearParameters eventYearParameters)
        {
            var eventYear = Enumerable.Empty<EventYear>().AsQueryable();

            ApplyFilters(ref eventYear, eventYearParameters);

            PerformSearch(ref eventYear, eventYearParameters.SearchTerm);

            var sortedEventYears = _sortHelper.ApplySort(eventYear, eventYearParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<EventYear>.ToPagedList
                (
                    sortedEventYears,
                    eventYearParameters.PageNumber,
                    eventYearParameters.PageSize)
                );
        }

        public async Task<EventYear> GetEventYearByIdAsync(Guid id)
        {
            return await FindByCondition(eventYear => eventYear.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EventYearExistAsync(EventYear eventYear)
        {
            return await FindByCondition(x => x.StartingDate == eventYear.StartingDate)
                .AnyAsync();
        }

        public async Task CreateEventYearAsync(EventYear eventYear)
        {
            await CreateAsync(eventYear);
        }

        public async Task UpdateEventYearAsync(EventYear eventYear)
        {
            await UpdateAsync(eventYear);
        }

        public async Task UpdateEventYearAsync(IEnumerable<EventYear> eventYear)
        {
            await UpdateAsync(eventYear);
        }

        public async Task DeleteEventYearAsync(EventYear eventYear)
        {
            await DeleteAsync(eventYear);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<EventYear> eventYear, EventYearParameters eventYearParameters)
        {
            eventYear = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(eventYearParameters.AppUserId))
            {
                eventYear = eventYear.Where(x => x.AppUserId == eventYearParameters.AppUserId);
            }

            if (eventYearParameters.MinBirthday != null)
            {
                eventYear = eventYear.Where(x => x.Birthday >= eventYearParameters.MinBirthday);
            }

            if (eventYearParameters.MaxBirthday != null)
            {
                eventYear = eventYear.Where(x => x.Birthday < eventYearParameters.MaxBirthday);
            }

            if (eventYearParameters.MinCreateAt != null)
            {
                eventYear = eventYear.Where(x => x.CreateAt >= eventYearParameters.MinCreateAt);
            }

            if (eventYearParameters.MaxCreateAt != null)
            {
                eventYear = eventYear.Where(x => x.CreateAt < eventYearParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<EventYear> eventYear, string searchTerm)
        {
            if (!eventYear.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            //eventYear = eventYear.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
