namespace DeliveryService.Contracts.Messages.Commands
{
    using System;

    public class DeleteDelivery
    {
        public Guid Id { get; set; }

        public DeleteDelivery(Guid id)
        {
            Id = id;
        }
    }
}
