namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;
    using DeliveryService.Contracts.Messages.Commands;

    public class DeliveryAggregate
    {
        private readonly DeliveryAggregateState state;
        private readonly bool isNew;

        public DeliveryAggregate(Guid id)
        {
            this.state = new DeliveryAggregateState { Id = id };
            this.isNew = true;
        }

        public DeliveryAggregate(DeliveryAggregateState state)
        {
            this.state = state ?? throw new ArgumentNullException(nameof(state));
            this.isNew = false;
        }

        public Guid Id => this.state.Id;

        public IDeliveryAggregateState State => this.state;
    }
}
