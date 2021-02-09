namespace DeliveryService.Infrastructure.Tests.Integration.Testcontainers
{
    using DotNet.Testcontainers.Containers.Configurations;
    using DotNet.Testcontainers.Containers.Modules.Abstractions;

    public class MySql57Testcontainer : TestcontainerDatabase
    {
        internal MySql57Testcontainer(ITestcontainersConfiguration configuration) : base(configuration)
        {
        }

        public override string ConnectionString => $"Server={this.Hostname};Port={this.Port};Database={this.Database};Uid={this.Username};Pwd={this.Password};";
    }
}