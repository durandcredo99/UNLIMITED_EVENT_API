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
    public class PromoteEventRepository : RepositoryBase<PromoteEvent>, IPromoteEventRepository
    {
        private ISortHelper<PromoteEvent> _sortHelper;

        public PromoteEventRepository(
            RepositoryContext repositoryContext,
            ISortHelper<PromoteEvent> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<PromoteEvent>> GetPromoteEventsAsync(PromoteEventParameters promoteEventParameters)
        {
            var promoteEvent = Enumerable.Empty<PromoteEvent>().AsQueryable();

            ApplyFilters(ref promoteEvent, promoteEventParameters);

            PerformSearch(ref promoteEvent, promoteEventParameters.SearchTerm);

            var sortedPromoteEvents = _sortHelper.ApplySort(promoteEvent, promoteEventParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<PromoteEvent>.ToPagedList
                (
                    sortedPromoteEvents,
                    promoteEventParameters.PageNumber,
                    promoteEventParameters.PageSize)
                );
        }

        public async Task<PromoteEvent> GetPromoteEventByIdAsync(Guid id)
        {
            return await FindByCondition(promoteEvent => promoteEvent.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PromoteEventExistAsync(PromoteEvent promoteEvent)
        {
            return await FindByCondition(x => x.StartingDate == promoteEvent.StartingDate)
                .AnyAsync();
        }

        public async Task<PromoteEvent> GetPromoteEventByPromoteIdAsync(string id)
        {
            return await FindByCondition(promoteEvent => promoteEvent.Id.Equals(id))
                .Include(x => x.Promote).Include(x => x.Event).ThenInclude(x=>x.AppUser)
                .OrderByDescending(x => x.Promote.Position)
                .FirstOrDefaultAsync();
        }


        public async Task CreatePromoteEventAsync(PromoteEvent promoteEvent)
        {
            await CreateAsync(promoteEvent);
        }

        public async Task UpdatePromoteEventAsync(PromoteEvent promoteEvent)
        {
            await UpdateAsync(promoteEvent);
        }

        public async Task UpdatePromoteEventAsync(IEnumerable<PromoteEvent> promoteEvent)
        {
            await UpdateAsync(promoteEvent);
        }

        public async Task DeletePromoteEventAsync(PromoteEvent promoteEvent)
        {
            await DeleteAsync(promoteEvent);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<PromoteEvent> promoteEvent, PromoteEventParameters promoteEventParameters)
        {
            promoteEvent = FindAll()
                 .Include(x => x.Event)
                 .Include(x => x.Promote);

            //if (!string.IsNullOrWhiteSpace(promoteEventParameters.AddFor))
            //{
            //    promoteEvent = promoteEvent.Where(x => x.AppUserId == promoteEventParameters.AddFor);
            //}
            /*
            if (!string.IsNullOrWhiteSpace(promoteEventParameters.AppUserId))
            {
                promoteEvent = promoteEvent.Where(x => x.AppUserId == promoteEventParameters.AppUserId);
            }

            if (promoteEventParameters.MinBirthday != null)
            {
                promoteEvent = promoteEvent.Where(x => x.Birthday >= promoteEventParameters.MinBirthday);
            }

            if (promoteEventParameters.MaxBirthday != null)
            {
                promoteEvent = promoteEvent.Where(x => x.Birthday < promoteEventParameters.MaxBirthday);
            }

            if (promoteEventParameters.MinCreateAt != null)
            {
                promoteEvent = promoteEvent.Where(x => x.CreateAt >= promoteEventParameters.MinCreateAt);
            }

            if (promoteEventParameters.MaxCreateAt != null)
            {
                promoteEvent = promoteEvent.Where(x => x.CreateAt < promoteEventParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<PromoteEvent> promoteEvent, string searchTerm)
        {
            if (!promoteEvent.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            //promoteEvent = promoteEvent.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
