using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Queries
{
public class GetAllPeopleByTypeQueryHandler : IRequestHandler<GetAllPeopleByTypeQuery, List<PersonDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPeopleByTypeQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

    public async Task<List<PersonDto>> Handle(GetAllPeopleByTypeQuery request, CancellationToken cancellationToken)
    {
        var person =await _unitOfWork.PersonRepository.GetAllByTypeAsync(request.Type,request.ApplicationUserId,cancellationToken);
        return _mapper.Map<List<PersonDto>>(person);
    }
}
}