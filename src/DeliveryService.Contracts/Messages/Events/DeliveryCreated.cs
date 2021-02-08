namespace DeliveryService.Contracts.Messages.Events
{
    using System;

    public class DeliveryCreated : IDeliveryCreated
    {
        public Guid Id { get; }

        public DateTime CreatedOn { get; }

        public DeliveryCreated(Guid id, DateTime createdOn)
        {
            this.Id = id;
            this.CreatedOn = createdOn;
        }
    }
}
