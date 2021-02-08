namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;
    using DeliveryService.Contracts.Enums;

    public interface IDeliveryAggregateState
    {
        Guid Id { get; }
        DateTime CreatedOn { get; }
        DateTime UpdatedOn { get; }
        DeliveryState State { get; }
        AccessWindow AccessWindow { get; }
    }
}
