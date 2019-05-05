using System;

namespace Evento.Infrastructure.Commends.Events
{
    public class CreateEvent : ICommand
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Tickets { get; set; }
        public decimal Price { get; set; }
    }
}
