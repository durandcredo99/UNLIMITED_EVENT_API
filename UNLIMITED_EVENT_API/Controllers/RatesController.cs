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
    public class RatesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public RatesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RateResponse>>> GetRates([FromQuery] RateParameters rateParameters)
        {
            var rates = await _repository.Rate.GetRatesAsync(rateParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(rates.MetaData));

            _logger.LogInfo($"Returned all rates from database.");

            var ratesResponses = _mapper.Map<IEnumerable<RateResponse>>(rates);

            return Ok(ratesResponses);
        }



        [HttpGet("{id}", Name = "RateById")]
        public async Task<ActionResult<RateResponse>> GetRateById(Guid id)
        {
            var rate = await _repository.Rate.GetRateByIdAsync(id);

            if (rate == null)
            {
                _logger.LogError($"Rate with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned rateRequest with id: {id}");

                var rateResponses = _mapper.Map<RateResponse>(rate);
                
                return Ok(rateResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<RateResponse>> CreateRate([FromBody] RateRequest rate)
        {
            if (rate == null)
            {
                _logger.LogError("Rate object sent from rate is null.");
                return BadRequest("Rate object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid rateRequest object sent from rate.");
                return BadRequest("Invalid model object");
            }

            var rateEntity = _mapper.Map<Rate>(rate);

            if (await _repository.Rate.RateExistAsync(rateEntity))
            {
                ModelState.AddModelError("", "This Rate exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Rate.CreateRateAsync(rateEntity);
            await _repository.SaveAsync();

            var rateResponses = _mapper.Map<RateResponse>(rateEntity);
            return CreatedAtRoute("RateById", new { id = rateResponses.Id }, rateResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<RateResponse>> UpdateRate(Guid id, [FromBody] RateRequest rateRequest)
        {
            if (rateRequest == null)
            {
                _logger.LogError("Rate object sent from rate is null.");
                return BadRequest("Rate object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid rateRequest object sent from rate.");
                return BadRequest("Invalid model object");
            }

            var rateEntity = await _repository.Rate.GetRateByIdAsync(id);
            if (rateEntity == null)
            {
                _logger.LogError($"Rate with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(rateRequest, rateEntity);


            await _repository.Rate.UpdateRateAsync(rateEntity);
            await _repository.SaveAsync();

            var rateResponses = _mapper.Map<RateResponse>(rateEntity);
            return Ok(rateResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialRateUpdate(Guid Id, JsonPatchDocument<RateRequest> patchDoc)
        {
            var rateModelFromRepository = await _repository.Rate.GetRateByIdAsync(Id);
            if (rateModelFromRepository == null) return NotFound();

            var rateToPatch = _mapper.Map<RateRequest>(rateModelFromRepository);
            patchDoc.ApplyTo(rateToPatch, ModelState);

            if (!TryValidateModel(rateToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(rateToPatch, rateModelFromRepository);

            await _repository.Rate.UpdateRateAsync(rateModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRate(Guid id)
        {
            var rate = await _repository.Rate.GetRateByIdAsync(id);

            if (rate == null)
            {
                _logger.LogError($"Rate with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Rate.DeleteRateAsync(rate);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
