namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;
    using System.Collections.Generic;
    using DeliveryService.Contracts.Enums;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Contracts.Messages.Events;

    public class DeliveryAggregate : IDeliveryAggregate
    {
        private readonly DeliveryAggregateState state;
        private readonly bool isNew;

        public DeliveryAggregate(Guid id)
        {
            this.state = new DeliveryAggregateState { Id = id };
            this.isNew = true;
        }

        public DeliveryAggregate(DeliveryAggregateState state)
        {
            this.state = state ?? throw new ArgumentNullException(nameof(state));
            this.isNew = false;
        }

        public Guid Id => this.state.Id;

        public IDeliveryAggregateState State => this.state;

        public bool IsNew => this.isNew;

        public List<IAggregateEvent> UncommittedEvents { get; } = new List<IAggregateEvent>();

        public void Create(CreateDelivery command)
        {
            if (command == null)
            {
                throw new ArgumentException("Command cannot be null", nameof(command));
            }

            if (!this.isNew)
            {
                throw new Exception("Delivery already exists.");
            }

            var evt = new DeliveryCreated(
                command.Id,
                command.State ?? DeliveryState.Created,
                command.AccessWindow,
                command.Recipient,
                command.Order);
            Apply(evt, Handle);
        }

        public void UpdateDeliveryState(UpdateDeliveryState command)
        {
            if (command == null)
            {
                throw new ArgumentException("Command cannot be null", nameof(command));
            }

            if (this.isNew)
            {
                throw new Exception("Delivery does not exist.");
            }

            switch (command.State)
            {
                case DeliveryState.Approved:
                    if (state.State != DeliveryState.Created)
                    {
                        throw new Exception("Only Delivery in Created state can be Approved.");
                    }
                    break;
                case DeliveryState.Completed:
                    if (state.State != DeliveryState.Approved)
                    {
                        throw new Exception("Only Delivery in Approved state can be Completed.");
                    }
                    break;
                case DeliveryState.Cancelled:
                    if (state.State != DeliveryState.Created || state.State != DeliveryState.Approved)
                    {
                        throw new Exception("Only Delivery in Pending (Created, Approved) state can be Cancelled.");
                    }
                    break;
                case DeliveryState.Expired:
                    if (state.State == DeliveryState.Completed || state.AccessWindow.EndTime >= DateTime.UtcNow)
                    {
                        throw new Exception("Only Delivery not in Completed state after access window time can be Expired.");
                    }
                    break;
            }

            var evt = new DeliveryStateUpdated(Id, command.State);
            Apply(evt, Handle);
        }

        public void Delete()
        {
            if (this.isNew)
            {
                throw new Exception("Delivery does not exist.");
            }

            var evt = new DeliveryDeleted(
                state.Id,
                state.State,
                state.AccessWindow,
                state.Recipient,
                state.Order);
            Apply(evt, Handle);
        }

        private void Apply<T>(T evt, Action<T> handler) where T : IAggregateEvent
        {
            handler(evt);
            this.UncommittedEvents.Add(evt);
        }

        private void Handle(DeliveryCreated evt)
        {
            this.state.CreatedOn = evt.Timestamp;
            this.state.UpdatedOn = evt.Timestamp;
            this.state.State = evt.State;
            this.state.AccessWindow = evt.AccessWindow;
            this.state.Recipient = evt.Recipient;
            this.state.Order = evt.Order;
        }

        private void Handle(DeliveryStateUpdated evt)
        {
            this.state.UpdatedOn = evt.Timestamp;
            this.state.State = evt.State;
        }

        private void Handle(DeliveryDeleted evt)
        {
        }
    }
}
