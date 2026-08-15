using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.People.Queries
{
    public record GetAllPeopleByApplicationUserIdQuery(string ApplicationUserId, int PageNumber, int PageSize) : IRequest<List<PersonDto>>;
}