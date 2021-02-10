namespace DeliveryService.Host
{
    using System.IO;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Messages.Commands;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;

    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    var startup = new Startup(hostBuilderContext.Configuration);
                    startup.ConfigureServices(services);
                })
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("DeliveryService.Host");

                    var conventions = endpointConfiguration.Conventions();
                    conventions.DefiningCommandsAs(type => type.Namespace == "DeliveryService.Contracts.Messages.Commands");
                    conventions.DefiningEventsAs(type => type.Namespace == "DeliveryService.Contracts.Messages.Events");
                    conventions.DefiningMessagesAs(type => type.Namespace == "DeliveryService.Infrastructure.Messages.Responses");

                    endpointConfiguration.UsePersistence<LearningPersistence>();
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();

                    var routing = transport.Routing();
                    routing.RouteToEndpoint(typeof(UpdateDeliveryState), "DeliveryService.Host");

                    endpointConfiguration.Recoverability()
                        .Delayed(x => x.NumberOfRetries(0))
                        .Immediate(x => x.NumberOfRetries(0));

                    return endpointConfiguration;
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}
