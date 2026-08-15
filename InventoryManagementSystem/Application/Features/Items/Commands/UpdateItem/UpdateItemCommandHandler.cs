using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Items.Commands
{
public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommandWithApplicationUserId, UpdateItemCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public UpdateItemCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)//,  IValidator<Item> ItemValidator )
        {            
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

    public async Task<UpdateItemCommandResponse> Handle(UpdateItemCommandWithApplicationUserId request, CancellationToken cancellationToken)
    {  
        var item = _mapper.Map<Item>(request);
        await _unitOfWork.ItemRepository.UpdateAsync(item);
        return await _unitOfWork.SaveChangesAsync() != 0 ?
        new UpdateItemCommandResponse("Item updated successfully")
        : new UpdateItemCommandResponse("Failed to update item");
    }
}

}