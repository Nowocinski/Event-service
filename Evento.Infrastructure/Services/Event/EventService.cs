using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Infrastructure.DTO;

namespace Evento.Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<EventDetailsDTO> GetAsync(Guid Id)
        {
            var @event = await _eventRepository.GetAsync(Id);
            return _mapper.Map<EventDetailsDTO>(@event);
        }

        public async Task<EventDetailsDTO> GetAsync(string Name)
        {
            var @event = await _eventRepository.GetAsync(Name);
            return _mapper.Map<EventDetailsDTO>(@event);
        }

        public async Task<IEnumerable<EventDTO>> BrowseAsync(string name = null)
        {
            var events = await _eventRepository.BrowseAsync(name);
            return _mapper.Map<IEnumerable<EventDTO>>(events);
        }

        public async Task AddTicets(Guid EventId, int Amount, decimal Price)
        {
            var @event = await _eventRepository.GetAsync(EventId);
            if (@event == null)
                throw new Exception($"Exception with id: '{EventId}' does not exists");

            @event.AddTickets(Amount, Price);
            await _eventRepository.UpdateAsync(@event);
        }

        public async Task CreateAsync(Guid Id, string Name, string Description, DateTime StartDate, DateTime EndDate)
        {
            var @event = await _eventRepository.GetAsync(Name);
            if (@event != null)
                throw new Exception($"Exception named: '{Name}' already exists");

            @event = new Event(Id, Name, Description, StartDate, EndDate);
            await _eventRepository.AddAsync(@event);
        }

        public async Task UpdateAsync(Guid Id, string Name, string Descrition)
        {
            var @event = await _eventRepository.GetAsync(Name);
            if (@event != null)
                throw new Exception($"Exception named: '{Name}' already exists");

            @event = await _eventRepository.GetAsync(Id);
            if (@event == null)
                throw new Exception($"Exception with id: '{Id}' does not exists");

            @event.SetName(Name);
            @event.SetDescription(Descrition);
            await _eventRepository.UpdateAsync(@event);
        }

        public async Task DeleteAsync(Guid Id)
        {
            var @event = await _eventRepository.GetAsync(Id);
            if (@event == null)
                throw new Exception($"Exception id: '{Id}' does not exists");

            await _eventRepository.DeleteAsync(@event);
        }
    }
}
