using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Queries
{
    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetPersonByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PersonDto?> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(request.Id,cancellationToken);
            if (person != null && person.ApplicationUserId == request.ApplicationUserId)
            {
                return _mapper.Map<PersonDto>(person);
            }
            return null;
        }
    }
}