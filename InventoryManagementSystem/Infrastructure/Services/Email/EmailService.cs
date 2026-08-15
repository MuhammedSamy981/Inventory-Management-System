using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using InventoryManagementSystem.Domain.Models;
using InventoryManagementSystem.Application.Interfaces;

namespace InventoryManagementSystem.Infrastructure.Services.Email
{
public class EmailService : IEmailService
{
  private readonly EmailSettings _emailSettings;

  public EmailService(IOptions<EmailSettings> settings)
    {
        _emailSettings = settings.Value;
    }


  public async Task<bool> SendAsync(Message message)
  {
    try
    {
      using (MimeMessage emailMessage = new MimeMessage())
      {
        emailMessage.From.Add(new MailboxAddress( _emailSettings.SenderName,  _emailSettings.SenderEmail));
        emailMessage.To.Add(new MailboxAddress(message.Email, message?.Email!));
        emailMessage.Subject = message?.Subject!;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message?.Body! };
        
        using (SmtpClient mailClient = new SmtpClient())
        {
          await mailClient.ConnectAsync(_emailSettings.Server, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
          await mailClient.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
          await mailClient.SendAsync(emailMessage);
          await mailClient.DisconnectAsync(true);
        }
      }
      return true;
    }
    catch
    {
      return false;
    }
  }
}
}