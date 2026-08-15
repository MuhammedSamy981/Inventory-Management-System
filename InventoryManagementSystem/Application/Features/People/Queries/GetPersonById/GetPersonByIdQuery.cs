using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.People.Queries
{
  public record GetPersonByIdQuery(int Id,string ApplicationUserId) : IRequest<PersonDto?>;
}