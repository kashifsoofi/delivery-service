namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;
    
    public class AccessWindow
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}