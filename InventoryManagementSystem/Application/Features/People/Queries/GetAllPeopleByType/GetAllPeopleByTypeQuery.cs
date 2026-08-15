using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.People.Queries
{
   public record GetAllPeopleByTypeQuery(string Type,string ApplicationUserId) : IRequest<List<PersonDto>>;
}