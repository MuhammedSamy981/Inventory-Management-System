using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Application.People.Commands;
using InventoryManagementSystem.Application.People.Queries;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace InventoryManagementSystem.Presentation.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PeopleController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<PersonDto>>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
          /*  bool developmentEnvironment = WebApplication.CreateBuilder().Environment.IsDevelopment();
            if (developmentEnvironment)
            {
                var queryForTest = new GetAllPeopleQuery();
                var peopleForTest = await _mediator.Send(queryForTest, cancellationToken);
                return Ok(peopleForTest);
            }
            // return BadRequest("This endpoint is only available in the development environment");*/
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetAllPeopleByApplicationUserIdQuery(applicationUserId, pageNumber, pageSize);
            var people = await _mediator.Send(query, cancellationToken);
            return Ok(people);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PersonDto>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetPersonByIdQuery(id, applicationUserId);
            var person = await _mediator.Send(query,cancellationToken);
            return person != null ? Ok(person) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<CreatePersonCommandResponse>> Create(CreatePersonCommand command)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var commandWithApplicationUserId = new CreatePersonCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
            //return StatusCode( result ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest);
            // return result ? CreatedAtAction(nameof(GetById), new { id = command.Id }, result): BadRequest();
        }



        [HttpPut]
        public async Task<ActionResult<UpdatePersonCommandResponse>> Update(UpdatePersonCommand command)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var commandWithApplicationUserId = new UpdatePersonCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<DeletePersonCommandResponse>> DeleteAsync(int id)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var command = new DeletePersonCommand(id, applicationUserId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        [Route("type/{type}")]
        public async Task<ActionResult<PersonDto>> GetAllByType(string type, CancellationToken cancellationToken = default)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetAllPeopleByTypeQuery(type, applicationUserId);
            var people = await _mediator.Send(query, cancellationToken);
            return Ok(people);
        }

    }

}