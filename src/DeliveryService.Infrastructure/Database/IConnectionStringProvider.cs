using System;
namespace DeliveryService.Infrastructure.Database
{
    public interface IConnectionStringProvider
    {
        string DeliveryServiceConnectionString { get; }
    }
}
