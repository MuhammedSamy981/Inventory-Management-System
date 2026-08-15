using AutoMapper;
using InventoryManagementSystem.Application.Inventories.Commands;
using InventoryManagementSystem.Application.InventoryItems.Commands;
using InventoryManagementSystem.Application.Items.Commands;
using InventoryManagementSystem.Application.Operations.Commands;
using InventoryManagementSystem.Application.People.Commands;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region InventoryMapping
            CreateMap<Inventory, InventoryDto>().ReverseMap();

            CreateMap<CreateInventoryCommand, Inventory>().ReverseMap();

            CreateMap<CreateInventoryCommandWithApplicationUserId, Inventory>()
             .IncludeMembers(src => src.CreateInventoryCommand).ReverseMap();

            CreateMap<UpdateInventoryCommand, Inventory>().ReverseMap();

            CreateMap<UpdateInventoryCommandWithApplicationUserId, Inventory>()
            .IncludeMembers(src => src.UpdateInventoryCommand).ReverseMap();
            #endregion


            #region ItemMapping
            CreateMap<Item, ItemDto>().ReverseMap();

            CreateMap<CreateItemCommand, Item>().ReverseMap();

            CreateMap<CreateItemCommandWithApplicationUserId, Item>()
            .IncludeMembers(src => src.CreateItemCommand).ReverseMap();

            CreateMap<UpdateItemCommand, Item>().ReverseMap();

            CreateMap<UpdateItemCommandWithApplicationUserId, Item>()
            .IncludeMembers(src => src.UpdateItemCommand).ReverseMap();
            #endregion


            #region OperationMapping
            CreateMap<Operation, OperationDto>().ReverseMap();

            CreateMap<CreateOperationCommand, Operation>().ReverseMap();

            CreateMap<CreateOperationCommandWithApplicationUserId, Operation>()
            .IncludeMembers(src => src.CreateOperationCommand)
            .ReverseMap();

            CreateMap<UpdateOperationCommand, Operation>().ReverseMap();

            CreateMap<UpdateOperationCommandWithApplicationUserId, Operation>()
            .IncludeMembers(src => src.UpdateOperationCommand)
            .ReverseMap();

            CreateMap<CreateTransferOperationCommand, Operation>().ReverseMap();

            CreateMap<CreateTransferOperationCommandWithApplicationUserId, Operation>()
            .IncludeMembers(src => src.CreateTransferOperationCommand)
            .ForMember(des=>des.Date, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des=>des.Type, opt => opt.MapFrom(src => "Transfer from InventoryId=" + src.CreateTransferOperationCommand.FromInventoryId))
            .ForMember(des=>des.InventoryId, opt => opt.MapFrom(src => src.CreateTransferOperationCommand.ToInventoryId))
            .ForMember(des=>des.PersonId, opt => opt.MapFrom(src => src.CreateTransferOperationCommand.SupplierId)).ReverseMap();
            #endregion


            #region PersonMapping
            CreateMap<Person, PersonDto>().ReverseMap();

            CreateMap<CreatePersonCommand, Person>().ReverseMap();

            CreateMap<CreatePersonCommandWithApplicationUserId, Person>()
            .IncludeMembers(src => src.CreatePersonCommand)
            .ReverseMap();

            CreateMap<UpdatePersonCommand, Person>().ReverseMap();
            
            CreateMap<UpdatePersonCommandWithApplicationUserId, Person>()
            .IncludeMembers(src => src.UpdatePersonCommand)
            .ReverseMap();
            #endregion

    
        }
    }
}