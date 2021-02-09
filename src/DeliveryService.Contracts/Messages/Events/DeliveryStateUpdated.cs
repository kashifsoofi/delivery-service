namespace DeliveryService.Contracts.Messages.Events
{
    using System;
    using DeliveryService.Contracts.Enums;

    public class DeliveryStateUpdated : AggregateEvent, IDeliveryStateUpdated
    {
        public Guid Id { get; }
        public DeliveryState State { get; }

        public DeliveryStateUpdated(Guid id, DeliveryState state)
        {
            this.Id = id;
            this.State = state;
        }
    }
}
