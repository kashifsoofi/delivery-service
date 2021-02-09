namespace DeliveryService.Api.Tests.Unit.Controllers
{
    using System;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using DeliveryService.Api.Controllers;
    using DeliveryService.Contracts.Responses;
    using DeliveryService.Infrastructure.Queries;
    using Moq;
    using NServiceBus.Testing;

    public class DeliveryControllerTests
    {
        private TestableMessageSession testableSession;
        private Mock<IGetAllDeliveriesQuery> getAllDeliveriesQueryMock;
        private Mock<IGetDeliveryByIdQuery> getDeliveryByIdQueryMock;

        private readonly DeliveryController sut;

        public DeliveryControllerTests()
        {
            getAllDeliveriesQueryMock = new Mock<IGetAllDeliveriesQuery>();
            getDeliveryByIdQueryMock = new Mock<IGetDeliveryByIdQuery>();

            sut = new DeliveryController(testableSession, getAllDeliveriesQueryMock.Object, getDeliveryByIdQueryMock.Object);
        }

        [Fact]
        public void Get_should_return_ok_with_Delivery()
        {
            var id = Guid.NewGuid();
            var result = sut.Get(id);

            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var delivery = okResult.Value.Should().BeAssignableTo<Delivery>().Subject;

            delivery.Id.Should().Be(id);
        }
    }
}
