using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Application.Operations.Commands;
using InventoryManagementSystem.Application.Operations.Queries;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace InventoryManagementSystem.Presentation.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OperationsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<OperationDto>>> GetAll(int pageNumber = 1, int pageSize = 10,CancellationToken cancellationToken = default)
        {
           /*  bool developmentEnvironment = WebApplication.CreateBuilder().Environment.IsDevelopment();
            if (developmentEnvironment)
            {
                var queryForTest = new GetAllOperationsQuery();
                var operationsForTest = await _mediator.Send(queryForTest, cancellationToken);
                return Ok(operationsForTest);
            }
            //return BadRequest("This endpoint is only available in the development environment");*/
        var userContext = HttpContext.User;

        if (userContext.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }
        var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetAllOperationsByApplicationUserIdQuery(applicationUserId, pageNumber, pageSize);
            var operations = await _mediator.Send(query, cancellationToken);
            return Ok(operations);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<OperationDto>> GetById(int id, CancellationToken cancellationToken = default)
        {
                var userContext = HttpContext.User;

        if (userContext.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }
        var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetOperationByIdQuery(id,applicationUserId);
            var operation = await _mediator.Send(query,cancellationToken);
            return operation != null ? Ok(operation) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<CreateOperationCommandResponse>> Create(CreateOperationCommand command)
        {
                            var userContext = HttpContext.User;

        if (userContext.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }
        var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
        var commandWithApplicationUserId = new CreateOperationCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
            //return StatusCode( result ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest);
            // return result ? CreatedAtAction(nameof(GetById), new { id = command.Id }, result): BadRequest();
        }


        [HttpPut]
        public async Task<ActionResult<UpdateOperationCommandResponse>> Update(UpdateOperationCommand command)
        {
                            var userContext = HttpContext.User;

        if (userContext.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }
        var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
        var commandWithApplicationUserId = new UpdateOperationCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<DeleteOperationCommandResponse>> DeleteAsync(int id)
        {
                            var userContext = HttpContext.User;

        if (userContext.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }
        var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var command = new DeleteOperationCommand(id, applicationUserId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        [Route("type/{type}")]
        public async Task<ActionResult<List<OperationDto>>> GetAllByType(string type, int pageNumber = 1, int pageSize = 10,CancellationToken cancellationToken = default)
        {
                         var userContext = HttpContext.User;

        if (userContext.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }
        var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetAllOperationsByTypeQuery(type, applicationUserId, pageNumber, pageSize);
            var operations = await _mediator.Send(query, cancellationToken);
            return Ok(operations);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<CreateTransferOperationCommandResponse>> CreateTransfer(CreateTransferOperationCommand command)
        {
                            var userContext = HttpContext.User;

        if (userContext.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }
        var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
        var commandWithApplicationUserId = new CreateTransferOperationCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
        }

        /*    [HttpGet("filterByInventory")]
            public async Task<ActionResult<List<OperationDto>>> GetAllFilteredByInventoryAsync([FromQuery]OperationFilters filters, CancellationToken cancellationToken = default)
            {   
                    var query = new GetAllOperationsFilteredByInventoryQuery(filters);
                        var operations = await _mediator.Send(query, cancellationToken);
                return Ok(operations);
           }

            [HttpGet("filterByOperation")]
            public async Task<ActionResult<List<OperationDto>>> GetAllFilteredByOperationAsync([FromQuery]OperationFilters filters, CancellationToken cancellationToken = default)
            {   
                    var query = new GetAllOperationsFilteredByOperationQuery(filters);
                        var operations = await _mediator.Send(query, cancellationToken);
                return Ok(operations);
           }


               [HttpGet]
            [Route("inventory/{inventoryId}")]
            public async Task<ActionResult<OperationDto>> GetAllMovedOperationsByinventoryId(int inventoryId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, CancellationToken cancellationToken = default)
            {
                var query = new GetAllOperationsMovedOperationsByInventoryIdQuery(inventoryId,fromDate, toDate);
                var operations = await _mediator.Send(query, cancellationToken);
                return Ok(operations);
            }*/
    }

}