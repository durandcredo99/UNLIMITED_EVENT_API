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
using System.Security.Claims;
using System.Threading.Tasks;

namespace UNLIMITED_EVENT_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class EventsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public EventsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/Event";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponse>>> GetEvents([FromQuery] EventQueryParameters eventParameters)
        {
           
            var events = await _repository.Event.GetEventsAsync(eventParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(events.MetaData));

            _logger.LogInfo($"Returned all events from database.");

            var eventsResponses = _mapper.Map<IEnumerable<EventResponse>>(events);

            eventsResponses.ToList().ForEach(eventResponse =>
            {
                if (!string.IsNullOrWhiteSpace(eventResponse.ImgLink)) eventResponse.ImgLink = $"{_baseURL}{eventResponse.ImgLink}";
            });

            return Ok(eventsResponses);
        }



        [HttpGet("{id}", Name = "EventById")]
        public async Task<ActionResult<EventResponse>> GetEventById(Guid id)
        {
            var _event = await _repository.Event.GetEventByIdAsync(id);

            if (_event == null)
            {
                _logger.LogError($"Event with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned eventRequest with id: {id}");

                var eventResponses = _mapper.Map<EventResponse>(_event);
                if (!string.IsNullOrWhiteSpace(eventResponses.ImgLink)) eventResponses.ImgLink = $"{_baseURL}{eventResponses.ImgLink}";

                return Ok(eventResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<EventResponse>> CreateEvent([FromBody] EventRequest _event)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var userFirstName = User.FindFirst(ClaimTypes.GivenName).Value;
            //var userName = User.FindFirst(ClaimTypes.Surname).Value;

            if (_event == null)
            {
                _logger.LogError("Event object sent from event is null.");
                return BadRequest("Event object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid eventRequest object sent from event.");
                return BadRequest("Invalid model object");
            }

            var eventEntity = _mapper.Map<Event>(_event);

            eventEntity.AppUserId = userId;


            if (await _repository.Event.EventExistAsync(eventEntity))
            {
                ModelState.AddModelError("", "This Event exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Event.CreateEventAsync(eventEntity);
            await _repository.SaveAsync();

            var eventResponses = _mapper.Map<EventResponse>(eventEntity);
            return CreatedAtRoute("EventById", new { id = eventResponses.Id }, eventResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<EventResponse>> UpdateEvent(Guid id, [FromBody] EventRequest eventRequest)
        {
            if (eventRequest == null)
            {
                _logger.LogError("Event object sent from event is null.");
                return BadRequest("Event object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid eventRequest object sent from event.");
                return BadRequest("Invalid model object");
            }

            var eventEntity = await _repository.Event.GetEventByIdAsync(id);
            if (eventEntity == null)
            {
                _logger.LogError($"Event with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(eventRequest, eventEntity);


            await _repository.Event.UpdateEventAsync(eventEntity);
            await _repository.SaveAsync();

            var eventResponses = _mapper.Map<EventResponse>(eventEntity);
            if (!string.IsNullOrWhiteSpace(eventResponses.ImgLink)) eventResponses.ImgLink = $"{_baseURL}{eventResponses.ImgLink}";

            return Ok(eventResponses);
        }



        [HttpPut("{id}/upload-picture")]
        public async Task<ActionResult<EventResponse>> UploadPicture(Guid id, [FromForm] IFormFile file)
        {
            var eventEntity = await _repository.Event.GetEventByIdAsync(id);
            if (eventEntity == null) return NotFound();

            if (file != null)
            {
                _repository.File.FilePath = id.ToString();

                var uploadResult = await _repository.File.UploadFile(file);

                if (uploadResult == null)
                {
                    ModelState.AddModelError("", "something went wrong when uploading the picture");
                    return ValidationProblem(ModelState);
                }
                else
                {
                    eventEntity.ImgLink = uploadResult;
                }
            }

            await _repository.Event.UpdateEventAsync(eventEntity);

            await _repository.SaveAsync();

            var eventResponse = _mapper.Map<EventResponse>(eventEntity);

            if (!string.IsNullOrWhiteSpace(eventResponse.ImgLink)) eventResponse.ImgLink = $"{_baseURL}{eventResponse.ImgLink}";

            return Ok(eventResponse);
        }



        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialEventUpdate(Guid Id, JsonPatchDocument<EventRequest> patchDoc)
        {
            var eventModelFromRepository = await _repository.Event.GetEventByIdAsync(Id);
            if (eventModelFromRepository == null) return NotFound();

            var eventToPatch = _mapper.Map<EventRequest>(eventModelFromRepository);
            patchDoc.ApplyTo(eventToPatch, ModelState);

            if (!TryValidateModel(eventToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(eventToPatch, eventModelFromRepository);

            await _repository.Event.UpdateEventAsync(eventModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            var _event = await _repository.Event.GetEventByIdAsync(id);

            if (_event == null)
            {
                _logger.LogError($"Event with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Event.DeleteEventAsync(_event);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
