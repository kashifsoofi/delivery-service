namespace DeliveryService.Infrastructure.Messages.Responses
{
    using System;

    public class ConfirmationResponse
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}