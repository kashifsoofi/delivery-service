namespace DeliveryService.Contracts.Messages.Commands
{
    using System;
    using DeliveryService.Contracts.Enums;

    public class UpdateDeliveryState
    {
        public Guid Id { get; }
        public DeliveryState State { get; set; }

        public UpdateDeliveryState(Guid id, DeliveryState state)
        {
            Id = id;
            this.State = state;
        }
    }
}
