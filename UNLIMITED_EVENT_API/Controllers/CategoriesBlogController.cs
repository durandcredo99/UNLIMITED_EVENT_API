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
    //[Authorize]
    public class CategoriesBlogController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CategoriesBlogController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryBlogResponse>>> GetCategoriesBlog([FromQuery] CategoryBlogParameters categoryBlogParameters)
        {
            var blogcategories = await _repository.CategoryBlog.GetCategoriesBlogAsync(categoryBlogParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(blogcategories.MetaData));

            _logger.LogInfo($"Returned all blogcategories from database.");

            var blogcategoriesResponses = _mapper.Map<IEnumerable<CategoryBlogResponse>>(blogcategories);

            return Ok(blogcategoriesResponses);
        }



        [HttpGet("{id}", Name = "CategoryBlogById")]
        public async Task<ActionResult<CategoryBlogResponse>> GetCategoryBlogById(Guid id)
        {
            var categoryBlog = await _repository.CategoryBlog.GetCategoryBlogByIdAsync(id);

            if (categoryBlog == null)
            {
                _logger.LogError($"CategoryBlog with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned categoryBlogRequest with id: {id}");

                var categoryBlogResponses = _mapper.Map<CategoryBlogResponse>(categoryBlog);
                
                return Ok(categoryBlogResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<CategoryBlogResponse>> CreateCategoryBlog([FromBody] CategoryBlogRequest categoryBlog)
        {
            if (categoryBlog == null)
            {
                _logger.LogError("CategoryBlog object sent from categoryBlog is null.");
                return BadRequest("CategoryBlog object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid categoryBlogRequest object sent from categoryBlog.");
                return BadRequest("Invalid model object");
            }

            var categoryBlogEntity = _mapper.Map<CategoryBlog>(categoryBlog);

            if (await _repository.CategoryBlog.CategoryBlogExistAsync(categoryBlogEntity))
            {
                ModelState.AddModelError("", "This CategoryBlog exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.CategoryBlog.CreateCategoryBlogAsync(categoryBlogEntity);
            await _repository.SaveAsync();

            var categoryBlogResponses = _mapper.Map<CategoryBlogResponse>(categoryBlogEntity);
            return CreatedAtRoute("CategoryBlogById", new { id = categoryBlogResponses.Id }, categoryBlogResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryBlogResponse>> UpdateCategoryBlog(Guid id, [FromBody] CategoryBlogRequest categoryBlogRequest)
        {
            if (categoryBlogRequest == null)
            {
                _logger.LogError("CategoryBlog object sent from categoryBlog is null.");
                return BadRequest("CategoryBlog object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid categoryBlogRequest object sent from categoryBlog.");
                return BadRequest("Invalid model object");
            }

            var categoryBlogEntity = await _repository.CategoryBlog.GetCategoryBlogByIdAsync(id);
            if (categoryBlogEntity == null)
            {
                _logger.LogError($"CategoryBlog with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(categoryBlogRequest, categoryBlogEntity);


            await _repository.CategoryBlog.UpdateCategoryBlogAsync(categoryBlogEntity);
            await _repository.SaveAsync();

            var categoryBlogResponses = _mapper.Map<CategoryBlogResponse>(categoryBlogEntity);
            return Ok(categoryBlogResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialCategoryBlogUpdate(Guid Id, JsonPatchDocument<CategoryBlogRequest> patchDoc)
        {
            var categoryBlogModelFromRepository = await _repository.CategoryBlog.GetCategoryBlogByIdAsync(Id);
            if (categoryBlogModelFromRepository == null) return NotFound();

            var categoryBlogToPatch = _mapper.Map<CategoryBlogRequest>(categoryBlogModelFromRepository);
            patchDoc.ApplyTo(categoryBlogToPatch, ModelState);

            if (!TryValidateModel(categoryBlogToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(categoryBlogToPatch, categoryBlogModelFromRepository);

            await _repository.CategoryBlog.UpdateCategoryBlogAsync(categoryBlogModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryBlog(Guid id)
        {
            var categoryBlog = await _repository.CategoryBlog.GetCategoryBlogByIdAsync(id);

            if (categoryBlog == null)
            {
                _logger.LogError($"CategoryBlog with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.CategoryBlog.DeleteCategoryBlogAsync(categoryBlog);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
