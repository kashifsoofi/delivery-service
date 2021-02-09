namespace DeliveryService.Infrastructure.AggregateRepositories.Delivery
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using DeliveryService.Domain.Aggregates.Delivery;
    using DeliveryService.Infrastructure.Database;
    using MySqlConnector;

    public class DeliveryRepository : IDeliveryAggregateRepository
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly IDeliveryAggregateFactory aggregateFactory;
        private readonly SqlHelper<DeliveryRepository> sqlHelper;

        public DeliveryRepository(IConnectionStringProvider connectionStringProvider, IDeliveryAggregateFactory aggregateFactory)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.aggregateFactory = aggregateFactory;
            this.sqlHelper = new SqlHelper<DeliveryRepository>();

        }

        public async Task<IDeliveryAggregate> GetByIdAsync(Guid id)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.DeliveryServiceConnectionString);
            {
                var state = await connection.QueryFirstOrDefaultAsync<DeliveryAggregateState>(
                    this.sqlHelper.GetSqlFromEmbeddedResource("GetDeliveryById"),
                    new { Id = id },
                    commandType: CommandType.Text);

                return state == null ? aggregateFactory.Create(id) : aggregateFactory.Create(state);
            }
        }

        public async Task CreateAsync(IDeliveryAggregate aggregate)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.DeliveryServiceConnectionString);
            {
                var parameters = new
                {
                    aggregate.Id,
                    aggregate.State.CreatedOn,
                    aggregate.State.UpdatedOn,
                    State = aggregate.State.State.ToString(),
                    aggregate.State.AccessWindow,
                    aggregate.State.Recipient,
                    aggregate.State.Order,
                };

                await connection.ExecuteAsync(this.sqlHelper.GetSqlFromEmbeddedResource("CreateDelivery"), parameters,
                    commandType: CommandType.Text);
            }
        }

        public async Task UpdateAsync(IDeliveryAggregate aggregate)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.DeliveryServiceConnectionString);
            {
                var parameters = new
                {
                    aggregate.Id,
                    aggregate.State.UpdatedOn,
                    State = aggregate.State.State.ToString(),
                    aggregate.State.AccessWindow,
                    aggregate.State.Recipient,
                    aggregate.State.Order,
                };

                await connection.ExecuteAsync(this.sqlHelper.GetSqlFromEmbeddedResource("UpdateDelivery"), parameters,
                    commandType: CommandType.Text);
            }
        }

        public async Task DeleteAsync(IDeliveryAggregate aggregate)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.DeliveryServiceConnectionString);
            {
                var parameters = new
                {
                    aggregate.Id,
                };

                await connection.ExecuteAsync(this.sqlHelper.GetSqlFromEmbeddedResource("UpdateDelivery"), parameters,
                    commandType: CommandType.Text);
            }
        }
    }
}
