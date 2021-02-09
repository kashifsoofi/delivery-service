namespace DeliveryService.Contracts.Messages.Events
{
    using System;
    using DeliveryService.Contracts.Enums;

    public interface IDeliveryStateUpdated : IAggregateEvent
    {
        public Guid Id { get; }
        public DeliveryState State { get; }
    }
}
