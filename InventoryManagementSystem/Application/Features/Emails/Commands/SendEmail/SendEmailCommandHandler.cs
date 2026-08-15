using MediatR;
using InventoryManagementSystem.Application.Interfaces;

namespace InventoryManagementSystem.Application.Emails.Commands
{
  public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, bool>
  {
    private readonly IEmailService _emailService;

    public SendEmailCommandHandler(IEmailService emailService)
    {
      _emailService = emailService;
    }
    public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
      return await _emailService.SendAsync(request.Message);
    }
  }

}