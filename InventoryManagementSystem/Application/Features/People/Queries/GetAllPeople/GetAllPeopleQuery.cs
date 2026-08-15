using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.People.Queries
{
  public record GetAllPeopleQuery : IRequest<List<PersonDto>>;
}