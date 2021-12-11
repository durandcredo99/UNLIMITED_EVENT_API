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
    public class PaymentsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PaymentsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResponse>>> GetPayments([FromQuery] PaymentParameters paymentParameters)
        {
            var payments = await _repository.Payment.GetPaymentsAsync(paymentParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(payments.MetaData));

            _logger.LogInfo($"Returned all payments from database.");

            var paymentsResponses = _mapper.Map<IEnumerable<PaymentResponse>>(payments);

            return Ok(paymentsResponses);
        }



        [HttpGet("{id}", Name = "PaymentById")]
        public async Task<ActionResult<PaymentResponse>> GetPaymentById(Guid id)
        {
            var payment = await _repository.Payment.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned paymentRequest with id: {id}");

                var paymentResponses = _mapper.Map<PaymentResponse>(payment);
                
                return Ok(paymentResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> CreatePayment([FromBody] PaymentRequest payment)
        {
            if (payment == null)
            {
                _logger.LogError("Payment object sent from payment is null.");
                return BadRequest("Payment object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid paymentRequest object sent from payment.");
                return BadRequest("Invalid model object");
            }

            var paymentEntity = _mapper.Map<Payment>(payment);

            if (await _repository.Payment.PaymentExistAsync(paymentEntity))
            {
                ModelState.AddModelError("", "This Payment exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Payment.CreatePaymentAsync(paymentEntity);
            await _repository.SaveAsync();

            var paymentResponses = _mapper.Map<PaymentResponse>(paymentEntity);
            return CreatedAtRoute("PaymentById", new { id = paymentResponses.Id }, paymentResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentResponse>> UpdatePayment(Guid id, [FromBody] PaymentRequest paymentRequest)
        {
            if (paymentRequest == null)
            {
                _logger.LogError("Payment object sent from payment is null.");
                return BadRequest("Payment object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid paymentRequest object sent from payment.");
                return BadRequest("Invalid model object");
            }

            var paymentEntity = await _repository.Payment.GetPaymentByIdAsync(id);
            if (paymentEntity == null)
            {
                _logger.LogError($"Payment with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(paymentRequest, paymentEntity);


            await _repository.Payment.UpdatePaymentAsync(paymentEntity);
            await _repository.SaveAsync();

            var paymentResponses = _mapper.Map<PaymentResponse>(paymentEntity);
            return Ok(paymentResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialPaymentUpdate(Guid Id, JsonPatchDocument<PaymentRequest> patchDoc)
        {
            var paymentModelFromRepository = await _repository.Payment.GetPaymentByIdAsync(Id);
            if (paymentModelFromRepository == null) return NotFound();

            var paymentToPatch = _mapper.Map<PaymentRequest>(paymentModelFromRepository);
            patchDoc.ApplyTo(paymentToPatch, ModelState);

            if (!TryValidateModel(paymentToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(paymentToPatch, paymentModelFromRepository);

            await _repository.Payment.UpdatePaymentAsync(paymentModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePayment(Guid id)
        {
            var payment = await _repository.Payment.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Payment.DeletePaymentAsync(payment);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
