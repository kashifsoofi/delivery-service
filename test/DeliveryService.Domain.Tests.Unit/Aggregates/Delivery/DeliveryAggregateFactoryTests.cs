namespace DeliveryService.Domain.Tests.Unit.Aggregates.Delivery
{
    using System;
    using AutoFixture.Xunit2;
    using DeliveryService.Domain.Aggregates.Delivery;
    using FluentAssertions;
    using Xunit;

    public class DeliveryAggregateFactoryTests
    {
        private readonly DeliveryAggregateFactory sut;

        public DeliveryAggregateFactoryTests()
        {
            sut = new DeliveryAggregateFactory();
        }

        [Fact]
        public void Create_GivenDefaultGuid_ShouldThrowException()
        {
            // act & assert
            var exception = Assert.Throws<ArgumentException>(() => sut.Create(Guid.Empty));
            exception.ParamName.Should().Be("id");
        }

        [Theory]
        [AutoData]
        public void Create_GivenValidGuid_ShouldReturnAggregate(Guid id)
        {
            var expectedState = new DeliveryAggregateState() { Id = id };

            // act
            var aggregate = this.sut.Create(id);

            // assert
            aggregate.IsNew.Should().BeTrue();
            aggregate.State.Should().BeEquivalentTo(expectedState);
        }

        [Fact]
        public void Create_GivenNullState_ShouldThrowException()
        {
            // act & assert
            var exception = Assert.Throws<ArgumentException>(() => sut.Create(null));
            exception.ParamName.Should().Be("state");
        }

        [Theory]
        [AutoData]
        public void Create_GivenValidState_ShouldReturnAggregate(DeliveryAggregateState state)
        {
            // act
            var aggregate = this.sut.Create(state);

            // assert
            aggregate.IsNew.Should().BeFalse();
            aggregate.State.Should().BeEquivalentTo(state);
        }
    }
}
