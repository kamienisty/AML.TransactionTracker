using AML.TransactionTracker.Application.Events;
using AML.TransactionTracker.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;

namespace AML.TransactionTracker.Infrastructure.Tests.RabbitMQ
{
    public class EventPublisherRabbitMQTests
    {
        private Mock<ILogger<EventPublisherRabbitMQ>> _loggerMock;
        private Mock<IRabbitMqConnectionFactory> _connectionFactoryMock;
        private Mock<IConnection> _connectionMock;
        private Mock<IModel> _modelMock;
        private Mock<IBasicProperties> _propertiesMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<EventPublisherRabbitMQ>>();
            _connectionFactoryMock = new Mock<IRabbitMqConnectionFactory>();
            _connectionMock = new Mock<IConnection>();
            _modelMock = new Mock<IModel>();
            _propertiesMock = new Mock<IBasicProperties>();

            _modelMock.Setup(x => x.CreateBasicProperties()).Returns(_propertiesMock.Object);

            _connectionMock.Setup(x => x.CreateModel()).Returns(_modelMock.Object);
            _connectionFactoryMock.Setup(x => x.GetConnection()).Returns(_connectionMock.Object);
        }

        [Test]
        public async Task PublishAsync_CallsRabbitMq_WhenReceiveingEvent()
        {
            var id = Guid.Parse("b7e8e8d7-38f3-4599-b98c-9703cd2d0da4");
            var routingKey = typeof(TransactionAdded).Name;

            var message = new TransactionAdded(id);
            var eventPublisher = new EventPublisherRabbitMQ(_loggerMock.Object, _connectionFactoryMock.Object);

            await eventPublisher.PublishAsync(message);

            _modelMock.Verify(x =>
                x.BasicPublish(
                    It.IsAny<string>(),
                    It.Is<string>( x => x == routingKey),
                    It.IsAny<bool>(),
                    It.IsAny<IBasicProperties>(),
                    It.IsAny<ReadOnlyMemory<byte>>()),
                Times.Once);
        }
    }
}
