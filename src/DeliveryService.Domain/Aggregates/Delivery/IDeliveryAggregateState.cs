namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;

    public interface IDeliveryAggregateState
    {
        Guid Id { get; }
        DateTime CreatedOn { get; }
        DateTime UpdatedOn { get; }
    }
}
