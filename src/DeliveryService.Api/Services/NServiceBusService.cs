namespace DeliveryService.Api.Services
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Messages.Commands;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;

    public class NServiceBusService : IHostedService
    {
        private IEndpointInstance endpointInstance;

        public IMessageSession MessageSession { get; internal set; }
        public ExceptionDispatchInfo StartupException { get; internal set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var endpointConfiguration = ConfigureEndpoint();

            try
            {
                endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
                MessageSession = endpointInstance;
            }
            catch (Exception e)
            {
                StartupException = ExceptionDispatchInfo.Capture(e);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (endpointInstance != null)
            {
                await endpointInstance.Stop().ConfigureAwait(false);
            }
        }

        private EndpointConfiguration ConfigureEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("DeliveryService.Api");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "DeliveryService.Contracts.Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "DeliveryService.Contracts.Messages.Events");
            conventions.DefiningMessagesAs(type => type.Namespace == "DeliveryService.Infrastructure.Messages.Responses");

            endpointConfiguration.UsePersistence<LearningPersistence>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(CreateDelivery), "DeliveryService.Host");
            routing.RouteToEndpoint(typeof(UpdateDeliveryState), "DeliveryService.Host");
            routing.RouteToEndpoint(typeof(DeleteDelivery), "DeliveryService.Host");

            endpointConfiguration.MakeInstanceUniquelyAddressable("api");

            return endpointConfiguration;
        }
    }
}