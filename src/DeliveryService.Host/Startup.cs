namespace DeliveryService.Host
{
    using Dapper;
    using DeliveryService.Contracts.Messages;
    using Microsoft.Extensions.DependencyInjection;
    using DeliveryService.Domain.Aggregates.Delivery;
    using DeliveryService.Infrastructure.AggregateRepositories.Delivery;
    using DeliveryService.Infrastructure.Database;
    using Microsoft.Extensions.Configuration;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(typeof(AccessWindow), new JsonTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Recipient), new JsonTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Order), new JsonTypeHandler());

            var databaseOptions = Configuration.GetSection("Database").Get<DatabaseOptions>();
            services.AddSingleton<IDatabaseOptions>(databaseOptions);
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();

            services.AddSingleton<IDeliveryAggregateFactory, DeliveryAggregateFactory>();
            services.AddSingleton<IDeliveryAggregateRepository, DeliveryRepository>();
        }
    }
}
