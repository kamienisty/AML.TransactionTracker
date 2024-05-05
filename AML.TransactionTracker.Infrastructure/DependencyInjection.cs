using AML.TransactionTracker.Application.Services;
using AML.TransactionTracker.Core.Repositories;
using AML.TransactionTracker.Infrastructure.RabbitMQ;
using AML.TransactionTracker.Infrastructure.SQLite.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace AML.TransactionTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerSQLiteRepository>();
            services.AddScoped<ITransactionRepository, TransactionSQLiteRepository>();

            services.AddSingleton<IEventPublisher, EventPublisherRabbitMQ>();
            services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();

            return services;
        }
    }
}
