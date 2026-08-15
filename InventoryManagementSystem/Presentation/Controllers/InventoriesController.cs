using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Application.Inventories.Commands;
using InventoryManagementSystem.Application.Inventories.Queries;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace InventoryManagementSystem.Presentation.WebApi.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoriesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<InventoryDto>>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
           /* bool developmentEnvironment = WebApplication.CreateBuilder().Environment.IsDevelopment();
            if (developmentEnvironment)
            {
                var queryForTest = new GetAllInventoriesQuery();
                var inventoriesForTest = await _mediator.Send(queryForTest, cancellationToken);
                return Ok(inventoriesForTest);
            }
            // return BadRequest("This endpoint is only available in the development environment");*/

            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetAllInventoriesByApplicationUserIdQuery(applicationUserId, pageNumber, pageSize);
            var inventories = await _mediator.Send(query, cancellationToken);
            return Ok(inventories);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<InventoryDto>> GetById(int id)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetInventoryByIdQuery(id, applicationUserId);
            var inventory = await _mediator.Send(query);
            return inventory != null ? Ok(inventory) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<CreateInventoryCommandResponse>> Create(CreateInventoryCommand command)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var commandWithApplicationUserId = new CreateInventoryCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
            //return StatusCode( result ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest);
            // return result ? CreatedAtAction(nameof(GetById), new { id = command.Id }, result): BadRequest();
        }



        [HttpPut]
        public async Task<ActionResult<UpdateInventoryCommandResponse>> Update(UpdateInventoryCommand command)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var commandWithApplicationUserId = new UpdateInventoryCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<DeleteInventoryCommandResponse>> DeleteAsync(int id)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var command = new DeleteInventoryCommand(id, applicationUserId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

           [HttpGet("inventoryIds")]
            public async Task<ActionResult<List<int>>> GetAllIds(CancellationToken cancellationToken = default)
            {   
                            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
                var query = new GetAllInventoryIdsQuery(applicationUserId);
                var inventoryIds = await _mediator.Send(query, cancellationToken);
                return Ok(inventoryIds);
           }
        [HttpGet("report/{inventoryId}")]
        public async Task<ActionResult<InventoryReportDto?>> GetInventoryReport(int inventoryId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetInventoryReportQuery(inventoryId, applicationUserId, fromDate, toDate);
            var inventoryReport = await _mediator.Send(query, cancellationToken);
            return Ok(inventoryReport);
        }

    }

}