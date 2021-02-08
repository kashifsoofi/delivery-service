namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;
    using DeliveryService.Contracts.Enums;

    public class DeliveryAggregateState : IDeliveryAggregateState
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DeliveryState State { get; set; }
        public AccessWindow AccessWindow { get; set; }
    }
}
