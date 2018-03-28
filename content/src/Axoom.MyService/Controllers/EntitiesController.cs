using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Axoom.MyService.Dto;
using Axoom.MyService.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Axoom.MyService.Controllers
{
    /// <summary>
    /// A sample service.
    /// </summary>
    [Route("entities")]
    public class EntitiesController : Controller
    {
        private readonly IEntityService _service;

        public EntitiesController(IEntityService service) => _service = service;

        /// <summary>
        /// Returns all <see cref="Entity"/>s.
        /// </summary>
        [HttpGet, Route("")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<Entity>> ReadAll()
            => await _service.GetAllAsync();

        /// <summary>
        /// Returns a specific <see cref="Entity"/>.
        /// </summary>
        /// <param name="id">The <see cref="Entity.Id"/> to look for.</param>
        [HttpGet, Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified entity not found.")]
        public async Task<Entity> Read([FromRoute] string id)
            => await _service.GetAsync(id);

        /// <summary>
        /// Creates a new <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The new <see cref="Entity"/>.</param>
        [HttpPost, Route("")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, description: "Missing or invalid request body.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified entity not found.")]
        public async Task<ActionResult> Create([FromBody] Entity entity)
        {
            if (entity == null) throw new InvalidDataException("Missing request body.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.AddAsync(entity);

            return CreatedAtAction(
                actionName: nameof(Read),
                routeValues: new {id = result.Id},
                value: entity);
        }

        /// <summary>
        /// Updates an existing <see cref="Entity"/>.
        /// </summary>
        /// <param name="id">The <see cref="Entity.Id"/> of the entity to update.</param>
        /// <param name="entity">The modified <see cref="Entity"/>.</param>
        [HttpPut, Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, description: "Missing or invalid request body.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified entity not found.")]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] Entity entity)
        {
            if (entity == null) throw new InvalidDataException("Missing request body.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (entity.Id != id) throw new InvalidDataException($"ID in URI ({id}) must match the ID in the body ({entity.Id}).");

            await _service.ModifyAsync(entity);

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes an existing <see cref="Entity"/>.
        /// </summary>
        /// <param name="id">The <see cref="Entity.Id"/> of the entity to delete.</param>
        [HttpDelete, Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified entity not found.")]
        public async Task Delete([FromRoute] string id)
            => await _service.RemoveAsync(id);
    }
}
