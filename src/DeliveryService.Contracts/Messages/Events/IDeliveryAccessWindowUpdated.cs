namespace DeliveryService.Contracts.Messages.Events
{
    using System;
    using DeliveryService.Contracts.Enums;

    public interface IDeliveryAccessWindowUpdated : IAggregateEvent
    {
        public Guid Id { get; }
        public AccessWindow AccessWindow { get; }
    }
}
