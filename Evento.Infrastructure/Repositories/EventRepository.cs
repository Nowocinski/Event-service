using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Evento.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly DataBaseContext _context;

        public EventRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Event> GetAsync(Guid Id)
        //=> await Task.FromResult(_context.Events.SingleOrDefault(x => x.Id == Id));
        {
            var @event = await _context.Events.SingleOrDefaultAsync(x => x.Id == Id);
            if(@event != null)
            {
                var ticets = await _context.Tickets.Where(x => x.EventId == @event.Id).ToListAsync();
                @event.LoadTickets(ticets);
            }
            return await Task.FromResult(@event);
        }

        public async Task<Event> GetAsync(string Name)
        //=> await Task.FromResult(_context.Events.SingleOrDefault(x => x.Name.ToLower() == Name.ToLower()));
        {
            var @event = await _context.Events.SingleOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
            if(@event != null)
            {
                var ticets = await _context.Tickets.Where(x => x.EventId == @event.Id).ToListAsync();
                @event.LoadTickets(ticets);
            }
            return await Task.FromResult(@event);
        }

        public async Task<IEnumerable<Event>> BrowseAsync(string Name = "")
        {
            var events = await _context.Events.ToListAsync();

            if(!string.IsNullOrWhiteSpace(Name))
                events = events.Where(x => x.Name.Contains(Name.ToLower())).ToList();

            return await Task.FromResult(events);
        }

        public async Task AddAsync(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Event @event)
        {
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Event @event)
        {
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
