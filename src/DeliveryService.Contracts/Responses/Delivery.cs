namespace DeliveryService.Contracts.Responses
{
    using System;
    using DeliveryService.Contracts.Enums;
    using DeliveryService.Contracts.Messages;

    public class Delivery
    {
        public Guid Id { get; set; }
        public DeliveryState State { get; set; }
        public AccessWindow AccessWindow { get; set; }
        public Recipient Recipient { get; set; }
        public Order Order { get; set; }
    }
}
