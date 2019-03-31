using Evento.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services
{
    public interface IEventService
    {
        Task<EventDetailsDTO> GetAsync(Guid Id);
        Task<EventDetailsDTO> GetAsync(string Name);
        Task<IEnumerable<EventDTO>> BrowseAsync(string name = null);
        Task AddTicets(Guid EventId, int Amount, decimal Price);
        Task CreateAsync(Guid Id, string Name, string Descrition, DateTime StartDate, DateTime EndDate);
        Task UpdateAsync(Guid Id, string Name, string Descrition);
        Task DeleteAsync(Guid Id);
    }
}
