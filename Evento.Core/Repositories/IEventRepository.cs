using Evento.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evento.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetAsync(Guid Id);
        Task<Event> GetAsync(string Name);
        Task<IEnumerable<Event>> BrowseAsync(string Name = "");
        Task AddAsync(Event @event);
        Task UpdateAsync(Event @event);
        Task DeleteAsync(Event @event);
    }
}
