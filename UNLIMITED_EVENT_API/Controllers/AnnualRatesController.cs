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
    public class AnnualRatesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public AnnualRatesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/AnnualRate";
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnnualRateResponse>>> GetAnnualRates([FromQuery] AnnualRateParameters annualRateParameters)
        {
            var annualRates = await _repository.AnnualRate.GetAnnualRatesAsync(annualRateParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(annualRates.MetaData));

            _logger.LogInfo($"Returned all annualRates from database.");

            var annualRatesResponses = _mapper.Map<IEnumerable<AnnualRateResponse>>(annualRates);

            return Ok(annualRatesResponses);
        }



        [HttpGet("{id}", Name = "AnnualRateById")]
        public async Task<ActionResult<AnnualRateResponse>> GetAnnualRateById(Guid id)
        {
            var annualRate = await _repository.AnnualRate.GetAnnualRateByIdAsync(id);

            if (annualRate == null)
            {
                _logger.LogError($"AnnualRate with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned annualRateRequest with id: {id}");

                var annualRateResponses = _mapper.Map<AnnualRateResponse>(annualRate);

                return Ok(annualRateResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<AnnualRateResponse>> CreateAnnualRate([FromBody] AnnualRateRequest annualRate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (annualRate == null)
            {
                _logger.LogError("AnnualRate object sent from annualRate is null.");
                return BadRequest("AnnualRate object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid annualRateRequest object sent from annualRate.");
                return BadRequest("Invalid model object");
            }

            var annualRateEntity = _mapper.Map<AnnualRate>(annualRate);

            if (await _repository.AnnualRate.AnnualRateExistAsync(annualRateEntity))
            {
                ModelState.AddModelError("", "This AnnualRate exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.AnnualRate.CreateAnnualRateAsync(annualRateEntity);
            await _repository.SaveAsync();

            var annualRateResponses = _mapper.Map<AnnualRateResponse>(annualRateEntity);
            return CreatedAtRoute("AnnualRateById", new { id = annualRateResponses.Id }, annualRateResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<AnnualRateResponse>> UpdateAnnualRate(Guid id, [FromBody] AnnualRateRequest annualRateRequest)
        {
            if (annualRateRequest == null)
            {
                _logger.LogError("AnnualRate object sent from annualRate is null.");
                return BadRequest("AnnualRate object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid annualRateRequest object sent from annualRate.");
                return BadRequest("Invalid model object");
            }

            var annualRateEntity = await _repository.AnnualRate.GetAnnualRateByIdAsync(id);
            if (annualRateEntity == null)
            {
                _logger.LogError($"AnnualRate with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(annualRateRequest, annualRateEntity);


            await _repository.AnnualRate.UpdateAnnualRateAsync(annualRateEntity);
            await _repository.SaveAsync();

            var annualRateResponses = _mapper.Map<AnnualRateResponse>(annualRateEntity);

            return Ok(annualRateResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialAnnualRateUpdate(Guid Id, JsonPatchDocument<AnnualRateRequest> patchDoc)
        {
            var annualRateModelFromRepository = await _repository.AnnualRate.GetAnnualRateByIdAsync(Id);
            if (annualRateModelFromRepository == null) return NotFound();

            var annualRateToPatch = _mapper.Map<AnnualRateRequest>(annualRateModelFromRepository);
            patchDoc.ApplyTo(annualRateToPatch, ModelState);

            if (!TryValidateModel(annualRateToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(annualRateToPatch, annualRateModelFromRepository);

            await _repository.AnnualRate.UpdateAnnualRateAsync(annualRateModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnnualRate(Guid id)
        {
            var annualRate = await _repository.AnnualRate.GetAnnualRateByIdAsync(id);

            if (annualRate == null)
            {
                _logger.LogError($"AnnualRate with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.AnnualRate.DeleteAnnualRateAsync(annualRate);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
