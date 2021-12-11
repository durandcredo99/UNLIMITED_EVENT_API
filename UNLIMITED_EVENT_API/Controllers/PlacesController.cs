using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UNLIMITED_EVENT_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlacesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PlacesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceResponse>>> GetPlaces([FromQuery] PlaceParameters placeParameters)
        {
            var places = await _repository.Place.GetPlacesAsync(placeParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(places.MetaData));

            _logger.LogInfo($"Returned all places from database.");

            var placesResponses = _mapper.Map<IEnumerable<PlaceResponse>>(places);

            return Ok(placesResponses);
        }



        [HttpGet("{id}", Name = "PlaceById")]
        public async Task<ActionResult<PlaceResponse>> GetPlaceById(Guid id)
        {
            var place = await _repository.Place.GetPlaceByIdAsync(id);

            if (place == null)
            {
                _logger.LogError($"Place with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned placeRequest with id: {id}");

                var placeResponses = _mapper.Map<PlaceResponse>(place);
                
                return Ok(placeResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PlaceResponse>> CreatePlace([FromBody] PlaceRequest place)
        {
            if (place == null)
            {
                _logger.LogError("Place object sent from place is null.");
                return BadRequest("Place object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid placeRequest object sent from place.");
                return BadRequest("Invalid model object");
            }

            place.NoPlace = await _repository.Place.getNextNumber();

            var placeEntity = _mapper.Map<Place>(place);


            if (await _repository.Place.PlaceExistAsync(placeEntity))
            {
                ModelState.AddModelError("", "This Place exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Place.CreatePlaceAsync(placeEntity);
            await _repository.SaveAsync();

            var placeResponses = _mapper.Map<PlaceResponse>(placeEntity);
            return CreatedAtRoute("PlaceById", new { id = placeResponses.Id }, placeResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PlaceResponse>> UpdatePlace(Guid id, [FromBody] PlaceRequest placeRequest)
        {
            if (placeRequest == null)
            {
                _logger.LogError("Place object sent from place is null.");
                return BadRequest("Place object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid placeRequest object sent from place.");
                return BadRequest("Invalid model object");
            }

            var placeEntity = await _repository.Place.GetPlaceByIdAsync(id);
            if (placeEntity == null)
            {
                _logger.LogError($"Place with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(placeRequest, placeEntity);


            await _repository.Place.UpdatePlaceAsync(placeEntity);
            await _repository.SaveAsync();

            var placeResponses = _mapper.Map<PlaceResponse>(placeEntity);
            return Ok(placeResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialPlaceUpdate(Guid Id, JsonPatchDocument<PlaceRequest> patchDoc)
        {
            var placeModelFromRepository = await _repository.Place.GetPlaceByIdAsync(Id);
            if (placeModelFromRepository == null) return NotFound();

            var placeToPatch = _mapper.Map<PlaceRequest>(placeModelFromRepository);
            patchDoc.ApplyTo(placeToPatch, ModelState);

            if (!TryValidateModel(placeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(placeToPatch, placeModelFromRepository);

            await _repository.Place.UpdatePlaceAsync(placeModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlace(Guid id)
        {
            var place = await _repository.Place.GetPlaceByIdAsync(id);

            if (place == null)
            {
                _logger.LogError($"Place with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Place.DeletePlaceAsync(place);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
