using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Queries
{
    public class GetAllPeopleByApplicationUserIdQueryHandler : IRequestHandler<GetAllPeopleByApplicationUserIdQuery, List<PersonDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPeopleByApplicationUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PersonDto>> Handle(GetAllPeopleByApplicationUserIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.PersonRepository.GetAllByApplicationUserIdAsync(request.ApplicationUserId, request.PageNumber, request.PageSize, cancellationToken);
            return _mapper.Map<List<PersonDto>>(person);
        }
    }
}