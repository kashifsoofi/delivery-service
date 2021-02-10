namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;

    public class DeliveryAggregateFactory : IDeliveryAggregateFactory
    {
        public DeliveryAggregate Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be null", nameof(id));
            }

            return new DeliveryAggregate(id);
        }

        public DeliveryAggregate Create(DeliveryAggregateState state)
        {
            if (state == null)
            {
                throw new ArgumentException("State cannot be null", nameof(state));
            }

            return new DeliveryAggregate(state);
        }
    }
}
