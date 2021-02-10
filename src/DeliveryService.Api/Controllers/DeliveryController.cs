namespace DeliveryService.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using DeliveryService.Contracts.Enums;
    using DeliveryService.Contracts.Messages.Commands;
    using DeliveryService.Contracts.Requests;
    using DeliveryService.Contracts.Responses;
    using DeliveryService.Infrastructure.Queries;
    using NServiceBus;

    [Route("[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IMessageSession messageSession;
        private readonly IGetAllDeliveriesQuery getAllDeliveriesQuery;
        private readonly IGetDeliveryByIdQuery getDeliveryByIdQuery;

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

            await this.messageSession.Send(createDeliveryCommand);

            return Accepted();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Delivery>> Put(Guid id, [FromBody] UpdateDeliveryRequest request)
        {
            var updateDeliveryCommand = new UpdateDeliveryState(id, request.State );

            await this.messageSession.Send(updateDeliveryCommand);

            return Accepted();
        }

        [HttpPut("{id}:approve")]
        public async Task<ActionResult<Delivery>> Approve(Guid id)
        {
            // Validate User
            var updateDeliveryCommand = new UpdateDeliveryState(id, DeliveryState.Approved );

            await this.messageSession.Send(updateDeliveryCommand);

            return Accepted();
        }

        [HttpPut("{id}:complete")]
        public async Task<ActionResult<Delivery>> Complete(Guid id)
        {
            // Validate Partner
            var updateDeliveryCommand = new UpdateDeliveryState(id, DeliveryState.Completed );

            await this.messageSession.Send(updateDeliveryCommand);

            return Accepted();
        }

        [HttpPut("{id}:cancel")]
        public async Task<ActionResult<Delivery>> Cancel(Guid id)
        {
            // Validate User or Partner
            var updateDeliveryCommand = new UpdateDeliveryState(id, DeliveryState.Cancelled );

            await this.messageSession.Send(updateDeliveryCommand);

            return Accepted();
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
