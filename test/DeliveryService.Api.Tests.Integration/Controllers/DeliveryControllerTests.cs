using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using DeliveryService.Contracts.Responses;

namespace DeliveryService.Api.Tests.Integration
{
    public class DeliveryControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private WebApplicationFactory<Startup> _factory;

        public DeliveryControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_should_return_ok_with_Delivery()
        {
            // Arrange
            var id = Guid.NewGuid();
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/aggregatename/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var aggregatename = JsonConvert.DeserializeObject<Delivery>(responseContent);
            aggregatename.Id.Should().Be(id);
        }
    }
}
