namespace DeliveryService.Infrastructure.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using DeliveryService.Contracts.Responses;
    using DeliveryService.Infrastructure.Database;
    using MySqlConnector;

    public class GetAllDeliveriesQuery : IGetAllDeliveriesQuery
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly SqlHelper<GetAllDeliveriesQuery> sqlHelper;

        public GetAllDeliveriesQuery(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.sqlHelper = new SqlHelper<GetAllDeliveriesQuery>();
        }

        public async Task<IEnumerable<Delivery>> ExecuteAsync()
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.DeliveryServiceConnectionString);
            return await connection.QueryAsync<Delivery>(
                this.sqlHelper.GetSqlFromEmbeddedResource("GetAllDeliveries"),
                commandType: CommandType.Text);
        }
    }
}
