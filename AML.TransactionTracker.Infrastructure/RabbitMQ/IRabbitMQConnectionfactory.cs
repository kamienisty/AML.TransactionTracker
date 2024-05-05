using RabbitMQ.Client;

namespace AML.TransactionTracker.Infrastructure.RabbitMQ
{
    public interface IRabbitMqConnectionFactory
    {
        IConnection GetConnection();
    }
}
