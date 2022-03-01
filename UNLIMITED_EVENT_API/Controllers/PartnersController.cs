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
    public class PartnersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PartnersController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartnerResponse>>> GetPartners([FromQuery] PartnerParameters partnerParameters)
        {
            var partners = await _repository.Partner.GetPartnersAsync(partnerParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(partners.MetaData));

            _logger.LogInfo($"Returned all partners from database.");

            var partnersResponses = _mapper.Map<IEnumerable<PartnerResponse>>(partners);

            return Ok(partnersResponses);
        }



        [HttpGet("{id}", Name = "PartnerById")]
        public async Task<ActionResult<PartnerResponse>> GetPartnerById(Guid id)
        {
            var partner = await _repository.Partner.GetPartnerByIdAsync(id);

            if (partner == null)
            {
                _logger.LogError($"Partner with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned partnerRequest with id: {id}");

                var partnerResponses = _mapper.Map<PartnerResponse>(partner);
                
                return Ok(partnerResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PartnerResponse>> CreatePartner([FromBody] PartnerRequest partner)
        {
            if (partner == null)
            {
                _logger.LogError("Partner object sent from partner is null.");
                return BadRequest("Partner object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid partnerRequest object sent from partner.");
                return BadRequest("Invalid model object");
            }

            var partnerEntity = _mapper.Map<Partner>(partner);

            if (await _repository.Partner.PartnerExistAsync(partnerEntity))
            {
                ModelState.AddModelError("", "This Partner exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Partner.CreatePartnerAsync(partnerEntity);
            await _repository.SaveAsync();

            var partnerResponses = _mapper.Map<PartnerResponse>(partnerEntity);
            return CreatedAtRoute("PartnerById", new { id = partnerResponses.Id }, partnerResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PartnerResponse>> UpdatePartner(Guid id, [FromBody] PartnerRequest partnerRequest)
        {
            if (partnerRequest == null)
            {
                _logger.LogError("Partner object sent from partner is null.");
                return BadRequest("Partner object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid partnerRequest object sent from partner.");
                return BadRequest("Invalid model object");
            }

            var partnerEntity = await _repository.Partner.GetPartnerByIdAsync(id);
            if (partnerEntity == null)
            {
                _logger.LogError($"Partner with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(partnerRequest, partnerEntity);


            await _repository.Partner.UpdatePartnerAsync(partnerEntity);
            await _repository.SaveAsync();

            var partnerResponses = _mapper.Map<PartnerResponse>(partnerEntity);
            return Ok(partnerResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialPartnerUpdate(Guid Id, JsonPatchDocument<PartnerRequest> patchDoc)
        {
            var partnerModelFromRepository = await _repository.Partner.GetPartnerByIdAsync(Id);
            if (partnerModelFromRepository == null) return NotFound();

            var partnerToPatch = _mapper.Map<PartnerRequest>(partnerModelFromRepository);
            patchDoc.ApplyTo(partnerToPatch, ModelState);

            if (!TryValidateModel(partnerToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(partnerToPatch, partnerModelFromRepository);

            await _repository.Partner.UpdatePartnerAsync(partnerModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePartner(Guid id)
        {
            var partner = await _repository.Partner.GetPartnerByIdAsync(id);

            if (partner == null)
            {
                _logger.LogError($"Partner with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Partner.DeletePartnerAsync(partner);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
