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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PlaceRepository : RepositoryBase<Place>, IPlaceRepository
    {
        private ISortHelper<Place> _sortHelper;

        public PlaceRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Place> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
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


        public async Task<int> getNextNumber()
        {
            int currentNum =0;

            if (FindAll().Any()) currentNum = await FindAll().MaxAsync(x => x.NoPlace);
            currentNum++;

            return currentNum;
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Place> places, PlaceParameters placeParameters)
        {
            places = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(placeParameters.AppUserId))
            {
                places = places.Where(x => x.AppUserId == placeParameters.AppUserId);
            }

            if (placeParameters.MinBirthday != null)
            {
                places = places.Where(x => x.Birthday >= placeParameters.MinBirthday);
            }

            if (placeParameters.MaxBirthday != null)
            {
                places = places.Where(x => x.Birthday < placeParameters.MaxBirthday);
            }

            if (placeParameters.MinCreateAt != null)
            {
                places = places.Where(x => x.CreateAt >= placeParameters.MinCreateAt);
            }

            if (placeParameters.MaxCreateAt != null)
            {
                places = places.Where(x => x.CreateAt < placeParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Place> places, string searchTerm)
        {
            if (!places.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            places = places.Where(x => x.Event.ToString().Contains(searchTerm.Trim().ToLower()));
        }


        #endregion

    }
}
