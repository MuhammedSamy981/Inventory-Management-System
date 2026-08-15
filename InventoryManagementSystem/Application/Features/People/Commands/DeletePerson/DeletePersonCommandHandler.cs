using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Commands
{
public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, DeletePersonCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)//,  IValidator<Person> PersonValidator )
        {            
          _unitOfWork = unitOfWork;
        }
    public async Task<DeletePersonCommandResponse> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person=await _unitOfWork.PersonRepository.GetByIdAsync(request.Id);
        if(person==null)
        {
            return new DeletePersonCommandResponse("The person you are trying to delete no longer exists");
        }
        if(request.ApplicationUserId!=person.ApplicationUserId)
        {
            return new DeletePersonCommandResponse($"You are not authorized to delete this {person.Type}");
        }
        await _unitOfWork.PersonRepository.DeleteAsync(person);
        return await _unitOfWork.SaveChangesAsync() != 0 ?
        new DeletePersonCommandResponse($"{person.Type} deleted successfully")
        : new DeletePersonCommandResponse($"Failed to delete {person.Type}");
    }
}

}