namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;

    public interface IDeliveryAggregateFactory
    {
        DeliveryAggregate Create(Guid id);

        DeliveryAggregate Create(DeliveryAggregateState state);
    }
}
