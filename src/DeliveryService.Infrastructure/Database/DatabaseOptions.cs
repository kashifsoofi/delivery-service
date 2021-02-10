namespace DeliveryService.Infrastructure.Database
{
    public interface IDatabaseOptions
    {
        string Server { get; set; }
        int? Port { get; set; }
        string Database { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }

    public class DatabaseOptions : IDatabaseOptions
    {
        public string Server { get; set; }
        public int? Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
