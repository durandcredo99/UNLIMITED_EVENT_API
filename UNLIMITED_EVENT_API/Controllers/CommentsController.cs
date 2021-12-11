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
    public class CommentsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CommentsController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetComments([FromQuery] CommentParameters commentParameters)
        {
            var comments = await _repository.Comment.GetCommentsAsync(commentParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(comments.MetaData));

            _logger.LogInfo($"Returned all comments from database.");

            var commentsResponses = _mapper.Map<IEnumerable<CommentResponse>>(comments);

            return Ok(commentsResponses);
        }



        [HttpGet("{id}", Name = "CommentById")]
        public async Task<ActionResult<CommentResponse>> GetCommentById(Guid id)
        {
            var comment = await _repository.Comment.GetCommentByIdAsync(id);

            if (comment == null)
            {
                _logger.LogError($"Comment with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned commentRequest with id: {id}");

                var commentResponses = _mapper.Map<CommentResponse>(comment);
                
                return Ok(commentResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<CommentResponse>> CreateComment([FromBody] CommentRequest comment)
        {
            if (comment == null)
            {
                _logger.LogError("Comment object sent from comment is null.");
                return BadRequest("Comment object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid commentRequest object sent from comment.");
                return BadRequest("Invalid model object");
            }

            var commentEntity = _mapper.Map<Comment>(comment);

            if (await _repository.Comment.CommentExistAsync(commentEntity))
            {
                ModelState.AddModelError("", "This Comment exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Comment.CreateCommentAsync(commentEntity);
            await _repository.SaveAsync();

            var commentResponses = _mapper.Map<CommentResponse>(commentEntity);
            return CreatedAtRoute("CommentById", new { id = commentResponses.Id }, commentResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<CommentResponse>> UpdateComment(Guid id, [FromBody] CommentRequest commentRequest)
        {
            if (commentRequest == null)
            {
                _logger.LogError("Comment object sent from comment is null.");
                return BadRequest("Comment object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid commentRequest object sent from comment.");
                return BadRequest("Invalid model object");
            }

            var commentEntity = await _repository.Comment.GetCommentByIdAsync(id);
            if (commentEntity == null)
            {
                _logger.LogError($"Comment with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(commentRequest, commentEntity);


            await _repository.Comment.UpdateCommentAsync(commentEntity);
            await _repository.SaveAsync();

            var commentResponses = _mapper.Map<CommentResponse>(commentEntity);
            return Ok(commentResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialCommentUpdate(Guid Id, JsonPatchDocument<CommentRequest> patchDoc)
        {
            var commentModelFromRepository = await _repository.Comment.GetCommentByIdAsync(Id);
            if (commentModelFromRepository == null) return NotFound();

            var commentToPatch = _mapper.Map<CommentRequest>(commentModelFromRepository);
            patchDoc.ApplyTo(commentToPatch, ModelState);

            if (!TryValidateModel(commentToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commentToPatch, commentModelFromRepository);

            await _repository.Comment.UpdateCommentAsync(commentModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            var comment = await _repository.Comment.GetCommentByIdAsync(id);

            if (comment == null)
            {
                _logger.LogError($"Comment with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Comment.DeleteCommentAsync(comment);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
