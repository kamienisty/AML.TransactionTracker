using AML.TransactionTracker.Application.Services;
using AML.TransactionTracker.Core.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.Retry;
using Polly;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using RabbitMQ.Client;
using System.Text.Json;

namespace AML.TransactionTracker.Infrastructure.RabbitMQ
{
    public sealed class EventPublisherRabbitMQ : IEventPublisher
    {
        private readonly ILogger<EventPublisherRabbitMQ> _logger;
        private readonly ResiliencePipeline _pipeline;
        private readonly IRabbitMqConnectionFactory _connectionFactory;

        public EventPublisherRabbitMQ(ILogger<EventPublisherRabbitMQ> logger, IRabbitMqConnectionFactory connectionFactory)
        {
            _logger = logger;
            _pipeline = CreateResiliencePipeline(10);
            _connectionFactory = connectionFactory;
        }

        public Task PublishAsync(IEvent @event)
        {
            var routingKey = @event.GetType().Name;

            _logger.LogTrace("Creating RabbitMQ channel to publish event: ({EventName})", routingKey);

            using var connection = _connectionFactory.GetConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: routingKey,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var body = SerializeMessage(@event);

            return _pipeline.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                _logger.LogTrace("Publishing event to RabbitMQ");

                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: routingKey,
                    basicProperties: properties,
                    body: body);

                return Task.CompletedTask;

            });
        }
        private byte[] SerializeMessage(IEvent @event) => JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

        private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
        {
            var retryOptions = new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
                MaxRetryAttempts = retryCount,
                DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
            };

            return new ResiliencePipelineBuilder()
                .AddRetry(retryOptions)
                .Build();

            static TimeSpan? GenerateDelay(int attempt)
            {
                return TimeSpan.FromSeconds(Math.Pow(2, attempt));
            }
        }
    }
}
