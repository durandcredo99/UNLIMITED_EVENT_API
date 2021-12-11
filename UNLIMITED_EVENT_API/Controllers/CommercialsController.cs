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
    public class CommercialsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CommercialsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommercialResponse>>> GetCommercials([FromQuery] CommercialParameters commercialParameters)
        {
            var commercials = await _repository.Commercial.GetCommercialsAsync(commercialParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(commercials.MetaData));

            _logger.LogInfo($"Returned all commercials from database.");

            var commercialsResponses = _mapper.Map<IEnumerable<CommercialResponse>>(commercials);

            return Ok(commercialsResponses);
        }



        [HttpGet("{id}", Name = "CommercialById")]
        public async Task<ActionResult<CommercialResponse>> GetCommercialById(Guid id)
        {
            var commercial = await _repository.Commercial.GetCommercialByIdAsync(id);

            if (commercial == null)
            {
                _logger.LogError($"Commercial with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned commercialRequest with id: {id}");

                var commercialResponses = _mapper.Map<CommercialResponse>(commercial);
                
                return Ok(commercialResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<CommercialResponse>> CreateCommercial([FromBody] CommercialRequest commercial)
        {
            if (commercial == null)
            {
                _logger.LogError("Commercial object sent from commercial is null.");
                return BadRequest("Commercial object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid commercialRequest object sent from commercial.");
                return BadRequest("Invalid model object");
            }

            var commercialEntity = _mapper.Map<Commercial>(commercial);

            if (await _repository.Commercial.CommercialExistAsync(commercialEntity))
            {
                ModelState.AddModelError("", "This Commercial exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Commercial.CreateCommercialAsync(commercialEntity);
            await _repository.SaveAsync();

            var commercialResponses = _mapper.Map<CommercialResponse>(commercialEntity);
            return CreatedAtRoute("CommercialById", new { id = commercialResponses.Id }, commercialResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<CommercialResponse>> UpdateCommercial(Guid id, [FromBody] CommercialRequest commercialRequest)
        {
            if (commercialRequest == null)
            {
                _logger.LogError("Commercial object sent from commercial is null.");
                return BadRequest("Commercial object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid commercialRequest object sent from commercial.");
                return BadRequest("Invalid model object");
            }

            var commercialEntity = await _repository.Commercial.GetCommercialByIdAsync(id);
            if (commercialEntity == null)
            {
                _logger.LogError($"Commercial with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(commercialRequest, commercialEntity);


            await _repository.Commercial.UpdateCommercialAsync(commercialEntity);
            await _repository.SaveAsync();

            var commercialResponses = _mapper.Map<CommercialResponse>(commercialEntity);
            return Ok(commercialResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialCommercialUpdate(Guid Id, JsonPatchDocument<CommercialRequest> patchDoc)
        {
            var commercialModelFromRepository = await _repository.Commercial.GetCommercialByIdAsync(Id);
            if (commercialModelFromRepository == null) return NotFound();

            var commercialToPatch = _mapper.Map<CommercialRequest>(commercialModelFromRepository);
            patchDoc.ApplyTo(commercialToPatch, ModelState);

            if (!TryValidateModel(commercialToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commercialToPatch, commercialModelFromRepository);

            await _repository.Commercial.UpdateCommercialAsync(commercialModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommercial(Guid id)
        {
            var commercial = await _repository.Commercial.GetCommercialByIdAsync(id);

            if (commercial == null)
            {
                _logger.LogError($"Commercial with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Commercial.DeleteCommercialAsync(commercial);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
