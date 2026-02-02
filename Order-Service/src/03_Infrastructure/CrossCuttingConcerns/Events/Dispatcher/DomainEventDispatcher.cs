using Order_Service.src._01_Domain.Core.Events;
using Order_Service.src._01_Domain.Core.Events.Handlers;

namespace Order_Service.src._03_Infrastructure.CrossCuttingConcerns.Events.Dispatcher
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(IDomainEvent domainEvent)
        {
            var eventType = domainEvent.GetType();
            // Convention: Handler name is {EventName}Handler
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

            // Note: In a real scenario with DI, we might need to resolve IEnumerable<IDomainEventHandler<T>>
            // Here we resolve a specific handler based on naming convention or explicit registration

            var handler = _serviceProvider.GetService(handlerType);

            if (handler != null)
            {
                var method = handlerType.GetMethod("Handle");
                if (method != null)
                {
                    await (Task)method.Invoke(handler, new object[] { domainEvent });
                }
            }
        }
    }

    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent domainEvent);
    }
}
