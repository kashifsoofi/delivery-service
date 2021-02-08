namespace DeliveryService.Infrastructure.Tests.Integration.Delivery
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using DeliveryService.Contracts.Responses;
    using DeliveryService.Infrastructure.Delivery;
    using DeliveryService.Infrastructure.Tests.Integration.DockerClient;
    using DeliveryService.Infrastructure.Tests.Integration.Testcontainers;
    using Xunit;

    [Collection("Database collection")]
    public class GetAllDeliverysQueryTests
    {
        private readonly GetAllDeliverysQuery sut;

        public GetAllDeliverysQueryTests(DockerFixture fixture)
        {
            this.sut = new GetAllDeliverysQuery(fixture.ConnectionStringProvider);
        }

        [Fact]
        public async Task ExecuteAsync_GivenNoRecords_ShouldReturnEmptyCollection()
        {
            // A
            var aggregatenameList = await this.sut.ExecuteAsync();

            // Assert
            aggregatenameList.Should().BeEmpty();
        }
    }
}
