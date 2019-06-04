using Evento.Infrastructure.Commends.Events;
using Evento.Infrastructure.DTO;
using Evento.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Evento.Api.Controllers
{
    [EnableCors("Origins")]
    public class EventsController : ApiControllerBase
    {
        private readonly IEventService _eventServices;

        public EventsController(IEventService eventService)
        {
            _eventServices = eventService;
        }

        // GET: api/events/ - Przeglądanie listy wyszstkich wydarzeń
        [HttpGet]
        public async Task<ActionResult<EventDTO>> GetEvents()
        {
            var @event = await _eventServices.BrowseAsync();

            return Json(@event);
        }

        // GET: api/events/5 - Przeglądanie szczegółów wydarzenia po id
        [HttpGet("{EventId}")]
        public async Task<ActionResult<EventDTO>> GetEvent(Guid EventId)
        {
            var @event = await _eventServices.GetAsync(EventId);

            if (@event == null)
                return NotFound();

            return Json(@event);
        }

        // POST: api/events - Dodanie wydarzenia
        [HttpPost]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<ActionResult> PostEvent([FromBody] CreateEvent command)
        {
            command.EventId = Guid.NewGuid();
            await _eventServices.CreateAsync(command.EventId, command.Name,
                command.Description, command.StartDate, command.EndDate);

            await _eventServices.AddTicets(command.EventId, command.Tickets, command.Price);

            return Created($"/events/{command.EventId}", null);
        }

        // PUT: api/events/id - Aktualizacja wydarzenia
        [HttpPut("{EventId}")]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<ActionResult> PutEvent(Guid EventId, [FromBody] UpdateEvent commend)
        {
            await _eventServices.UpdateAsync(EventId, commend.Name, commend.Description);

            return NoContent();
        }

        // DELETE: api/events/id - Usuwanie wydarzenia
        [HttpDelete("{EventId}")]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<ActionResult> DeleteAsync(Guid EventId)
        {
            await _eventServices.DeleteAsync(EventId);

            return NoContent();
        }
    }
}
