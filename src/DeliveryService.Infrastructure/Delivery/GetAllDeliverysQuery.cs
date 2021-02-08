namespace DeliveryService.Infrastructure.Delivery
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using MySqlConnector;
    using DeliveryService.Infrastructure.Database;
    using DeliveryService.Contracts.Responses;
    using Dapper;

    public class GetAllDeliverysQuery : IGetAllDeliverysQuery
    {
        private readonly IConnectionStringProvider connectionStringProvider;

        public GetAllDeliverysQuery(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        public async Task<List<Delivery>> ExecuteAsync()
        {
            var sql = "SELECT Id, CreatedOn, UpdatedOn FROM Delivery ORDER BY CreatedOn DESC";

            using (var connection = new MySqlConnection(this.connectionStringProvider.DeliveryServiceConnectionString))
            {
                var query = await connection.QueryAsync<Delivery>(sql);
                return query.ToList();
            }
        }
    }
}
