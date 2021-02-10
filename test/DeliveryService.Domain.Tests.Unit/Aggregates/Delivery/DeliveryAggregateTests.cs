namespace DeliveryService.Domain.Tests.Unit.Aggregates.Delivery
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture.Xunit2;
    using DeliveryService.Contracts.Enums;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Contracts.Messages.Events;
    using DeliveryService.Domain.Aggregates.Delivery;
    using FluentAssertions;
    using Xunit;

    public class DeliveryAggregateTests
    {
        [Theory]
        [AutoData]
        public void Create_GivenNullCommand_ShouldThrowException(Guid id)
        {
            var sut = new DeliveryAggregate(id);

            // act & assert
            var exception = Assert.Throws<ArgumentException>(() => sut.Create(null));
            exception.ParamName.Should().Be("command");
        }

        [Theory]
        [AutoData]
        public void Create_GivenStateIsNew_ShouldCreateEvent(CreateDelivery command)
        {
            var aggregate = new DeliveryAggregate(command.Id);

            // act
            aggregate.Create(command);

            // assert
            aggregate.UncommittedEvents.Count.Should().Be(1);

            var evt = aggregate.UncommittedEvents.First(e => e.GetType() == typeof(DeliveryCreated)) as DeliveryCreated;
            evt.Should().NotBeNull();
        }

        [Theory]
        [AutoData]
        public void UpdateDeliveryState_GivenStateIsNotCreated_AndSettingApproved_ShouldThrowException(DeliveryAggregateState state, UpdateDeliveryState command)
        {
            // arrange
            state.Id = command.Id;
            state.State = DeliveryState.Completed;
            var aggregate = new DeliveryAggregate(state);

            command.State = DeliveryState.Approved;

            // act & assert
            var exception = Assert.Throws<Exception>(() => aggregate.UpdateDeliveryState(command));
        }

        [Theory]
        [AutoData]
        public void UpdateDeliveryState_GivenStateIsNotApproved_AndSettingCompleted_ShouldThrowException(DeliveryAggregateState state, UpdateDeliveryState command)
        {
            // arrange
            state.Id = command.Id;
            state.State = DeliveryState.Created;
            var aggregate = new DeliveryAggregate(state);

            command.State = DeliveryState.Completed;

            // act & assert
            var exception = Assert.Throws<Exception>(() => aggregate.UpdateDeliveryState(command));
        }

        [Theory]
        [AutoData]
        public void UpdateDeliveryState_GivenStateIsNotCreated_AndSettingCancelled_ShouldThrowException(DeliveryAggregateState state, UpdateDeliveryState command)
        {
            // arrange
            state.Id = command.Id;
            state.State = DeliveryState.Expired;
            var aggregate = new DeliveryAggregate(state);

            command.State = DeliveryState.Cancelled;

            // act & assert
            var exception = Assert.Throws<Exception>(() => aggregate.UpdateDeliveryState(command));
        }

        [Theory]
        [AutoData]
        public void UpdateDeliveryState_GivenStateIsNotApproved_AndSettingCancelled_ShouldThrowException(DeliveryAggregateState state, UpdateDeliveryState command)
        {
            // arrange
            state.Id = command.Id;
            state.State = DeliveryState.Expired;
            var aggregate = new DeliveryAggregate(state);

            command.State = DeliveryState.Cancelled;

            // act & assert
            var exception = Assert.Throws<Exception>(() => aggregate.UpdateDeliveryState(command));
        }

        [Theory]
        [AutoData]
        public void UpdateDeliveryState_GivenStateIsNotCompleted_AndSettingExpired_ShouldThrowException(DeliveryAggregateState state, UpdateDeliveryState command)
        {
            // arrange
            state.Id = command.Id;
            state.State = DeliveryState.Completed;
            var aggregate = new DeliveryAggregate(state);

            command.State = DeliveryState.Expired;

            // act & assert
            var exception = Assert.Throws<Exception>(() => aggregate.UpdateDeliveryState(command));
        }

        [Theory]
        [AutoData]
        public void UpdateDeliveryState_GivenEndTimeIsInFuture_AndSettingExpired_ShouldThrowException(DeliveryAggregateState state, UpdateDeliveryState command)
        {
            // arrange
            state.Id = command.Id;
            state.State = DeliveryState.Approved;
            state.AccessWindow.EndTime = DateTime.UtcNow.AddHours(1);
            var aggregate = new DeliveryAggregate(state);

            command.State = DeliveryState.Expired;

            // act & assert
            var exception = Assert.Throws<Exception>(() => aggregate.UpdateDeliveryState(command));
        }

        [Theory]
        [AutoData]
        public void UpdateDeliveryState_GivenStateIsCreated_SettingApproved_ShouldGenerateEvent(DeliveryAggregateState state, UpdateDeliveryState command)
        {
            // arrange
            state.Id = command.Id;
            state.State = DeliveryState.Created;
            var aggregate = new DeliveryAggregate(state);

            command.State = DeliveryState.Approved;

            // act
            aggregate.UpdateDeliveryState(command);

            // assert
            aggregate.UncommittedEvents.Count.Should().Be(1);

            var evt = aggregate.UncommittedEvents.First(e => e.GetType() == typeof(DeliveryStateUpdated)) as DeliveryStateUpdated;
            evt.Should().NotBeNull();
        }
    }
}