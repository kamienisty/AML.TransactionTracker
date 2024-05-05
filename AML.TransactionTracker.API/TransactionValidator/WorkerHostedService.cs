using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using MediatR;
using AML.TransactionTracker.Infrastructure.RabbitMQ;
using AML.TransactionTracker.Application.Commands;
using AML.TransactionTracker.Application.Events;

namespace AML.TransactionTracker.API.TransactionValidator
{
    public class WorkerHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IOptions<RabbitMqOptions> _options;
        private readonly ILogger<WorkerHostedService> _logger;
        public WorkerHostedService(IServiceScopeFactory serviceScopeFactory, IOptions<RabbitMqOptions> options,
            ILogger<WorkerHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var connectionFactory = new ConnectionFactory { HostName = _options.Value.Hostname, DispatchConsumersAsync = true };
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _options.Value.QueueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = JsonSerializer.Deserialize<TransactionAdded>(body);

                    var command = new ValidateTransaction(message.TransactionId);
                    await mediator.Send(command);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An exception occured while consuming message for transaction validation");
                    throw;
                }
            };

            channel.BasicConsume(queue: _options.Value.QueueName,
                                     autoAck: false,
                                     consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
