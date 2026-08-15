using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Commands
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommandWithApplicationUserId, UpdatePersonCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UpdatePersonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)//,  IValidator<Person> PersonValidator )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UpdatePersonCommandResponse> Handle(UpdatePersonCommandWithApplicationUserId request, CancellationToken cancellationToken)
        {
            var person = _mapper.Map<Person>(request);
            await _unitOfWork.PersonRepository.UpdateAsync(person);
            return await _unitOfWork.SaveChangesAsync() != 0 ?
            new UpdatePersonCommandResponse($"{person.Type} updated successfully")
            : new UpdatePersonCommandResponse($"Failed to update {person.Type}");
        }
    }

}