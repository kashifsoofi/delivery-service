namespace DeliveryService.Contracts.Responses
{
    using System;

    public class Delivery
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
