using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Items.Commands
{
public class CreateItemCommandHandler : IRequestHandler<CreateItemCommandWithApplicationUserId, CreateItemCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateItemCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {            
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }
    public async Task<CreateItemCommandResponse> Handle(CreateItemCommandWithApplicationUserId request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Item>(request);                
        await _unitOfWork.ItemRepository.AddAsync(item);
        return await _unitOfWork.SaveChangesAsync() != 0 ?
        new CreateItemCommandResponse("item created successfully")
        : new CreateItemCommandResponse("Failed to create item");
    }
}

}