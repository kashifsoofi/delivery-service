namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;
    using DeliveryService.Contracts.Enums;
    using DeliveryService.Contracts.Messages;

    public class DeliveryAggregateState : IDeliveryAggregateState
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DeliveryState State { get; set; }
        public AccessWindow AccessWindow { get; set; }
        public Recipient Recipient { get; set; }
        public Order Order { get; set; }
    }
}
