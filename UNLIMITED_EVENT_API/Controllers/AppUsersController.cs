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
    public class AppUsersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly string _baseURL;

        public AppUsersController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _repository.Path = "/pictures/AppUser";
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserResponse>>> GetAppUsers([FromQuery] AppUserParameters appUserParameters)
        {
            var appUsers = await _repository.AppUser.GetAppUsersAsync(appUserParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(appUsers.MetaData));

            _logger.LogInfo($"Returned all appUsers from database.");

            var appUsersResponses = _mapper.Map<IEnumerable<AppUserResponse>>(appUsers);

            appUsersResponses.ToList().ForEach(appUserResponse =>
            {
                if (!string.IsNullOrWhiteSpace(appUserResponse.ImgLink)) appUserResponse.ImgLink = $"{_baseURL}{appUserResponse.ImgLink}";
            });

            return Ok(appUsersResponses);
        }



        [HttpGet("{id}", Name = "AppUserById")]
        public async Task<ActionResult<AppUserResponse>> GetAppUserById(string id)
        {
            var appUser = await _repository.AppUser.GetAppUserByIdAsync(id);

            if (appUser == null)
            {
                _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned appUserRequest with id: {id}");

                var appUserResponses = _mapper.Map<AppUserResponse>(appUser);

                if (!string.IsNullOrWhiteSpace(appUserResponses.ImgLink)) appUserResponses.ImgLink = $"{_baseURL}{appUserResponses.ImgLink}";
               
                return Ok(appUserResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<AppUserResponse>> CreateAppUser([FromBody] AppUserRequest appUser)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (appUser == null)
            {
                _logger.LogError("AppUser object sent from appUser is null.");
                return BadRequest("AppUser object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid appUserRequest object sent from appUser.");
                return BadRequest("Invalid model object");
            }

            var appUserEntity = _mapper.Map<AppUser>(appUser);

            if (await _repository.AppUser.AppUserExistAsync(appUserEntity))
            {
                ModelState.AddModelError("", "This AppUser exists already");
                return base.ValidationProblem(ModelState);
            }

            //await _repository.AppUser.CreateAppUserAsync(appUserEntity);
            await _repository.SaveAsync();

            var appUserResponses = _mapper.Map<AppUserResponse>(appUserEntity);
            return CreatedAtRoute("AppUserById", new { id = appUserResponses.Id }, appUserResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<AppUserResponse>> UpdateAppUser(string id, [FromBody] AppUserRequest appUserRequest)
        {
            if (appUserRequest == null)
            {
                _logger.LogError("AppUser object sent from appUser is null.");
                return BadRequest("AppUser object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid appUserRequest object sent from appUser.");
                return BadRequest("Invalid model object");
            }

            var appUserEntity = await _repository.AppUser.GetAppUserByIdAsync(id);
            if (appUserEntity == null)
            {
                _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(appUserRequest, appUserEntity);


            await _repository.AppUser.UpdateAppUserAsync(appUserEntity);
            await _repository.SaveAsync();

            var appUserResponses = _mapper.Map<AppUserResponse>(appUserEntity);

            if (!string.IsNullOrWhiteSpace(appUserResponses.ImgLink)) appUserResponses.ImgLink = $"{_baseURL}{appUserResponses.ImgLink}";

            return Ok(appUserResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialAppUserUpdate(string Id, JsonPatchDocument<AppUserRequest> patchDoc)
        {
            var appUserModelFromRepository = await _repository.AppUser.GetAppUserByIdAsync(Id);
            if (appUserModelFromRepository == null) return NotFound();

            var appUserToPatch = _mapper.Map<AppUserRequest>(appUserModelFromRepository);
            patchDoc.ApplyTo(appUserToPatch, ModelState);

            if (!TryValidateModel(appUserToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(appUserToPatch, appUserModelFromRepository);

            await _repository.AppUser.UpdateAppUserAsync(appUserModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppUser(string id)
        {
            var appUser = await _repository.AppUser.GetAppUserByIdAsync(id);

            if (appUser == null)
            {
                _logger.LogError($"AppUser with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.AppUser.DeleteAppUserAsync(appUser);

            await _repository.SaveAsync();

            return NoContent();
        }



        [HttpPut("{id}/upload-picture")]
        public async Task<ActionResult<AppUserResponse>> UploadPicture(string id, [FromForm] IFormFile file)
        {
            var appUserEntity = await _repository.AppUser.GetAppUserByIdAsync(id);
            if (appUserEntity == null) return NotFound();

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
                    appUserEntity.ImgLink = uploadResult;
                }
            }

            await _repository.AppUser.UpdateAppUserAsync(appUserEntity);

            await _repository.SaveAsync();

            var appUserResponse = _mapper.Map<AppUserResponse>(appUserEntity);

            if (!string.IsNullOrWhiteSpace(appUserResponse.ImgLink)) appUserResponse.ImgLink = $"{_baseURL}{appUserResponse.ImgLink}";

            return Ok(appUserResponse);
        }
    }
}
