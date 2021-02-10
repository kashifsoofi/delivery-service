namespace DeliveryService.Host.Handlers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Domain.Aggregates.Delivery;
    using NServiceBus;

    public class DeleteDeliveryCommandHandler : IHandleMessages<DeleteDelivery>
    {
        private readonly IDeliveryAggregateRepository deliveryAggregateRepository;

        public DeleteDeliveryCommandHandler(IDeliveryAggregateRepository deliveryAggregateRepository)
        {
            this.deliveryAggregateRepository = deliveryAggregateRepository;
        }

        public async Task Handle(DeleteDelivery command, IMessageHandlerContext context)
        {
            if (command == null || command.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(command));
            }

            try
            {
                var aggregate = await this.deliveryAggregateRepository.GetByIdAsync(command.Id);
                aggregate.Delete();

                await PersistAndPublishAsync(aggregate, context);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in DeleteDeliveryCommandHandler: {e}");
                throw;
            }
        }

        private async Task PersistAndPublishAsync(IDeliveryAggregate aggregate, IMessageHandlerContext context)
        {
            await deliveryAggregateRepository.DeleteAsync(aggregate);

            await Task.WhenAll(aggregate.UncommittedEvents.Select(async (x) => await context.Publish(x)));
        }
    }
}