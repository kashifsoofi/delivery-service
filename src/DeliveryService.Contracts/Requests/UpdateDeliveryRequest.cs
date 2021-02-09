namespace DeliveryService.Contracts.Requests
{
    using DeliveryService.Contracts.Enums;
    using DeliveryService.Contracts.Messages;

    public class UpdateDeliveryRequest
    {
        public DeliveryState State { get; set; }
        public AccessWindow AccessWindow { get; set; }
        public Recipient Recipient { get; set; }
        public Order Order { get; set; }
    }
}
