using Meetup_API.Data;
using Meetup_API.Helpers;
using Meetup_API.Interfaces;
using Meetup_API.Interfaces.Data;
using Meetup_API.Services;

namespace Meetup_API.Extensions;

public static class ServiceProviderExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        services.AddScoped<ITokenService, TokenService>();
    }
}
