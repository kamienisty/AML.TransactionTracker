using Microsoft.Extensions.DependencyInjection;

namespace AML.TransactionTracker.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));

            return services;
        }
    }
}
