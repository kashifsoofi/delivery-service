namespace DeliveryService.Infrastructure.Queries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Responses;

    public interface IGetAllDeliveriesQuery
    {
        Task<IEnumerable<Delivery>> ExecuteAsync();
    }
}
