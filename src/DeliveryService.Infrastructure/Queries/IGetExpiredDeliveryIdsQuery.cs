namespace DeliveryService.Infrastructure.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGetExpiredDeliveryIdsQuery
    {
        Task<IEnumerable<Guid>> ExecuteAsync();
    }
}
