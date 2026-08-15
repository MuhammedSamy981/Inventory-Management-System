using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Application.Items.Commands;
using InventoryManagementSystem.Application.Items.Queries;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace InventoryManagementSystem.Presentation.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<ItemDto>>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
           /* bool developmentEnvironment = WebApplication.CreateBuilder().Environment.IsDevelopment();
            if (developmentEnvironment)
            {
                var queryForTest = new GetAllItemsQuery();
                var itemsForTest = await _mediator.Send(queryForTest, cancellationToken);
                return Ok(itemsForTest);
            }
            //  return BadRequest("This endpoint is only available in the development environment");*/
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetAllItemsByApplicationUserIdQuery(applicationUserId, pageNumber, pageSize);
            var items = await _mediator.Send(query, cancellationToken);
            return Ok(items);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetItemByIdQuery(id, applicationUserId);
            var item = await _mediator.Send(query);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<CreateItemCommandResponse>> Create(CreateItemCommand command)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var commandWithApplicationUserId = new CreateItemCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
            //return StatusCode( result ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest);
            // return result ? CreatedAtAction(nameof(GetById), new { id = command.Id }, result): BadRequest();
        }



        [HttpPut]
        public async Task<ActionResult<UpdateItemCommandResponse>> Update(UpdateItemCommand command)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var commandWithApplicationUserId = new UpdateItemCommandWithApplicationUserId(command, applicationUserId);
            var response = await _mediator.Send(commandWithApplicationUserId);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<DeleteItemCommandResponse>> DeleteAsync(int id)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var command = new DeleteItemCommand(id, applicationUserId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }



        /*[HttpPost("listInventoryIds")]
        public async Task<ActionResult<List<ItemDetailsForEachInventoryDto>>> GetAllbyListInventoryIds(List<int> inventoryIds, CancellationToken cancellationToken = default)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetAllItemsByListInventoryIdsQuery(inventoryIds, applicationUserId);
            var inventoryItems = await _mediator.Send(query, cancellationToken);
            return Ok(inventoryItems);
        }*/

        [HttpPost("report")]
        public async Task<ActionResult<ItemReportDto?>> GetItemReport(int itemId, List<int> inventoryIds, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetItemReportQuery(itemId, inventoryIds, applicationUserId, fromDate, toDate);
            var itemReport = await _mediator.Send(query, cancellationToken);
            return Ok(itemReport);
        }

        [HttpGet("storage/{inventoryId}/{daysCount}/{monthsCount}/{yearsCount}")]
        public async Task<ActionResult<List<StoredItemDetailsDto>>> GetAllbyInventoryIdAndStorageDate(int inventoryId, int daysCount, int monthsCount, int yearsCount, CancellationToken cancellationToken = default)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetStoredItemsReportByInventoryIdQuery(inventoryId, applicationUserId, daysCount, monthsCount, yearsCount);
            var items = await _mediator.Send(query, cancellationToken);
            return Ok(items);
        }

        [HttpGet("expired/{inventoryId}/{daysCount}/{monthsCount}/{yearsCount}")]
        public async Task<ActionResult<List<ExpiredItemDetailsDto>>> GetAllbyInventoryIdAndExpiryDate(int inventoryId, int daysCount, int monthsCount, int yearsCount, CancellationToken cancellationToken = default)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;

            var query = new GetExpiredItemsReportByInventoryIdQuery(inventoryId, applicationUserId, daysCount, monthsCount, yearsCount);
            var items = await _mediator.Send(query, cancellationToken);
            return Ok(items);
        }


        [HttpPost("movements/listInventoryIds")]
        public async Task<ActionResult<ItemsMovementReportDto>> GetAllMovedItemsbyListInventoryIds(List<int> inventoryIds, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken = default)
        {
            var userContext = HttpContext.User;

            if (userContext.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }
            var applicationUserId = userContext.FindFirst(JwtRegisteredClaimNames.Jti)?.Value!;
            var query = new GetItemsMovementReportByListInventoryIdsQuery(inventoryIds, applicationUserId, fromDate, toDate);
            var operations = await _mediator.Send(query, cancellationToken);
            return Ok(operations);
        }
    }

}