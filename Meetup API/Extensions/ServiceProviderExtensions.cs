using Meetup_API.Data;
using Meetup_API.Interfaces.Data;

namespace Meetup_API.Extensions;

public static class ServiceProviderExtensions
{
    public static void AddUnitOfWorkService(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
