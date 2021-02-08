namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;

    public class DeliveryAggregateFactory : IDeliveryAggregateFactory
    {
        public DeliveryAggregate Create(Guid id)
        {
            return new DeliveryAggregate(id);
        }

        public DeliveryAggregate Create(DeliveryAggregateState state)
        {
            return new DeliveryAggregate(state);
        }
    }
}
