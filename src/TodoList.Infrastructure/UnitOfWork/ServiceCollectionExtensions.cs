using Microsoft.Extensions.DependencyInjection;

namespace TodoList.Infrastructure.UnitOfWork
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
