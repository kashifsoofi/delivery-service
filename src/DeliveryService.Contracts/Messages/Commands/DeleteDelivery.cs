namespace DeliveryService.Contracts.Messages.Commands
{
    using System;

    public class DeleteDelivery
    {
        public Guid Id { get; }

        public DeleteDelivery(Guid id)
        {
            Id = id;
        }
    }
}
