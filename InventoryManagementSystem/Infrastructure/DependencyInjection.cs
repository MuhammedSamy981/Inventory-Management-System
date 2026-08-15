using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InventoryManagementSystem.Infrastructure.Data;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Infrastructure.Repositories;
using InventoryManagementSystem.Infrastructure.Services.JwtToken;
using InventoryManagementSystem.Application.Interfaces;

namespace InventoryManagementSystem.Infrastructure.DependencyInjection
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {   
                     /*        services.AddIdentityCore<ApplicationUser>(options =>
                        {
                            options.Password.RequiredLength = 8;
                            options.User.RequireUniqueEmail = true;
                        })
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddRoles<IdentityRole>()
                        .AddTokenProvider<TokenProviderDescriptor>(TokenOptions.DefaultProvider);*/

            // services.AddScoped<IIdentityService, IdentityService>();

            #region Database
            string? connectionString = configuration.GetConnectionString("InventoryManagementSystem");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            #endregion


            #region Repositories
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
            services.AddScoped<IOperationRepository, OperationRepository>();
            #endregion

            #region Unit of work
            services.AddScoped<IUnitOfWork, InventoryManagementSystem.Infrastructure.UnitOfWork.UnitOfWork>();
            #endregion

            #region Jwt token
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            #endregion

            return services;
        }
    }

}