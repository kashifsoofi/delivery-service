﻿namespace DeliveryService.Contracts.Messages.Events
{
    using System;
    using DeliveryService.Contracts.Enums;

    public class DeliveryCreated : AggregateEvent, IDeliveryCreated
    {
        public Guid Id { get; }
        public DeliveryState State { get; }
        public AccessWindow AccessWindow { get; }
        public Recipient Recipient { get; }
        public Order Order { get; }

        public DeliveryCreated(Guid id, DeliveryState state, AccessWindow accessWindow, Recipient recipient, Order order)
        {
            this.Id = id;
            this.State = state;
            this.AccessWindow = accessWindow;
            this.Recipient = recipient;
            this.Order = order;
        }
    }
}
