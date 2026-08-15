using InventoryManagementSystem.Infrastructure.Data;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Presentation.Middlewares
{
    public class AuditLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditLogMiddleware> _logger;

        public AuditLogMiddleware(RequestDelegate next, ILogger<AuditLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            var request = context.Request;
            var auditLog = new
            {
                UserName = context?.User.Identity!.Name ?? "Anonymous",
                Path = request.Path,
                Method = request.Method,
                TimeStamp = DateTime.UtcNow,
                IPAddress = context?.Connection.RemoteIpAddress?.ToString()
            };

            _logger.LogInformation("Audit Log: {@AuditLog}", auditLog);
            var auditLogToAdd = new AuditLog
            {
                UserName = auditLog.UserName,
                Path = auditLog.Path,
                Method = auditLog.Method,
                TimeStamp = auditLog.TimeStamp,
                IPAddress = auditLog.IPAddress!
            };
            dbContext.AuditLogs.Add(auditLogToAdd);
            await dbContext.SaveChangesAsync();

            await _next(context!);
        }
    }

}