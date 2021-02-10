namespace DeliveryService.Host.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DeliveryService.Contracts.Enums;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Infrastructure.Queries;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;

    public class ExpireDeliveriesService : BackgroundService
    {
        private readonly IGetExpiredDeliveryIdsQuery getExpiredDeliveryIdsQuery;
        private readonly IMessageSession messageSession;

        public ExpireDeliveriesService(IGetExpiredDeliveryIdsQuery getExpiredDeliveryIdsQuery, IMessageSession messageSession)
        {
            this.getExpiredDeliveryIdsQuery = getExpiredDeliveryIdsQuery;
            this.messageSession = messageSession;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var expiredDeliveryIds = await getExpiredDeliveryIdsQuery.ExecuteAsync();

                if (expiredDeliveryIds.Any())
                {
                    await Task.WhenAll(expiredDeliveryIds.Select(async (id) =>
                        await messageSession.Send(new UpdateDeliveryState(id, DeliveryState.Expired))));
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}