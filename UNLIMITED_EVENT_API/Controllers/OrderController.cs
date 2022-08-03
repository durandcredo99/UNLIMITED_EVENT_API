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
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public OrderController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders([FromQuery] OrderParameters orderParameters)
        {
            var orders = await _repository.Order.GetOrdersAsync(orderParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(orders.MetaData));

            _logger.LogInfo($"Returned all orders from database.");

            var ordersResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);

            return Ok(ordersResponses);
        }



        [HttpGet("{id}", Name = "OrderById")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
        {
            var order = await _repository.Order.GetOrderByIdAsync(id);

            if (order == null)
            {
                _logger.LogError($"Order with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned orderRequest with id: {id}");

                var orderResponses = _mapper.Map<OrderResponse>(order);
                
                return Ok(orderResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] OrderRequest order)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (order == null)
            {
                _logger.LogError("Order object sent from order is null.");
                return BadRequest("Order object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid orderRequest object sent from order.");
                return BadRequest("Invalid model object");
            }

            var orderEntity = _mapper.Map<Order>(order);
            orderEntity.AppUserId = userId;


            if (await _repository.Order.OrderExistAsync(orderEntity))
            {
                ModelState.AddModelError("", "This Order exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Order.CreateOrderAsync(orderEntity);
            await _repository.SaveAsync();

            var orderResponses = _mapper.Map<OrderResponse>(orderEntity);
            return CreatedAtRoute("OrderById", new { id = orderResponses.Id }, orderResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<OrderResponse>> UpdateOrder(Guid id, [FromBody] OrderRequest orderRequest)
        {
            if (orderRequest == null)
            {
                _logger.LogError("Order object sent from order is null.");
                return BadRequest("Order object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid orderRequest object sent from order.");
                return BadRequest("Invalid model object");
            }

            var orderEntity = await _repository.Order.GetOrderByIdAsync(id);
            if (orderEntity == null)
            {
                _logger.LogError($"Order with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(orderRequest, orderEntity);


            await _repository.Order.UpdateOrderAsync(orderEntity);
            await _repository.SaveAsync();

            var orderResponses = _mapper.Map<OrderResponse>(orderEntity);
            return Ok(orderResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialOrderUpdate(Guid Id, JsonPatchDocument<OrderRequest> patchDoc)
        {
            var orderModelFromRepository = await _repository.Order.GetOrderByIdAsync(Id);
            if (orderModelFromRepository == null) return NotFound();

            var orderToPatch = _mapper.Map<OrderRequest>(orderModelFromRepository);
            patchDoc.ApplyTo(orderToPatch, ModelState);

            if (!TryValidateModel(orderToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(orderToPatch, orderModelFromRepository);

            await _repository.Order.UpdateOrderAsync(orderModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var order = await _repository.Order.GetOrderByIdAsync(id);

            if (order == null)
            {
                _logger.LogError($"Order with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Order.DeleteOrderAsync(order);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
