using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.TransactionTracker.Infrastructure.RabbitMQ
{
    internal sealed class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        private readonly ConnectionFactory _connectionFactory;
        public RabbitMqConnectionFactory(IOptions<RabbitMqOptions> options)
        {
            _connectionFactory = new ConnectionFactory { HostName = options.Value.Hostname };
        }

        public IConnection GetConnection() => _connectionFactory.CreateConnection();
    }
}
