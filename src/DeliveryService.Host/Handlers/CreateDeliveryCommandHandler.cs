namespace DeliveryService.Host.Handlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Domain.Aggregates.Delivery;
    using NServiceBus;

    public class CreateDeliveryCommandHandler : IHandleMessages<CreateDelivery>
    {
        private readonly IDeliveryAggregateRepository deliveryAggregateRepository;

        public CreateDeliveryCommandHandler(IDeliveryAggregateRepository deliveryAggregateRepository)
        {
            this.deliveryAggregateRepository = deliveryAggregateRepository;
        }

        public async Task Handle(CreateDelivery command, IMessageHandlerContext context)
        {
            if (command == null || command.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(command));
            }

            try
            {
                var aggregate = await this.deliveryAggregateRepository.GetByIdAsync(command.Id);
                aggregate.Create(command);

                await PersistAndPublishAsync(aggregate, context);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in CreateDeliveryCommandHandler: {e}");
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