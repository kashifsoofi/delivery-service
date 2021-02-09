namespace DeliveryService.Contracts.Messages.Events
{
    using System;
    using DeliveryService.Contracts.Enums;

    public interface IDeliveryCreated : IAggregateEvent
    {
        public Guid Id { get; }
        public DeliveryState State { get; }
        public AccessWindow AccessWindow { get; }
        public Recipient Recipient { get; }
        public Order Order { get; }
    }
}
