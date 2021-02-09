namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;
    using System.Collections.Generic;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Contracts.Messages.Events;

    public interface IDeliveryAggregate
    {
        Guid Id { get; }

        IDeliveryAggregateState State { get; }

        bool IsNew { get; }

        void Create(CreateDelivery command);

        List<IAggregateEvent> UncommittedEvents { get; }
    }
}