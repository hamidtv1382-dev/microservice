namespace Seller_Finance_Service.src._01_Domain.Core.Common
{
    public abstract class AggregateRoot : BaseEntity
    {
        private readonly List<object> _domainEvents = new List<object>();

        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        
        public void AddDomainEvent(object domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
