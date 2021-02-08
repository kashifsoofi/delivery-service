namespace DeliveryService.Api.Tests.Unit.Controllers
{
    using System;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using DeliveryService.Api.Controllers;
    using DeliveryService.Contracts.Responses;

    public class DeliveryControllerTests
    {
        private readonly DeliveryController _sut;

        public DeliveryControllerTests()
        {
            _sut = new DeliveryController();
        }

        [Fact]
        public void Get_should_return_ok_with_Delivery()
        {
            var id = Guid.NewGuid();
            var result = _sut.Get(id);

            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var aggregatename = okResult.Value.Should().BeAssignableTo<Delivery>().Subject;

            aggregatename.Id.Should().Be(id);
        }
    }
}
