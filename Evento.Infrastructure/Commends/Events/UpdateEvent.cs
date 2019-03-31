using System;

namespace Evento.Infrastructure.Commends.Events
{
    public class UpdateEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
