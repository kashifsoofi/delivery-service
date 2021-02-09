namespace DeliveryService.Infrastructure.Queries
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using DeliveryService.Contracts.Responses;
    using DeliveryService.Infrastructure.Database;
    using MySqlConnector;

    public class GetDeliveryByIdQuery : IGetDeliveryByIdQuery
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly SqlHelper<GetDeliveryByIdQuery> sqlHelper;

        public GetDeliveryByIdQuery(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.sqlHelper = new SqlHelper<GetDeliveryByIdQuery>();
        }

        public async Task<Delivery> ExecuteAsync(Guid id)
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.DeliveryServiceConnectionString);
            return await connection.QueryFirstOrDefaultAsync<Delivery>(
                this.sqlHelper.GetSqlFromEmbeddedResource("GetDeliveryById"),
                new { Id = id },
                commandType: CommandType.Text);
        }
    }
}