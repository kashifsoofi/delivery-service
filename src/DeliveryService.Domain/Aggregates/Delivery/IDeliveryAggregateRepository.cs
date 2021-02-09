namespace DeliveryService.Domain.Aggregates.Delivery
{
    using System;
    using System.Threading.Tasks;

    public interface IDeliveryAggregateRepository
    {
        Task<IDeliveryAggregate> GetByIdAsync(Guid id);

        Task CreateAsync(IDeliveryAggregate aggregate);

        Task UpdateAsync(IDeliveryAggregate aggregate);

        Task DeleteAsync(IDeliveryAggregate aggregate);
    }
}
