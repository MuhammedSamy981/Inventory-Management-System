using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Commands
{
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommandWithApplicationUserId, CreatePersonCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreatePersonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreatePersonCommandResponse> Handle(CreatePersonCommandWithApplicationUserId request, CancellationToken cancellationToken)
        {
            var person = _mapper.Map<Person>(request);
            await _unitOfWork.PersonRepository.AddAsync(person);
            return await _unitOfWork.SaveChangesAsync() != 0 ?
            new CreatePersonCommandResponse($"{person.Type} created successfully")
            : new CreatePersonCommandResponse($"Failed to create {person.Type}");
        }
    }

}