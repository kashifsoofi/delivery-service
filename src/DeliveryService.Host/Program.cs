using System.IO;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DeliveryService.Host
{
    using Autofac.Extensions.DependencyInjection;
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((hostBuilderContext, containerBuilder) =>
                {
                    var startup = new Startup(hostBuilderContext.Configuration);
                    startup.ConfigureContainer(containerBuilder);
                })
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("DeliveryService.Host");

                    var conventions = endpointConfiguration.Conventions();
                    conventions.DefiningCommandsAs(type => type.Namespace == "DeliveryService.Contracts.Messages.Commands");
                    conventions.DefiningEventsAs(type => type.Namespace == "DeliveryService.Contracts.Messages.Events");

                    endpointConfiguration.UsePersistence<InMemoryPersistence>();
                    endpointConfiguration.UseTransport<LearningTransport>();

                    return endpointConfiguration;
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}
