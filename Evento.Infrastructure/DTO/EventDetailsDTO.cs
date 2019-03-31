using System.Collections.Generic;

namespace Evento.Infrastructure.DTO
{
    public class EventDetailsDTO : EventDTO
    {
        public IEnumerable<TicketDTO> Tickets { get; set; }
    }
}
