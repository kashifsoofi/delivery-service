namespace DeliveryService.Infrastructure.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using DeliveryService.Infrastructure.Database;
    using MySqlConnector;

    public class GetExpiredDeliveryIdsQuery : IGetExpiredDeliveryIdsQuery
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly SqlHelper<GetAllDeliveriesQuery> sqlHelper;

        public GetExpiredDeliveryIdsQuery(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.sqlHelper = new SqlHelper<GetAllDeliveriesQuery>();
        }

        public async Task<IEnumerable<Guid>> ExecuteAsync()
        {
            await using var connection = new MySqlConnection(this.connectionStringProvider.DeliveryServiceConnectionString);
            return await connection.QueryAsync<Guid>(
                this.sqlHelper.GetSqlFromEmbeddedResource("GetExpiredDeliveryIds"),
                commandType: CommandType.Text);
        }
    }
}
