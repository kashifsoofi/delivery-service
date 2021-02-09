namespace DeliveryService.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using DeliveryService.Contracts.Enums;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Contracts.Requests;
    using DeliveryService.Contracts.Responses;
    using DeliveryService.Infrastructure.Messages.Responses;
    using DeliveryService.Infrastructure.Queries;
    using NServiceBus;

    [Route("[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private IMessageSession messageSession;
        private IGetAllDeliveriesQuery getAllDeliveriesQuery;
        private IGetDeliveryByIdQuery getDeliveryByIdQuery;

        public DeliveryController(IMessageSession messageSession, IGetAllDeliveriesQuery getAllDeliveriesQuery, IGetDeliveryByIdQuery getDeliveryByIdQuery)
        {
            this.messageSession = messageSession;
            this.getAllDeliveriesQuery = getAllDeliveriesQuery;
            this.getDeliveryByIdQuery = getDeliveryByIdQuery;
        }

        [HttpGet]
        public async Task<ActionResult<Delivery>> Get()
        {
            // Enforce security

            var result = await this.getAllDeliveriesQuery.ExecuteAsync();
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> Get(Guid id)
        {
            var result = await getDeliveryByIdQuery.ExecuteAsync(id);

            return result == null ? (ActionResult)this.NotFound() : this.Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Delivery>> Post([FromBody] CreateDeliveryRequest request)
        {
            var createDeliveryCommand =
                new CreateDelivery(request.Id, request.State, request.AccessWindow, request.Recipient, request.Order);
            
            var result = await this.messageSession
                .Request<ConfirmationResponse>(createDeliveryCommand, new SendOptions(), CancellationToken.None)
                .ConfigureAwait(false);
            if (!result.Success)
            {
                throw result.Exception;
            }

            var delivery = await getDeliveryByIdQuery.ExecuteAsync(request.Id);
            return CreatedAtAction("delivery", new { id = request.Id }, delivery);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Delivery>> Put(Guid id, [FromBody] UpdateDeliveryRequest request)
        {
            var updateDeliveryCommand = new UpdateDeliveryState(id, request.State );

            var result = await this.messageSession
                .Request<ConfirmationResponse>(updateDeliveryCommand, new SendOptions(), CancellationToken.None)
                .ConfigureAwait(false);
            if (!result.Success)
            {
                throw result.Exception;
            }

            var delivery = await getDeliveryByIdQuery.ExecuteAsync(id);
            return new OkObjectResult(delivery);
        }

        [HttpPut("{id}:approve")]
        public async Task<ActionResult<Delivery>> Approve(Guid id)
        {
            // Validate User
            var updateDeliveryCommand = new UpdateDeliveryState(id, DeliveryState.Approved );

            var result = await this.messageSession
                .Request<ConfirmationResponse>(updateDeliveryCommand, new SendOptions(), CancellationToken.None)
                .ConfigureAwait(false);
            if (!result.Success)
            {
                throw result.Exception;
            }

            var delivery = await getDeliveryByIdQuery.ExecuteAsync(id);
            return new OkObjectResult(delivery);
        }

        [HttpPut("{id}:complete")]
        public async Task<ActionResult<Delivery>> Complete(Guid id)
        {
            // Validate Partner
            var updateDeliveryCommand = new UpdateDeliveryState(id, DeliveryState.Completed );

            var result = await this.messageSession
                .Request<ConfirmationResponse>(updateDeliveryCommand, new SendOptions(), CancellationToken.None)
                .ConfigureAwait(false);
            if (!result.Success)
            {
                throw result.Exception;
            }

            var delivery = await getDeliveryByIdQuery.ExecuteAsync(id);
            return new OkObjectResult(delivery);
        }

        [HttpPut("{id}:cancel")]
        public async Task<ActionResult<Delivery>> Cancel(Guid id)
        {
            // Validate User or Partner
            var updateDeliveryCommand = new UpdateDeliveryState(id, DeliveryState.Cancelled );

            var result = await this.messageSession
                .Request<ConfirmationResponse>(updateDeliveryCommand, new SendOptions(), CancellationToken.None)
                .ConfigureAwait(false);
            if (!result.Success)
            {
                throw result.Exception;
            }

            var delivery = await getDeliveryByIdQuery.ExecuteAsync(id);
            return new OkObjectResult(delivery);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleteDeliveryCommand = new DeleteDelivery(id);
            await this.messageSession.Send(deleteDeliveryCommand);

            return NoContent();
        }
    }
}
