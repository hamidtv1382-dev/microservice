namespace Order_Service.src._01_Domain.Core.Events.Handlers
{
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        Task Handle(T domainEvent);
    }
}
