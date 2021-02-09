namespace DeliveryService.Infrastructure.Queries
{
    using System;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Responses;

    public interface IGetDeliveryByIdQuery
    {
        Task<Delivery> ExecuteAsync(Guid id);
    }
}