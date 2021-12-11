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
    public class CategoriesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CategoriesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories([FromQuery] CategoryParameters categoryParameters)
        {
            //var userFirstName = User.FindFirst(ClaimTypes.GivenName).Value;
            //var userName = User.FindFirst(ClaimTypes.Surname).Value;

            /*
             new Claim(ClaimTypes.GivenName, user.Firstname),
                new Claim(ClaimTypes.Surname, user.Name),
             */
            var categories = await _repository.Category.GetCategoriesAsync(categoryParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categories.MetaData));

            _logger.LogInfo($"Returned all categories from database.");

            var categoriesResponses = _mapper.Map<IEnumerable<CategoryResponse>>(categories);

            return Ok(categoriesResponses);
        }



        [HttpGet("{id}", Name = "CategoryById")]
        public async Task<ActionResult<CategoryResponse>> GetCategoryById(Guid id)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(id);

            if (category == null)
            {
                _logger.LogError($"Category with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned categoryRequest with id: {id}");

                var categoryResponses = _mapper.Map<CategoryResponse>(category);
                
                return Ok(categoryResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CategoryRequest category)
        {
            if (category == null)
            {
                _logger.LogError("Category object sent from category is null.");
                return BadRequest("Category object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid categoryRequest object sent from category.");
                return BadRequest("Invalid model object");
            }

            var categoryEntity = _mapper.Map<Category>(category);

            if (await _repository.Category.CategoryExistAsync(categoryEntity))
            {
                ModelState.AddModelError("", "This Category exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Category.CreateCategoryAsync(categoryEntity);
            await _repository.SaveAsync();

            var categoryResponses = _mapper.Map<CategoryResponse>(categoryEntity);
            return CreatedAtRoute("CategoryById", new { id = categoryResponses.Id }, categoryResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponse>> UpdateCategory(Guid id, [FromBody] CategoryRequest categoryRequest)
        {
            if (categoryRequest == null)
            {
                _logger.LogError("Category object sent from category is null.");
                return BadRequest("Category object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid categoryRequest object sent from category.");
                return BadRequest("Invalid model object");
            }

            var categoryEntity = await _repository.Category.GetCategoryByIdAsync(id);
            if (categoryEntity == null)
            {
                _logger.LogError($"Category with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(categoryRequest, categoryEntity);


            await _repository.Category.UpdateCategoryAsync(categoryEntity);
            await _repository.SaveAsync();

            var categoryResponses = _mapper.Map<CategoryResponse>(categoryEntity);
            return Ok(categoryResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialCategoryUpdate(Guid Id, JsonPatchDocument<CategoryRequest> patchDoc)
        {
            var categoryModelFromRepository = await _repository.Category.GetCategoryByIdAsync(Id);
            if (categoryModelFromRepository == null) return NotFound();

            var categoryToPatch = _mapper.Map<CategoryRequest>(categoryModelFromRepository);
            patchDoc.ApplyTo(categoryToPatch, ModelState);

            if (!TryValidateModel(categoryToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(categoryToPatch, categoryModelFromRepository);

            await _repository.Category.UpdateCategoryAsync(categoryModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            var category = await _repository.Category.GetCategoryByIdAsync(id);

            if (category == null)
            {
                _logger.LogError($"Category with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Category.DeleteCategoryAsync(category);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
