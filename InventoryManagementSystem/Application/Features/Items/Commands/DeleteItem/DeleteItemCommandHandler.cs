using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Items.Commands
{
public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, DeleteItemCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteItemCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)//,  IValidator<Item> ItemValidator )
        {            
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }
    public async Task<DeleteItemCommandResponse> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item=await _unitOfWork.ItemRepository.GetByIdAsync(request.Id);
        if(item==null)
        {
            return new DeleteItemCommandResponse("The item you are trying to delete no longer exists");
        }
        if(request.ApplicationUserId!=item.ApplicationUserId)
        {
            return new DeleteItemCommandResponse("You are not authorized to delete this item");
        }

        await _unitOfWork.ItemRepository.DeleteAsync(item);
        return await _unitOfWork.SaveChangesAsync() != 0 ?
        new DeleteItemCommandResponse("Item deleted successfully")
        : new DeleteItemCommandResponse("Failed to delete item");
    }
}

}