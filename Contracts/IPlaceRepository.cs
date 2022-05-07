
using DinkToPdf;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Contracts.IDinkToPdfRepository;

namespace Contracts
{
    public interface IPlaceRepository
    {
        Task<PagedList<Place>> GetPlacesAsync(PlaceParameters placeParameters);

        Task<Place> GetPlaceByIdAsync(Guid id);
        Task<Place> GetPlaceDetailsAsync(Guid id);
        Task<bool> PlaceExistAsync(Place place);

        Task<int> getNextNumber(Guid eventId);

        Task CreatePlaceAsync(Place place);
        Task CreatePlaceAsync(IEnumerable<Place> places);
        Task UpdatePlaceAsync(Place place);
        Task DeletePlaceAsync(Place place);
        Task UpdatePlaceAsync(IEnumerable<Place> places);
        Task DeletePlaceAsync(IEnumerable<Place> places);
    }
}
