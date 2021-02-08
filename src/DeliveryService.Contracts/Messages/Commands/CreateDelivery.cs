namespace DeliveryService.Contracts.Messages.Commands
{
    using System;

    public class CreateDelivery
    {
        public int Id { get; }
        public DateTime CreatedOn { get; }
        public DateTime UpdatedOn { get; }

        public CreateDelivery(int id)
        {
            Id = id;
        }
    }
}
