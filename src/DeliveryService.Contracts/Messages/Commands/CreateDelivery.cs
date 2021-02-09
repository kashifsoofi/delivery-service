namespace DeliveryService.Contracts.Messages.Commands
{
    using System;
    using DeliveryService.Contracts.Enums;

    public class CreateDelivery
    {
        public Guid Id { get; set; }
        public DeliveryState? State { get; set; }
        public AccessWindow AccessWindow { get; set; }
        public Recipient Recipient { get; set; }
        public Order Order { get; set; }

        public CreateDelivery(Guid id, DeliveryState? state, AccessWindow accessWindow, Recipient recipient, Order order)
        {
            Id = id;
            this.State = state;
            this.AccessWindow = accessWindow;
            this.Recipient = recipient;
            this.Order = order;
        }
    }
}
