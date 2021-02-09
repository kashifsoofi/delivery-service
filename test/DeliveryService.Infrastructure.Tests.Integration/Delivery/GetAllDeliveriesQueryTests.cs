namespace DeliveryService.Infrastructure.Tests.Integration.Delivery
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using DeliveryService.Infrastructure.Queries;
    using DeliveryService.Infrastructure.Tests.Integration.DockerClient;
    using Xunit;

    [Collection("Database collection")]
    public class GetAllDeliveriesQueryTests
    {
        private readonly GetAllDeliveriesQuery sut;

        public GetAllDeliveriesQueryTests(DockerFixture fixture)
        {
            this.sut = new GetAllDeliveriesQuery(fixture.ConnectionStringProvider);
        }

        [Fact]
        public async Task ExecuteAsync_GivenNoRecords_ShouldReturnEmptyCollection()
        {
            // A
            var deliveriesList = await this.sut.ExecuteAsync();

            // Assert
            deliveriesList.Should().BeEmpty();
        }
    }
}
