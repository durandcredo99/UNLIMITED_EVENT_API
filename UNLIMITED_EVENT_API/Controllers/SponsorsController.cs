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
    public class SponsorsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public SponsorsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponse>>> GetSponsors([FromQuery] SponsorParameters sponsorParameters)
        {
            var sponsors = await _repository.Sponsor.GetSponsorsAsync(sponsorParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(sponsors.MetaData));

            _logger.LogInfo($"Returned all sponsors from database.");

            var sponsorsResponses = _mapper.Map<IEnumerable<SponsorResponse>>(sponsors);

            return Ok(sponsorsResponses);
        }



        [HttpGet("{id}", Name = "SponsorById")]
        public async Task<ActionResult<SponsorResponse>> GetSponsorById(Guid id)
        {
            var sponsor = await _repository.Sponsor.GetSponsorByIdAsync(id);

            if (sponsor == null)
            {
                _logger.LogError($"Sponsor with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned sponsorRequest with id: {id}");

                var sponsorResponses = _mapper.Map<SponsorResponse>(sponsor);
                
                return Ok(sponsorResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<SponsorResponse>> CreateSponsor([FromBody] SponsorRequest sponsor)
        {
            if (sponsor == null)
            {
                _logger.LogError("Sponsor object sent from sponsor is null.");
                return BadRequest("Sponsor object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid sponsorRequest object sent from sponsor.");
                return BadRequest("Invalid model object");
            }

            var sponsorEntity = _mapper.Map<Sponsor>(sponsor);

            if (await _repository.Sponsor.SponsorExistAsync(sponsorEntity))
            {
                ModelState.AddModelError("", "This Sponsor exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Sponsor.CreateSponsorAsync(sponsorEntity);
            await _repository.SaveAsync();

            var sponsorResponses = _mapper.Map<SponsorResponse>(sponsorEntity);
            return CreatedAtRoute("SponsorById", new { id = sponsorResponses.Id }, sponsorResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<SponsorResponse>> UpdateSponsor(Guid id, [FromBody] SponsorRequest sponsorRequest)
        {
            if (sponsorRequest == null)
            {
                _logger.LogError("Sponsor object sent from sponsor is null.");
                return BadRequest("Sponsor object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid sponsorRequest object sent from sponsor.");
                return BadRequest("Invalid model object");
            }

            var sponsorEntity = await _repository.Sponsor.GetSponsorByIdAsync(id);
            if (sponsorEntity == null)
            {
                _logger.LogError($"Sponsor with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(sponsorRequest, sponsorEntity);


            await _repository.Sponsor.UpdateSponsorAsync(sponsorEntity);
            await _repository.SaveAsync();

            var sponsorResponses = _mapper.Map<SponsorResponse>(sponsorEntity);
            return Ok(sponsorResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialSponsorUpdate(Guid Id, JsonPatchDocument<SponsorRequest> patchDoc)
        {
            var sponsorModelFromRepository = await _repository.Sponsor.GetSponsorByIdAsync(Id);
            if (sponsorModelFromRepository == null) return NotFound();

            var sponsorToPatch = _mapper.Map<SponsorRequest>(sponsorModelFromRepository);
            patchDoc.ApplyTo(sponsorToPatch, ModelState);

            if (!TryValidateModel(sponsorToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(sponsorToPatch, sponsorModelFromRepository);

            await _repository.Sponsor.UpdateSponsorAsync(sponsorModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSponsor(Guid id)
        {
            var sponsor = await _repository.Sponsor.GetSponsorByIdAsync(id);

            if (sponsor == null)
            {
                _logger.LogError($"Sponsor with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Sponsor.DeleteSponsorAsync(sponsor);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
