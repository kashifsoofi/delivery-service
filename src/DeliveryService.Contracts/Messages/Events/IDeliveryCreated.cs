namespace DeliveryService.Contracts.Messages.Events
{
    using System;

    public interface IDeliveryCreated
    {
        Guid Id { get; }
        DateTime CreatedOn { get; }
    }
}
