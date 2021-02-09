namespace DeliveryService.Host
{
    using System.Collections.Generic;
    using Autofac;
    using Dapper;
    using DeliveryService.Contracts.Messages;
    using Microsoft.Extensions.DependencyInjection;
    using DeliveryService.Domain.Aggregates.Delivery;
    using DeliveryService.Infrastructure.AggregateRepositories.Delivery;
    using DeliveryService.Infrastructure.Database;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            SqlMapper.AddTypeHandler(typeof(AccessWindow), new JsonTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Recipient), new JsonTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Order), new JsonTypeHandler());

            builder.Register(ctx => Configuration.GetSection("Database").Get<DatabaseOptions>()).SingleInstance();
            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>().SingleInstance();

            builder.RegisterType<DeliveryAggregateFactory>().As<IDeliveryAggregateFactory>().SingleInstance();
            builder.RegisterType<DeliveryRepository>().As<IDeliveryAggregateRepository>().SingleInstance();
        }
    }
}
