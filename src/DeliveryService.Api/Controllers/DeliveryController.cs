using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Contracts.Requests;
using DeliveryService.Contracts.Responses;

namespace DeliveryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        // GET api/aggreatename
        [HttpGet]
        public IActionResult Get()
        {
            var aggregatenames = new List<Delivery>
            {
                new Delivery { Id = Guid.NewGuid(), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow },
                new Delivery { Id = Guid.NewGuid(), CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow },
            };
            return Ok(aggregatenames);
        }

        // GET api/aggregatename/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var aggregatename = new Delivery { Id = id, CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow };
            return Ok(aggregatename);
        }

        // POST api/aggreatename
        [HttpPost]
        public IActionResult Post([FromBody] CreateDeliveryRequest request)
        {
            var aggregatename = new Delivery { Id = request.Id, CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow };
            return CreatedAtAction("Get", new { id = aggregatename.Id }, aggregatename);
        }

        // PUT api/aggreatename/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return NoContent();
        }

        // DELETE api/aggreatename/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
