using AML.TransactionTracker.Core.Events;
using MediatR;

namespace AML.TransactionTracker.Application.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEvent @event);
    }
}
