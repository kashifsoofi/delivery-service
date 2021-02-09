namespace DeliveryService.Contracts.Messages.Events
{
    using System;
    using DeliveryService.Contracts.Enums;

    public class DeliveryAccessWindowUpdated : AggregateEvent, IDeliveryAccessWindowUpdated
    {
        public Guid Id { get; }
        public AccessWindow AccessWindow { get; }

        public DeliveryAccessWindowUpdated(Guid id, AccessWindow accessWindow)
        {
            this.Id = id;
            this.AccessWindow = accessWindow;
        }
    }
}
