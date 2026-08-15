using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Queries
{
    public class GetAllPeopleQueryHandler : IRequestHandler<GetAllPeopleQuery, List<PersonDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPeopleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PersonDto>> Handle(GetAllPeopleQuery request, CancellationToken cancellationToken)
        {
            var persons = await _unitOfWork.PersonRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<List<PersonDto>>(persons);
        }
    }
}