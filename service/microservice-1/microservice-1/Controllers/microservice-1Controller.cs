using Atlas.Auditing;
using Atlas.Controllers.Responses;
using Atlas.Logging;
using microservice_1.Auditing;
using microservice_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace microservice_1.Controllers
{
    /// <inheritdoc />
    [Route("things")]
    public class microservice-1Controller : Controller
    {
        private const string RouteGetThing = nameof(GetThing);

        private static readonly ILogger _log = LoggingFactory.CreateLogger();
        private readonly IAuditor<microservice-1Controller> _auditor;

        private static readonly Dictionary<long, string> _things = new Dictionary<long, string>();
        private static long _counter;

        /// <inheritdoc />
        public microservice-1Controller(IAuditor<microservice-1Controller> auditor)
        {
            _auditor = auditor;
        }

		/// <summary>
        /// Get all things
        /// </summary>
        /// <response code="200">Returns a list of things</response>
        [HttpGet]
        [Route("", Name = nameof(GetAllThings))]
        [ProducesResponseType(typeof(ApiOkResponse<IEnumerable<ThingResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllThings(CancellationToken token)
        {
            await Task.CompletedTask;

            var things = _things
                    .Select(pair => new ThingResponse { Id = pair.Key, Name = pair.Value })
                    .ToArray()
                    .AsEnumerable();

            return Respond.Ok(things);
        }

        /// <summary>
        /// Get a thing
        /// </summary>
        /// <response code="200">Returns the details for a thing</response>
        /// <response code="404">If the thing was not found</response>
        [HttpGet]
        [Route("{thingId:long}", Name = RouteGetThing)]
        [ProducesResponseType(typeof(ApiOkResponse<ThingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiNotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetThing(long thingId, CancellationToken token)
        {
            await Task.CompletedTask;

            _log.LogDebug($"GetThing is requesting id '{thingId}'"); // just an example 

            if (!_things.TryGetValue(thingId, out var thingName))
            {
                return Respond.NotFound($"Thing with id '{thingId}' not found.");
            }

            return Respond.Ok(new ThingResponse{Id = thingId, Name = thingName});
        }

        /// <summary>
        /// Create a thing
        /// </summary>
        /// <response code="201">Returns the details for the new thing</response>
        /// <response code="404">If an error occured</response>
        [HttpPost]
        [Route("", Name = nameof(CreateThing))]
        [ProducesResponseType(typeof(ApiCreatedResponse<ThingResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateThing([FromBody]CreateThingRequest createRequest, CancellationToken token)
        {
            var newThingId = ++_counter;
            _things[newThingId] = createRequest.Name;

            // audit
            await _auditor.ThingCreated(newThingId, createRequest.Name, true);

            var response = new ThingResponse{Id = newThingId, Name=createRequest.Name};
            return Respond.CreatedAtRoute(RouteGetThing, new {thingId = newThingId}, response);
        }

        /// <summary>
        /// Update a thing
        /// </summary>
        /// <response code="200">If the thing was updated</response>
        /// <response code="400">If the request was bad</response>
        /// <response code="404">If the thing was not found</response>
        [HttpPut]
        [Route("{thingId:long}", Name = nameof(UpdateThing))]
        [ProducesResponseType(typeof(ApiOkResponse<ThingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiNotFoundResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateThing(long thingId, [FromBody]UpdateThingRequest updateRequest, CancellationToken token)
        {
            if (!_things.ContainsKey(thingId))
            {
                await _auditor.ThingRenamed(thingId, updateRequest.NewName, "", false);
                return Respond.NotFound($"Thing with id '{thingId}' not found.");
            }

            var oldName = _things[thingId];
            _things[thingId] = updateRequest.NewName;

            // audit
            await _auditor.ThingRenamed(thingId, updateRequest.NewName, oldName, true);

            var response = new ThingResponse { Id = thingId, Name = updateRequest.NewName};
            return Respond.Ok(response);
        }

        /// <summary>
        /// Delete a thing
        /// </summary>
        /// <response code="200">If the thing was deleted</response>
        /// <response code="404">If the thing was not found</response>
        [HttpDelete]
        [Route("{thingId:long}", Name = nameof(DeleteThing))]
        [ProducesResponseType(typeof(ApiOkResponse<ThingResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiBadRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteThing(long thingId, CancellationToken token)
        {
            if (!_things.ContainsKey(thingId))
            {
                await _auditor.ThingDeleted(thingId, string.Empty, false);
                return Respond.NotFound($"Thing with id '{thingId}' not found.");
            }

            var thingName = _things[thingId];
            _things.Remove(thingId);

            // audit
            await _auditor.ThingDeleted(thingId, thingName, true);

            var response = new ThingResponse { Id = thingId, Name = thingName };
            return Respond.Ok(response);
        }
    }
}
