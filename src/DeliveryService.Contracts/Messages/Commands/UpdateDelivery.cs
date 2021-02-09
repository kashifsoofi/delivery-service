namespace DeliveryService.Contracts.Messages.Commands
{
    using System;
    using DeliveryService.Contracts.Enums;

    public class UpdateDelivery
    {
        public Guid Id { get; }
        public DeliveryState State { get; set; }
        public AccessWindow AccessWindow { get; }
        public Recipient Recipient { get; }
        public Order Order { get; }

        public UpdateDelivery(Guid id, DeliveryState state, AccessWindow accessWindow, Recipient recipient, Order order)
        {
            Id = id;
            this.State = state;
            this.AccessWindow = accessWindow;
            this.Recipient = recipient;
            this.Order = order;
        }
    }
}
