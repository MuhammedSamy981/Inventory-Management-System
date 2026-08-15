using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
    public record GetInventoryReportQuery(int Id, string ApplicationUserId,
    DateTime? FromDate,
    DateTime? ToDate) : IRequest<InventoryReportDto?>;
}