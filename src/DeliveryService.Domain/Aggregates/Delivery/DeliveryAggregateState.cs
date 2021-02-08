namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;

    public class DeliveryAggregateState : IDeliveryAggregateState
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
