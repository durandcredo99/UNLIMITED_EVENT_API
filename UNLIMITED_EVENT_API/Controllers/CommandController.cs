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
    public class CommandController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public CommandController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandResponse>>> GetCommands([FromQuery] CommandParameters commandParameters)
        {
            var commands = await _repository.Command.GetCommandsAsync(commandParameters);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(commands.MetaData));

            _logger.LogInfo($"Returned all commands from database.");

            var commandsResponses = _mapper.Map<IEnumerable<CommandResponse>>(commands);

            return Ok(commandsResponses);
        }



        [HttpGet("{id}", Name = "CommandById")]
        public async Task<ActionResult<CommandResponse>> GetCommandById(Guid id)
        {
            var command = await _repository.Command.GetCommandByIdAsync(id);

            if (command == null)
            {
                _logger.LogError($"Command with id: {id}, hasn't been found.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned commandRequest with id: {id}");

                var commandResponses = _mapper.Map<CommandResponse>(command);
                
                return Ok(commandResponses);
            }
        }



        [HttpPost]
        public async Task<ActionResult<CommandResponse>> CreateCommand([FromBody] CommandRequest command)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (command == null)
            {
                _logger.LogError("Command object sent from command is null.");
                return BadRequest("Command object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid commandRequest object sent from command.");
                return BadRequest("Invalid model object");
            }

            var commandEntity = _mapper.Map<Command>(command);
            commandEntity.AppUserId = userId;


            if (await _repository.Command.CommandExistAsync(commandEntity))
            {
                ModelState.AddModelError("", "This Command exists already");
                return base.ValidationProblem(ModelState);
            }


            await _repository.Command.CreateCommandAsync(commandEntity);
            await _repository.SaveAsync();

            var commandResponses = _mapper.Map<CommandResponse>(commandEntity);
            return CreatedAtRoute("CommandById", new { id = commandResponses.Id }, commandResponses);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<CommandResponse>> UpdateCommand(Guid id, [FromBody] CommandRequest commandRequest)
        {
            if (commandRequest == null)
            {
                _logger.LogError("Command object sent from command is null.");
                return BadRequest("Command object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid commandRequest object sent from command.");
                return BadRequest("Invalid model object");
            }

            var commandEntity = await _repository.Command.GetCommandByIdAsync(id);
            if (commandEntity == null)
            {
                _logger.LogError($"Command with id: {id}, hasn't been found.");
                return NotFound();
            }

            _mapper.Map(commandRequest, commandEntity);


            await _repository.Command.UpdateCommandAsync(commandEntity);
            await _repository.SaveAsync();

            var commandResponses = _mapper.Map<CommandResponse>(commandEntity);
            return Ok(commandResponses);
        }




        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialCommandUpdate(Guid Id, JsonPatchDocument<CommandRequest> patchDoc)
        {
            var commandModelFromRepository = await _repository.Command.GetCommandByIdAsync(Id);
            if (commandModelFromRepository == null) return NotFound();

            var commandToPatch = _mapper.Map<CommandRequest>(commandModelFromRepository);
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepository);

            await _repository.Command.UpdateCommandAsync(commandModelFromRepository);
                             
            await _repository.SaveAsync();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommand(Guid id)
        {
            var command = await _repository.Command.GetCommandByIdAsync(id);

            if (command == null)
            {
                _logger.LogError($"Command with id: {id}, hasn't been found.");
                return NotFound();
            }


            await _repository.Command.DeleteCommandAsync(command);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
