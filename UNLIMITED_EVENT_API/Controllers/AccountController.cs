using AutoMapper;
using Contracts;
using Entities.DataTransfertObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UNLIMITED_EVENT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly string _baseURL;



        public AccountController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _baseURL = string.Concat(httpContextAccessor.HttpContext.Request.Scheme, "://", httpContextAccessor.HttpContext.Request.Host);
        }




        //GET api/accounts/users/count
        [HttpGet("users/count")]
        [AllowAnonymous]
        public async Task<int> GetUsersCount()
        {
            _logger.LogInfo($"Count users of database.");
            return (await _repository.Account.CountUsersAsync());
        }



        //POST api/accounts/user/registration
        [HttpPost("user/registration")]
        [AllowAnonymous]
        public async Task<ActionResult<AppUserResponse>> RegisterUser([FromBody] AppUserRequest registrationRequest)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (await GetUsersCount() < 1)
            {
                ModelState.AddModelError("", "create an admin account first");
                return ValidationProblem(ModelState);
            }

            _logger.LogInfo($"Registration attempt by : {registrationRequest.Firstname } {registrationRequest.Name }");

            var workstation = await _repository.Workstation.GetWorkstationByNameAsync(registrationRequest.WorkstationName);
            if (workstation == null) return NotFound("Workstation not found");

            var userWithoutWorkstation = _mapper.Map<AppUser>(registrationRequest);
            var result = await _repository.Account.RegisterUserAsync(userWithoutWorkstation, registrationRequest.Password);

            var userResponse = _mapper.Map<AppUserResponse>(userWithoutWorkstation);


            if (result.IsSuccess)
            {
                var workstationAssignationResult = await _repository.Account.AddToWorkstationAsync(userWithoutWorkstation, workstation);

                if (workstationAssignationResult.IsSuccess)
                {
                    var userWorkstations = await _repository.Account.GetUsersWorkstationsAsync(userWithoutWorkstation);
                    //if (userWorkstations.Any()) userResponse.Workstation = _mapper.Map<Workstation, WorkstationResponse>(await _repository.Workstation.GetWorkstationByNameAsync(userWorkstations.First()));

                    //await SendVerificationEmail(userResponse.Id);

                    _logger.LogInfo($"Registration was successful");

                    //email verification should be enable later
                    //await SendVerificationEmail(userResponse.Id);
                    return Ok(userResponse);
                }
                else
                {
                    foreach (var error in workstationAssignationResult.ErrorDetails)
                    {
                        ModelState.AddModelError(error, error);
                    }
                    return ValidationProblem(ModelState);
                }
            }
            else
            {
                foreach (var error in result.ErrorDetails)
                {
                    ModelState.AddModelError(error, error);
                }
                _logger.LogError($"Registration failed ErrorMessage : {result.ErrorDetails}");
                return ValidationProblem(ModelState);
            }
        }




        [HttpPost("admin/registration")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<ActionResult<AppUserResponse>> RegisterAdmin([FromBody] AppUserRequest adminRegistrationDto)
        {
            if (await GetUsersCount() >= 1) return BadRequest("Admin registration shortcut is no longer available");

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var workstation = await _repository.Workstation.GetWorkstationByNameAsync("SuperAdmin");
            if (workstation == null) return NotFound("Workstation not found");

            var admin = _mapper.Map<AppUser>(adminRegistrationDto);
            var result = await _repository.Account.RegisterUserAsync(admin, adminRegistrationDto.Password);
            var userResponse = _mapper.Map<AppUserResponse>(admin);


            if (result.IsSuccess)
            {
                var workstationAssignationResult = await _repository.Account.AddToWorkstationAsync(admin, workstation);

                if (workstationAssignationResult.IsSuccess)
                {
                    var userWorkstations = await _repository.Account.GetUsersWorkstationsAsync(admin);
                    //if (userWorkstations.Any()) userResponse.Workstation = _mapper.Map<Workstation, WorkstationResponse>(await _repository.Workstation.GetWorkstationByNameAsync(userWorkstations.First()));

                    //await SendVerificationEmail(userResponse.Id);

                    return Ok(userResponse);
                }
                else
                {
                    foreach (var error in workstationAssignationResult.ErrorDetails)
                    {
                        ModelState.AddModelError(error, error);
                    }
                    return ValidationProblem(ModelState);
                }
            }
            else
            {
                foreach (var error in result.ErrorDetails)
                {
                    ModelState.AddModelError(error, error);
                }
                return ValidationProblem(ModelState);
            }
        }




        //POST api/accounts/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInfo($"Account attempt");

                var loginModel = _mapper.Map<LoginModel>(loginRequest);
                var authentication = await _repository.Account.SignInAsync(loginModel, loginRequest.Password);

                if (authentication.IsSuccess)
                {
                    _logger.LogInfo($"User Named: {authentication.UserInfo["Name"]} has logged in successfully");

                    var authenticationResponse = _mapper.Map<Authentication>(authentication);

                    if (authenticationResponse.UserInfo["ImgUrl"] != null) authenticationResponse.UserInfo["ImgUrl"] = $"{_baseURL}{authenticationResponse.UserInfo["ImgUrl"]}";

                    return Ok(authenticationResponse);
                }

                _logger.LogError($"Account failed : {authentication.Message}");
                return NotFound(authentication.Message);
            }

            return ValidationProblem(ModelState);
        }




        //POST api/accounts/login
        [HttpPost("resend-verification-email")]
        public async Task<ActionResult> ResendVerificationEmail()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("userId or token invalid");

            await SendVerificationEmail(userId);

            return Ok("Verification email sent successfully");
        }




        private async Task SendVerificationEmail(string userId)
        {
            var user = await _repository.AppUser.GetAppUserByIdAsync(userId);
            var token = await _repository.Account.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = await _repository.Account.EncodeTokenAsync(token);

            string url = $"{_baseURL}/api/accounts/confirmemail?userId={userId}&token={encodedToken}";

            var email = new EmailModel
            {
                ToEmail = user.Email,
                ToName = user.Firstname,
                Subject = "Confirm your email",
                Body = $"<h1>Welcome to A-UN</h1><p>Please confirm your email by <a href='{url}'>Clicking here</a></p>",
            };

            await _repository.Mail.SendEmailAsync(email);
        }




        //POST api/accounts/login
        [HttpGet("confirmemail")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest("userId or token invalid");

            var result = await _repository.Account.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Ok($"{result.UserInfo["Email"]} Email confirmed");
            }

            return BadRequest(result);
        }




        //POST api/accounts/forgetpassword
        [HttpPost("forgetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("invalid email");

            var result = await _repository.Account.ForgetPasswordAsync(email);

            if (result.IsSuccess)
            {

                string url = $"{_baseURL}/api/accounts/resetpassword?email={email}&token={result.Token}";

                var emailData = new EmailModel
                {
                    ToEmail = result.UserInfo["Email"],
                    ToName = result.UserInfo["Name"],
                    Subject = "Reset Password",
                    Body = $"<h1>Follow the instruction to reset your password</h1> <p> To reset your password <a href='{url}'>Clicking here</a></p>",
                };

                await _repository.Mail.SendEmailAsync(emailData);

                return Ok(result);
            }

            return BadRequest(result);
        }




        //POST api/accounts/forgetpassword
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.Account.ResetPasswordAsync(model);

                if (result.IsSuccess) return Ok(result);
            }

            return ValidationProblem(ModelState);
        }
    }
}
