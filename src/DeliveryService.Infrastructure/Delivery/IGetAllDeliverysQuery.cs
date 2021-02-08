namespace DeliveryService.Infrastructure.Delivery
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Responses;

    public interface IGetAllDeliverysQuery
    {
        Task<List<Delivery>> ExecuteAsync();
    }
}
