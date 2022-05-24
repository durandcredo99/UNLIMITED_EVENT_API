using Contracts;
using DinkToPdf;
using DinkToPdf.Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Contracts.IPlaceRepository;

namespace Repository
{
    public class PlaceRepository : RepositoryBase<Place>, IPlaceRepository
    {
        private ISortHelper<Place> _sortHelper;
        private readonly IWebHostEnvironment _webHostEnvironnement;

        public PlaceRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Place> sortHelper,
            IWebHostEnvironment webHostEnvironnement
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _webHostEnvironnement = webHostEnvironnement;
        }

        public async Task<PagedList<Place>> GetPlacesAsync(PlaceParameters placeParameters)
        {
            var places = Enumerable.Empty<Place>().AsQueryable();

            ApplyFilters(ref places, placeParameters);

            PerformSearch(ref places, placeParameters.SearchTerm);

            var sortedPlaces = _sortHelper.ApplySort(places, placeParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Place>.ToPagedList
                (
                    sortedPlaces,
                    placeParameters.PageNumber,
                    placeParameters.PageSize)
                );
        }

        public async Task<Place> GetPlaceByIdAsync(Guid id)
        {
            return await FindByCondition(place => place.Id.Equals(id))
                //.Include(x=>x.Order)
                .FirstOrDefaultAsync();
        }

        public async Task<Place> GetPlaceDetailsAsync(Guid id)
        {
            return await FindByCondition(place => place.Id.Equals(id))
                .Include(x=>x.Event).ThenInclude(x=>x.Category)
                .Include(x=>x.Order)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PlaceExistAsync(Place place)
        {
            return await FindByCondition(x => x.Event == place.Event)
                .AnyAsync();
        }

        public async Task CreatePlaceAsync(Place place)
        {
            await CreateAsync(place);
        }

        public async Task CreatePlaceAsync(IEnumerable<Place> places)
        {
            await CreateAsync(places);
        }

        public async Task UpdatePlaceAsync(Place place)
        {
            await UpdateAsync(place);
        }

        public async Task UpdatePlaceAsync(IEnumerable<Place> places)
        {
            await UpdateAsync(places);
        }

        public async Task DeletePlaceAsync(Place place)
        {
            await DeleteAsync(place);
        }

        public async Task DeletePlaceAsync(IEnumerable<Place> places)
        {
            await DeleteAsync(places);
        }


        public async Task<int> getNextNumber(Guid eventId)
        {
            int currentNum =0;

            if (FindByCondition(x=>x.EventId == eventId).Any()) currentNum = await FindByCondition(x => x.EventId == eventId).MaxAsync(x => x.NoPlace);
            currentNum++;

            return currentNum;
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Place> places, PlaceParameters placeParameters)
        {
            places = FindAll()
                .Include(x=>x.Event)
                .Include(x => x.Order);

            if (placeParameters.EventId != null && placeParameters.EventId != new Guid())
            {
                places = places.Where(x => x.EventId == placeParameters.EventId);
            }

            if (!string.IsNullOrWhiteSpace(placeParameters.BookededBy))
            {
                places = places.Where(x => x.Order.AppUserId == placeParameters.BookededBy);
            }
        }

        private void PerformSearch(ref IQueryable<Place> places, string searchTerm)
        {
            if (!places.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            places = places.Where(x => x.Event.ToString().Contains(searchTerm.Trim().ToLower()));
        }


        #endregion

    }
}
