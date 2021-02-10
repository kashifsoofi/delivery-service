namespace DeliveryService.Api.Handlers
{
    using System.Threading.Tasks;
    using DeliveryService.Infrastructure.Messages.Responses;
    using NServiceBus;

    public class ConfirmationResponseMessageHandler : IHandleMessages<ConfirmationResponse>
    {
        public Task Handle(ConfirmationResponse message, IMessageHandlerContext context) => Task.CompletedTask;
    }
}