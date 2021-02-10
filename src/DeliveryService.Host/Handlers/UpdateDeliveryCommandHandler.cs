namespace DeliveryService.Host.Handlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Domain.Aggregates.Delivery;
    using DeliveryService.Infrastructure.Messages.Responses;
    using NServiceBus;

    public class UpdateDeliveryStateCommandHandler : IHandleMessages<UpdateDeliveryState>
    {
        private readonly IDeliveryAggregateRepository deliveryAggregateRepository;

        public UpdateDeliveryStateCommandHandler(IDeliveryAggregateRepository deliveryAggregateRepository)
        {
            this.deliveryAggregateRepository = deliveryAggregateRepository;
        }

        public async Task Handle(UpdateDeliveryState command, IMessageHandlerContext context)
        {
            if (command == null || command.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(command));
            }

            try
            {
                var aggregate = await this.deliveryAggregateRepository.GetByIdAsync(command.Id);
                aggregate.UpdateDeliveryState(command);

                await PersistAndPublishAsync(aggregate, context);

                await context.Reply(new ConfirmationResponse { Success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in UpdateDeliveryStateCommandHandler: {e}");
                throw;
            }
        }

        private async Task PersistAndPublishAsync(IDeliveryAggregate aggregate, IMessageHandlerContext context)
        {
            if (aggregate.IsNew)
            {
                await deliveryAggregateRepository.CreateAsync(aggregate);
            }
            else
            {
                await deliveryAggregateRepository.UpdateAsync(aggregate);
            }

            await Task.WhenAll(aggregate.UncommittedEvents.Select(async (x) => await context.Publish(x)));
        }
    }
}