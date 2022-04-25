using Meetup_API.Data;
using Meetup_API.Helpers;
using Meetup_API.Interfaces.Data;
namespace Meetup_API.Extensions;

public static class ServiceProviderExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
    }
}
