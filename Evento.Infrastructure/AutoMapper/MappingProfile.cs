using AutoMapper;
using Evento.Core.Domain;
using Evento.Infrastructure.DTO;
using System.Linq;

namespace Evento.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDTO>()
                .ForMember(
                    x => x.TicketsCount,
                    y => y.MapFrom(p => p.Tickets.Count())
                );
            CreateMap<Event, EventDetailsDTO>();
            CreateMap<Ticket, TicketDTO>();
        }
    }
}
