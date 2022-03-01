
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPlaceRepository
    {
        Task<PagedList<Place>> GetPlacesAsync(PlaceParameters placeParameters);

        Task<Place> GetPlaceByIdAsync(Guid id);
        Task<bool> PlaceExistAsync(Place place);

        Task<int> getNextNumber();

        Task CreatePlaceAsync(Place place);
        Task CreatePlaceAsync(IEnumerable<Place> places);
        Task UpdatePlaceAsync(Place place);
        Task UpdatePlaceAsync(IEnumerable<Place> places);
        Task DeletePlaceAsync(Place place);
    }
}
