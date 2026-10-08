using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ploch.Data.GenericRepository.EFCore;

namespace Ploch.MyApp.Data;

public static class ServiceCollectionRegistrations
{
    public static IServiceCollection AddDataServices(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureOptions,
        IConfiguration? configuration = null)
    {
        services.AddDbContext<MyAppDbContext>(configureOptions);
        services.AddRepositories<MyAppDbContext>(configuration);

        return services;
    }
}
