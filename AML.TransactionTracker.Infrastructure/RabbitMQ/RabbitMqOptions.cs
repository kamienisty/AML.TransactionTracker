namespace AML.TransactionTracker.Infrastructure.RabbitMQ
{
    public class RabbitMqOptions
    {
        public string Hostname { get; set; }
        public string QueueName { get; set; }
    }
}
