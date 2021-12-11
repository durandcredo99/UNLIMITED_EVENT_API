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
    public class PaymentTypesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PaymentTypesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentTypeResponse>>> GetPaymentTypes([FromQuery] PaymentTypeParameters paymentTypeParameters)
        {
            var paymentTypes = await _repository.PaymentType.GetPaymentTypesAsync(paymentTypeParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paymentTypes.MetaData));

            _logger.LogInfo($"Returned all paymentTypes from database.");

            var paymentTypesResponses = _mapper.Map<IEnumerable<PaymentTypeResponse>>(paymentTypes);

            return Ok(paymentTypesResponses);
        }



        [HttpGet("{id}", Name = "PaymentTypeById")]
        public async Task<ActionResult<PaymentTypeResponse>> GetPaymentTypeById(Guid id)
        {
            var _paymentType = await _repository.PaymentType.GetPaymentTypeByIdAsync(id);

            if (_paymentType == null)
            {
                _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned paymentTypeRequest with id: {id}");

                var paymentTypeResponses = _mapper.Map<PaymentTypeResponse>(_paymentType);
                
                return Ok(paymentTypeResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PaymentTypeResponse>> CreatePaymentType([FromBody] PaymentTypeRequest _paymentType)
        {
            if (_paymentType == null)
            {
                _logger.LogError("PaymentType object sent from paymentType is null.");
                return BadRequest("PaymentType object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid paymentTypeRequest object sent from paymentType.");
                return BadRequest("Invalid model object");
            }

            var paymentTypeEntity = _mapper.Map<PaymentType>(_paymentType);

            if (await _repository.PaymentType.PaymentTypeExistAsync(paymentTypeEntity))
            {
                ModelState.AddModelError("", "This PaymentType exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.PaymentType.CreatePaymentTypeAsync(paymentTypeEntity);
            await _repository.SaveAsync();

            var paymentTypeResponses = _mapper.Map<PaymentTypeResponse>(paymentTypeEntity);
            return CreatedAtRoute("PaymentTypeById", new { id = paymentTypeResponses.Id }, paymentTypeResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentTypeResponse>> UpdatePaymentType(Guid id, [FromBody] PaymentTypeRequest paymentTypeRequest)
        {
            if (paymentTypeRequest == null)
            {
                _logger.LogError("PaymentType object sent from paymentType is null.");
                return BadRequest("PaymentType object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid paymentTypeRequest object sent from paymentType.");
                return BadRequest("Invalid model object");
            }

            var paymentTypeEntity = await _repository.PaymentType.GetPaymentTypeByIdAsync(id);
            if (paymentTypeEntity == null)
            {
                _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(paymentTypeRequest, paymentTypeEntity);


            await _repository.PaymentType.UpdatePaymentTypeAsync(paymentTypeEntity);
            await _repository.SaveAsync();

            var paymentTypeResponses = _mapper.Map<PaymentTypeResponse>(paymentTypeEntity);
            return Ok(paymentTypeResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialPaymentTypeUpdate(Guid Id, JsonPatchDocument<PaymentTypeRequest> patchDoc)
        {
            var paymentTypeModelFromRepository = await _repository.PaymentType.GetPaymentTypeByIdAsync(Id);
            if (paymentTypeModelFromRepository == null) return NotFound();

            var paymentTypeToPatch = _mapper.Map<PaymentTypeRequest>(paymentTypeModelFromRepository);
            patchDoc.ApplyTo(paymentTypeToPatch, ModelState);

            if (!TryValidateModel(paymentTypeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(paymentTypeToPatch, paymentTypeModelFromRepository);

            await _repository.PaymentType.UpdatePaymentTypeAsync(paymentTypeModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaymentType(Guid id)
        {
            var _paymentType = await _repository.PaymentType.GetPaymentTypeByIdAsync(id);

            if (_paymentType == null)
            {
                _logger.LogError($"PaymentType with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.PaymentType.DeletePaymentTypeAsync(_paymentType);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
