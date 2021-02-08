namespace DeliveryService.Infrastructure.Database
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly DatabaseOptions databaseOptions;

        public ConnectionStringProvider(DatabaseOptions databaseOptions)
        {
            this.databaseOptions = databaseOptions;
        }

        public string DeliveryServiceConnectionString
        {
            get
            {
                var port = databaseOptions.Port ?? 3306;
                var database = string.IsNullOrEmpty(databaseOptions.Database) ? "DeliveryService" : databaseOptions.Database;
                return $"Server={databaseOptions.Server};Port={port};Database={database};Uid={databaseOptions.Username};Pwd={databaseOptions.Password};";
            }
        }
    }
}
