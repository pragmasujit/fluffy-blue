using HouseBroker.Application.Providers.CommissionProvider;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.Services;
using HouseBroker.Application.UnitOfWork;
using HouseBroker.Infrastructure.Data;
using HouseBroker.Infrastructure.Repositories;
using HouseBroker.Shared.Subject;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Extensions;

public static class ServiceExtensions
{
    public static void AddHouseBroker(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<HouseBrokerDbContext>(x =>
        {
            x.UseSqlServer(connectionString);
        });

        services
            .AddIdentity<IdentityUser, IdentityRole>(x =>
            {
            })
            .AddEntityFrameworkStores<HouseBrokerDbContext>()
            .AddUserManager<UserManager<IdentityUser>>()
            .AddRoleManager<RoleManager<IdentityRole>>();

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(x =>
            {
            });

        services.AddMediator(typeof(HouseBroker.Application.Queries.GetPublicPropertyListings).Assembly);

        services.AddRepositories();

        services.AddServices();
            
        services.AddAuthorization();

        services.AddControllers();
        
        services.AddMemoryCache();

        services.AddSubject();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPropertyListingReadRepository, PropertyListingReadRepository>();
        
        services.AddScoped<IPropertyListingWriteRepository, PropertyListingWriteRepository>();
        
        services.AddScoped<ICommissionWriteRepository, CommissionWriteRepository>();

        services.AddScoped<IHouseBrokerUnitOfWork, HouseBrokerUnitOfWork>();

        services.AddProviders();
        
        return services;
    }
    
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<ICommissionProvider, CommissionProvider>();

        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICommissionEngine, CommissionEngine>();
                 
        return services;
    }
    
    public static IServiceCollection AddSubject(this IServiceCollection services)
    {
        services.AddScoped<Subject>(x =>
        {
            var httpContextAccessor = x.GetRequiredService<IHttpContextAccessor>();
            var context = httpContextAccessor.HttpContext;

            if (context?.User?.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                return new Subject(userId);
            }

            return new Subject();
        });
                 
        return services;
    }
}