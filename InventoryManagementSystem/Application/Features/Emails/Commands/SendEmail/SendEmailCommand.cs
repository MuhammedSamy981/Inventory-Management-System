using MediatR;
using InventoryManagementSystem.Domain.Models;

namespace InventoryManagementSystem.Application.Emails.Commands
{
    public record SendEmailCommand(Message Message) : IRequest<bool>;
}
