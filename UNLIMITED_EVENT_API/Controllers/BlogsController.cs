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
    public class BlogsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public BlogsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/Blog";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogResponse>>> GetBlogs([FromQuery] BlogParameters blogParameters)
        {
            var blogs = await _repository.Blog.GetBlogsAsync(blogParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(blogs.MetaData));

            _logger.LogInfo($"Returned all blogs from database.");

            var blogsResponses = _mapper.Map<IEnumerable<BlogResponse>>(blogs);

            blogsResponses.ToList().ForEach(blogResponse =>
            {
                if (!string.IsNullOrWhiteSpace(blogResponse.ImgLink)) blogResponse.ImgLink = $"{_baseURL}{blogResponse.ImgLink}";
            });

            return Ok(blogsResponses);
        }



        [HttpGet("{id}", Name = "BlogById")]
        public async Task<ActionResult<BlogResponse>> GetBlogById(Guid id)
        {
            var blog = await _repository.Blog.GetBlogByIdAsync(id);

            if (blog == null)
            {
                _logger.LogError($"Blog with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned blogRequest with id: {id}");

                var blogResponses = _mapper.Map<BlogResponse>(blog);

                if (!string.IsNullOrWhiteSpace(blogResponses.ImgLink)) blogResponses.ImgLink = $"{_baseURL}{blogResponses.ImgLink}";
               
                return Ok(blogResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<BlogResponse>> CreateBlog([FromBody] BlogRequest blog)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (blog == null)
            {
                _logger.LogError("Blog object sent from blog is null.");
                return BadRequest("Blog object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid blogRequest object sent from blog.");
                return BadRequest("Invalid model object");
            }

            var blogEntity = _mapper.Map<Blog>(blog);
            blogEntity.AppUserId = userId;

            if (await _repository.Blog.BlogExistAsync(blogEntity))
            {
                ModelState.AddModelError("", "This Blog exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Blog.CreateBlogAsync(blogEntity);
            await _repository.SaveAsync();

            var blogResponses = _mapper.Map<BlogResponse>(blogEntity);
            return CreatedAtRoute("BlogById", new { id = blogResponses.Id }, blogResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<BlogResponse>> UpdateBlog(Guid id, [FromBody] BlogRequest blogRequest)
        {
            if (blogRequest == null)
            {
                _logger.LogError("Blog object sent from blog is null.");
                return BadRequest("Blog object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid blogRequest object sent from blog.");
                return BadRequest("Invalid model object");
            }

            var blogEntity = await _repository.Blog.GetBlogByIdAsync(id);
            if (blogEntity == null)
            {
                _logger.LogError($"Blog with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(blogRequest, blogEntity);


            await _repository.Blog.UpdateBlogAsync(blogEntity);
            await _repository.SaveAsync();

            var blogResponses = _mapper.Map<BlogResponse>(blogEntity);

            if (!string.IsNullOrWhiteSpace(blogResponses.ImgLink)) blogResponses.ImgLink = $"{_baseURL}{blogResponses.ImgLink}";

            return Ok(blogResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialBlogUpdate(Guid Id, JsonPatchDocument<BlogRequest> patchDoc)
        {
            var blogModelFromRepository = await _repository.Blog.GetBlogByIdAsync(Id);
            if (blogModelFromRepository == null) return NotFound();

            var blogToPatch = _mapper.Map<BlogRequest>(blogModelFromRepository);
            patchDoc.ApplyTo(blogToPatch, ModelState);

            if (!TryValidateModel(blogToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(blogToPatch, blogModelFromRepository);

            await _repository.Blog.UpdateBlogAsync(blogModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlog(Guid id)
        {
            var blog = await _repository.Blog.GetBlogByIdAsync(id);

            if (blog == null)
            {
                _logger.LogError($"Blog with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Blog.DeleteBlogAsync(blog);

            await _repository.SaveAsync();

            return NoContent();
        }



        [HttpPut("{id}/upload-picture")]
        public async Task<ActionResult<BlogResponse>> UploadPicture(Guid id, [FromForm] IFormFile file)
        {
            var blogEntity = await _repository.Blog.GetBlogByIdAsync(id);
            if (blogEntity == null) return NotFound();

            if (file != null)
            {
                _repository.File.FilePath = id.ToString();

                var uploadResult = await _repository.File.UploadFile(file);

                if (uploadResult == null)
                {
                    ModelState.AddModelError("", "something went wrong when uploading the picture");
                    return ValidationProblem(ModelState);
                }
                else
                {
                    blogEntity.ImgLink = uploadResult;
                }
            }

            await _repository.Blog.UpdateBlogAsync(blogEntity);

            await _repository.SaveAsync();

            var blogResponse = _mapper.Map<BlogResponse>(blogEntity);

            if (!string.IsNullOrWhiteSpace(blogResponse.ImgLink)) blogResponse.ImgLink = $"{_baseURL}{blogResponse.ImgLink}";

            return Ok(blogResponse);
        }
    }
}
